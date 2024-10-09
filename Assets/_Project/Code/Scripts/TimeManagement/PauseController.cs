using System;
using DrawCar.Ad;
using DrawCar.Audio;
using DrawCar.Car;
using DrawCar.Car.Input;
using DrawCar.Level;
using DrawCar.Utils;
using VContainer;
using VContainer.Unity;

namespace Code.Scripts.TimeManagement
{
    public interface IPauseController
    {
        void SetPause(in bool value, in bool affectLockInputs, in bool affectCar, in bool stopTimer, in bool byUi = true);
        void StopTimer(bool isStopped);
    }
    
    public sealed class PauseController : IPauseController, IDisposable, IStartable
    {
        private readonly IAdService _adService;
        private readonly IRecordTimer _timer;
        private IAudioController _audioController;
        private IInputService _inputService;
        private ICarController _carController;
        
        private bool _isPaused;
        private bool _isPausedByUi;
        
        [Inject]
        public PauseController(
            IAdService adService,
            IRecordTimer timer,
            IStageService stageService,
            IAudioController audioController,
            IInputService inputService,
            ICarController carController)
        {
            _adService = adService;
            _timer = timer;
            _audioController = audioController;
            _inputService = inputService;
            _carController = carController;
            
            this.LogInjectSuccess();
        }
        
        private void OnPauseByAd()
        {
            if(_isPaused) return;
            SetPause(true, true, true, true, false);
        }

        private void OnUnpauseByAd()
        {
            if(_isPausedByUi) return;
            SetPause(false, true, true, true, false);
        }

        public void SetPause(in bool value, in bool affectLockInputs, in bool affectCar, in bool stopTimer, in bool byUi = true)
        {
            _isPaused = value;
            _isPausedByUi = byUi;
            
            if(affectLockInputs)
                _inputService.LockInputs(value);
            
            if(affectCar)
            {
                _audioController.StopCarSfx(value);
            }
            
            if(stopTimer)
                StopTimer(value);
        }

        public void StopTimer(bool isStopped)
        {
            _timer.Stop(isStopped);
        }

        private void SubscribeEvents()
        {
            _adService.OnInterstitialBannerOpenedAction += OnPauseByAd;
            _adService.OnInterstitialBannerClosedAction += OnUnpauseByAd;
        }
        
        private void UnsubscribeEvents()
        {
            _adService.OnInterstitialBannerOpenedAction -= OnPauseByAd;
            _adService.OnInterstitialBannerClosedAction -= OnUnpauseByAd;
        }

        public void Dispose()
        {
            UnsubscribeEvents();
        }

        public void Start()
        {
            SubscribeEvents();
        }
    }
}