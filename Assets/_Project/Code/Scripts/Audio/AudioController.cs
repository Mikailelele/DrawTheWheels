using System;
using System.Runtime.CompilerServices;
using DrawCar.Car.Input;
using DrawCar.Utils;
using VContainer;

namespace DrawCar.Audio
{
    public interface IAudioController 
    {
        void StopCarSfx(bool isStopped);
    }
    public sealed class AudioController : IAudioController, IDisposable
    {
        private IAudioService _audioService;
        private IInputService _inputService;
        private IDisposable _disposable;

        [Inject]
        private void Construct(
            IAudioService audioService,
            IInputService inputService)
        {
            _audioService = audioService;
            _inputService = inputService;

            SubscribeCarInputEvents();
            
            this.LogInjectSuccess();
        }
        
        private void UpdateCarSfxPitch()
        {
            SetCarSfxPitch(_inputService.Drive.z != 0);
        }
        
        public void StopCarSfx(bool isStopped)
        {
            if(isStopped)
                UnsubscribeCarInputEvents();
            else
                SubscribeCarInputEvents();

            SetCarSfxPitch(false);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetCarSfxPitch(bool isPlaying)
        {
            _audioService.SetPitch(EAudioType.Drive, isPlaying ? 1 : 0);
        }
 
        private void SubscribeCarInputEvents()
        { 
            _inputService.OnDriveAction += UpdateCarSfxPitch;
        }
        
        private void UnsubscribeCarInputEvents()
        {
            _inputService.OnDriveAction -= UpdateCarSfxPitch;
        }
        
        public void Dispose()
        {
            UnsubscribeCarInputEvents();
        }
    }
}