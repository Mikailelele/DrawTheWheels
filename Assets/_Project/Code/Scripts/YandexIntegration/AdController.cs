using System;
using Code.Scripts.TimeManagement;
using DrawCar.Ad;
using DrawCar.Audio;
using DrawCar.Utils;
using UnityEngine;
using VContainer;
using YG;

namespace Code.Scripts.YandexIntegration
{
    public sealed class AdController : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _uiCanvasGroup;

        private IAdService _adService;
        private IAudioService _audioService;
        private IPauseController _pauseController;
        
        [Inject]
        private void Construct(
            IAdService adService,
            IAudioService audioService,
            IPauseController pauseController)
        {
            _adService = adService;
            _audioService = audioService;
            _pauseController = pauseController;
            
            SubscribeEvents();
            this.LogInjectSuccess();
        }

        private void OnDestroy()
        {
            UnsubscribeEvents();
        }
        
        public void PauseGame()
        {
            this.Log("PauseGame");
            _uiCanvasGroup.blocksRaycasts = false;
            _audioService.PauseAll(true, false);
            TimeService.SetPause(true);
        }
        
        private void UnpauseGame()
        {
            this.Log("UnpauseGame");
            _uiCanvasGroup.blocksRaycasts = true;
            _audioService.PauseAll(YandexGame.savesData.IsAudioMuted, false);
            TimeService.SetPause(false);
        }
        
        private void SubscribeEvents()
        {
            _adService.OnInterstitialBannerClosedAction += UnpauseGame;
        }

        private void UnsubscribeEvents()
        {
            _adService.OnInterstitialBannerClosedAction -= UnpauseGame;
        }
    }
}