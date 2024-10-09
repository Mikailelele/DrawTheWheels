using System;
using DrawCar.Audio;
using DrawCar.Car;
using DrawCar.Car.Input;
using DrawCar.Ui.Messages;
using MessagePipe;
using UnityEngine;
using VContainer;

namespace DrawCar.Level
{
    public sealed class LevelStarter : MonoBehaviour
    { 
        private ICarController _carController;
        private IAudioService _audioService;
        private IInputService _inputService;
        private IDisposable _disposable;
        
        [Inject]
        private void Construct(
            ISubscriber<OnUiActionMessage> onUiActionMessage,
            IInputService inputService,
            IAudioService audioService,
            ICarController carController
            )
        {
            var bag = DisposableBag.CreateBuilder();
            onUiActionMessage.Subscribe(HandleUiAction).AddTo(bag);
            _disposable = bag.Build();
            
            _inputService = inputService;
            _audioService = audioService;
            _carController = carController;
        }
        
        private void Start()
        {
            FreezeCar();
        }
        
        private void FreezeCar()
        {
            _inputService.LockMouseRotation(true);
            _carController.Stop(true);
        }
        
        private void HandleUiAction(OnUiActionMessage message)
        {
            if(message.Type != EUiActionType.CloseDrawCanvas) return;
            
            _audioService.Init();
            _carController.Stop(false);
            _inputService.LockMouseRotation(false);
            
            _disposable.Dispose();
        }
    }
}