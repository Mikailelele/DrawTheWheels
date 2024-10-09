using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DrawCar.Car.Input;
using DrawCar.ScriptableObjects;
using DrawCar.Utils;
using UnityEngine;
using VContainer;

namespace DrawCar.Car
{
    public interface ICarController
    {
        void ResetCar();
        void Stop(bool isStopped);
        
        Transform Transform { get; }
        List<Transform> WheelsPositions { get; }
    }
    
    [SelectionBase]
    [DisallowMultipleComponent]
    public sealed class CarController : MonoBehaviour, ICarController
    {
        [SerializeField] private Rigidbody _carRigidbody;
        [SerializeField] private Rigidbody[] _wheels;
        [SerializeField] private Transform[] _steeringWheels;
        
        private ICarSettings _carSettings;
        private IInputService _inputService;
        
        private Vector3 _driveValue;
        private Vector2 _steerValue;
        private Vector3 _cachedCurrentWheelRotation;
        
        private int _maxAngularVelocity;

        private Transform _transform;
        
        public Transform Transform => _transform;
        public List<Transform> WheelsPositions { get; } = new();

        [Inject]
        private void Construct(
            IInputService inputService, 
            ICarSettings carSettings)
        {
            _inputService = inputService;
            _carSettings = carSettings;
            
            this.LogInjectSuccess();
        }
        
        private void Awake()
        {
            _transform = transform;
            SetMaxAngularVelocity(_carSettings.Acceleration);

            foreach (var wheel in _wheels)
            {
                WheelsPositions.Add(wheel.transform);
            }
        }
        
        private void FixedUpdate()
        {
            Drive();
        }

        private void Update()
        {
            UpdateSteering();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Drive()
        {
            _driveValue = _inputService.Drive;
            if(_driveValue == Vector3.zero) return;
            
            Span<Rigidbody> wheels = _wheels;

            for (int i = 0, count = wheels.Length; i < count; ++i)
            {
                wheels[i].AddRelativeTorque(_driveValue * (-_carSettings.Acceleration * Time.fixedDeltaTime), ForceMode.Impulse);
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateSteering()
        {
            _steerValue = _inputService.Steer;
            Span<Transform> steeringWheels = _steeringWheels;
            
            for (int i = 0; i < steeringWheels.Length; i++)
            {
                Steer(steeringWheels[i],
                    Quaternion.Euler(0, _steerValue.y * _carSettings.SteeringAngle, 0));
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Steer(in Transform wheelTransform, in Quaternion rotation)
        {
            _cachedCurrentWheelRotation = wheelTransform.localRotation.eulerAngles;
            
            _cachedCurrentWheelRotation.y = Mathf.MoveTowardsAngle(_cachedCurrentWheelRotation.y, rotation.eulerAngles.y, _carSettings.SteeringSpeed * Time.deltaTime);
            wheelTransform.localRotation = Quaternion.Euler(_cachedCurrentWheelRotation);
        }
        
        public void ResetCar()
        {
            _carRigidbody.velocity = Vector3.zero;
            _carRigidbody.angularVelocity = Vector3.zero;
            
            Span<Rigidbody> wheels = _wheels;
            Transform wheelTransform;
            Rigidbody wheelBaseRigidbody;
            for (int i = 0, count = wheels.Length; i < count; ++i)
            {
                wheels[i].angularVelocity = Vector3.zero;
                wheels[i].velocity = Vector3.zero;
                
                wheelTransform = wheels[i].transform;
                if(wheelTransform.childCount > 0)
                {
                    wheelTransform.localRotation = Quaternion.identity;
                    wheelTransform.GetChild(0).TryGetComponent(out wheelBaseRigidbody);
                    wheelBaseRigidbody.angularVelocity = Vector3.zero;
                    wheelBaseRigidbody.velocity = Vector3.zero;
                }
            }
        }
        
        public void Stop(bool isStopped)
        {
            _carRigidbody.isKinematic = isStopped;
        }
        
        private void SetMaxAngularVelocity(int value)
        {
            foreach (var wheel in _wheels)
            {
                wheel.maxAngularVelocity = value;
            }
        }
    }
}