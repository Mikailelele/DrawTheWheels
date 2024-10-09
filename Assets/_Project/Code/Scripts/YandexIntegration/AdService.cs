using System;
using VContainer.Unity;
using YG;

namespace DrawCar.Ad
{
    public interface IAdService
    {
        void ShowRewardBasedVideo(int rewardIndex);
        void ShowInterstitialBanner();
        
        event Action OnInterstitialBannerOpenedAction;
        event Action OnInterstitialBannerClosedAction;
    }
    
    public sealed class AdService : IAdService, IInitializable, IDisposable
    {
        public event Action OnInterstitialBannerOpenedAction = () => { };
        public event Action OnInterstitialBannerClosedAction = () => { };
        
        public void Initialize()
        {
            YandexGame.RewardVideoEvent += OnRewardVideoCompleted;
            YandexGame.OpenFullAdEvent += OnInterstitialBannerOpened;
            YandexGame.CloseFullAdEvent += OnInterstitialBannerClosed;
        }

        public void ShowRewardBasedVideo(int rewardIndex)
        {
            YandexGame.RewVideoShow(rewardIndex);
        }
        
        public void ShowInterstitialBanner()
        {
            YandexGame.FullscreenShow();
        }
        
        private void OnRewardVideoCompleted(int rewardIndex) {}
        private void OnInterstitialBannerOpened() => OnInterstitialBannerOpenedAction.Invoke();
        private void OnInterstitialBannerClosed() => OnInterstitialBannerClosedAction.Invoke();
        
        public void Dispose()
        {
            YandexGame.RewardVideoEvent -= OnRewardVideoCompleted;
            YandexGame.OpenFullAdEvent -= OnInterstitialBannerOpened;
            YandexGame.CloseFullAdEvent -= OnInterstitialBannerClosed;
        }
    }
}
