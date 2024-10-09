using System;
using System.Collections.Generic;
using DrawCar.Drawing.Messages;
using DrawCar.Level;
using DrawCar.Level.Messages;
using DrawCar.ScriptableObjects;
using MessagePipe;
using NTC.Pool;
using UnityEngine;
using VContainer;

namespace DrawCar.Car
{
    public sealed class CarSpawner : MonoBehaviour
    { 
        private readonly List<GameObject> _currentWheels = new();
        
        private ICarController _carController;
        private ICarSettings _carSettings;

        private IStageService _stageService;
        private IDisposable _disposable;

        [Inject]
        private void Construct(
            ISubscriber<OnWheelGeneratedMessage> onMeshGeneratedMessage,
            ISubscriber<OnStageResetMessage> onStageResetMessage,
            ICarController carController,
            IStageService stageService,
            ICarSettings carSettings)
        {
            _carController = carController;
            _stageService = stageService;
            _carSettings = carSettings;

            var bag = DisposableBag.CreateBuilder();
            onMeshGeneratedMessage.Subscribe(ChangeWheels).AddTo(bag);
            onStageResetMessage.Subscribe(OnSetCarToStartedPosition).AddTo(bag);
            
            _disposable = bag.Build(); 
        }
        
        private void OnDestroy()
        {
            _disposable.Dispose();
        }

        private void ChangeWheels(OnWheelGeneratedMessage message)
        {
            _stageService.RestartStage();
            PlaceWheels(message.GeneratedWheels);
        }

        private void PlaceWheels(GameObject[] generatedWheels)
        {
            DespawnPreviousWheels();
            
            GameObject wheel;
            Transform wheelTransform;
            for (int i = 0; i < 4; i++)
            {
                wheel = generatedWheels[i];
                wheelTransform = wheel.transform;
                
                wheelTransform.SetParent(_carController.WheelsPositions[i]);
                wheelTransform.localPosition = Vector3.zero;
                wheelTransform.localRotation = Quaternion.identity;
                
                wheel.GetComponent<FixedJoint>().connectedBody =
                    _carController.WheelsPositions[i].GetComponent<Rigidbody>();
                _currentWheels.Add(wheel);
            }
        }

        private void SetCarToStartedPosition(Vector3 position)
        {
            var carTransform = _carController.Transform;
            carTransform.position = position + _carSettings.CarSpawnOffset;
            carTransform.rotation = Quaternion.identity;
            
            _carController.ResetCar();
        }
        
        private void OnSetCarToStartedPosition(OnStageResetMessage message)
        {
            SetCarToStartedPosition(message.StagePosition);
        }

        private void DespawnPreviousWheels()
        {
            Transform wheel;
            for (int i = 0; i < _currentWheels.Count; i++)
            {
                wheel = _currentWheels[i].transform;
                for (int j = 0; j < wheel.childCount; j++)
                {
                    NightPool.Despawn(wheel.GetChild(j));
                }
                
                Destroy(_currentWheels[i]);
            }
            _currentWheels.Clear();
        }
    }
}