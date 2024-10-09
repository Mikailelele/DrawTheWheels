using System;
using DrawCar.Car.Input;
using DrawCar.Level.Messages;
using DrawCar.Ui.Messages;
using MessagePipe;
using UnityEngine;
using VContainer;
using YG;

namespace DrawCar.Camera
{
    public sealed class CameraManager : MonoBehaviour
    {
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private Transform _playerTransform;
        
        private IInputService _inputService;
        
        private bool _isHardLock;

        private IDisposable _disposable;
        
        [Inject]
        private void Construct(
            ISubscriber<OnUiActionMessage> onUiActionSubscriber, 
            ISubscriber<OnStageResetMessage> onStageResetSubscriber,
            IInputService inputService)
        {
            _inputService = inputService;
            _inputService.OnHoldCursorAction += OnHoldCursorAction;
            
            var bag = DisposableBag.CreateBuilder();
            onUiActionSubscriber.Subscribe(OnUiActionMessage).AddTo(bag);
            onStageResetSubscriber.Subscribe(OnResetCamera).AddTo(bag);
            
            _disposable = bag.Build();
        }

        private void Start()
        {
            if (YandexGame.EnvironmentData.isMobile)
                _cameraController.SetMobileSensitivity();
            else
                _cameraController.LockCamera(true); 
        }

        private void OnDestroy()
        {
            _inputService.OnHoldCursorAction -= OnHoldCursorAction;
            
            _disposable.Dispose();
        }

        private void OnHoldCursorAction(bool value)
        {
            if(_isHardLock)
                return;
            _cameraController.LockCamera(!value);
        }
        
        
        private void OnUiActionMessage(OnUiActionMessage message)
        {
            _isHardLock = message.Type switch
            {
                EUiActionType.OpenDrawCanvas => true,
                
                EUiActionType.CloseDrawCanvas => false,
                _ => _isHardLock
            };
        }
        
        private void OnResetCamera(OnStageResetMessage message)
        {
            _cameraController.ResetRotation().Forget();
        }
    }
}