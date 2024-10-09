using System;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer.Unity;
using YG;

namespace DrawCar.Car.Input
{
    public interface IInputService
    {
        Vector3 Drive { get; }
        Vector2 Steer { get; }
        
        event Action<bool> OnHoldCursorAction;
        event Action OnDriveAction;
        
        void Init(bool isMobile);
        void LockInputs(bool isLocked);
        void LockMouseRotation(bool value);
    }
    
    public sealed class InputService : IInputService, IInitializable, IDisposable
    {
        private ProjectInput _input;
        
        public Vector3 Drive => _input.Player.Drive.ReadValue<Vector3>();
        public Vector2 Steer => _input.Player.Steer.ReadValue<Vector2>();

        public event Action<bool> OnHoldCursorAction;
        public event Action OnDriveAction = () => { };
        
        public void Initialize()
        {
            _input = new ProjectInput();
            _input.Enable();
        }
        
        public void Init(bool isMobile)
        {
            if (isMobile)
                SubscribeMobileEvents();
            else
                SubscribeDesktopEvents();
        }
        
        public void LockInputs(bool isLocked)
        {
            if (isLocked)
                _input.Disable();
            else
                _input.Enable();
        }

        public void LockMouseRotation(bool value)
        {
            if(YandexGame.EnvironmentData.isMobile) return;
            
            if (value)
                UnsubscribeDesktopEvents();
            else
                SubscribeDesktopEvents();
        }

        private void OnHoldCursorPerformed(InputAction.CallbackContext ctx)
        {
            OnHoldCursorAction?.Invoke(true);
        }
        
        private void OnHoldCursorCanceled(InputAction.CallbackContext ctx)
        {
            OnHoldCursorAction?.Invoke(false);
        }
        
        private void OnDrivePerformed(InputAction.CallbackContext ctx)
        {
            OnDriveAction.Invoke();
        }
        
        private void SubscribeDesktopEvents()
        {
            _input.Player.HoldCursor.performed += OnHoldCursorPerformed;
            _input.Player.HoldCursor.canceled += OnHoldCursorCanceled;
            
            _input.Player.Drive.performed += OnDrivePerformed;
        }
        
        private void UnsubscribeDesktopEvents()
        {
            _input.Player.HoldCursor.performed -= OnHoldCursorPerformed;
            _input.Player.HoldCursor.canceled -= OnHoldCursorCanceled;
            
            _input.Player.Drive.performed -= OnDrivePerformed;
        }

        private void SubscribeMobileEvents()
        {
            _input.Player.Drive.performed += OnDrivePerformed;
        }
        
        private void UnsubscribeMobileEvents()
        {
            _input.Player.Drive.performed -= OnDrivePerformed;
        }
        
        public void Dispose()
        {
            if(!YandexGame.EnvironmentData.isMobile)
                UnsubscribeDesktopEvents();
            else 
                UnsubscribeMobileEvents();
            
            _input.Disable();
        }
    }
}