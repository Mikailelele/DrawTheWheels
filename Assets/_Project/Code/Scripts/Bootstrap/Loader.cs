using Cysharp.Threading.Tasks;
using DrawCar.Car.Input;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using YG;

namespace DrawCar.Bootstrap
{
    public sealed class Loader : MonoBehaviour
    { 
        private void OnEnable() => YandexGame.GetDataEvent += OnYandexInitialized;
        private void OnDisable() => YandexGame.GetDataEvent -= OnYandexInitialized;

        private IInputService _inputService;
        
        [Inject]
        private void Construct(IInputService inputService)
        {
            _inputService = inputService;
        }

        private void Awake()
        {
            if (YandexGame.SDKEnabled)
            {
                OnYandexInitialized();
            }
        }

        private async void OnYandexInitialized()
        {
            SetOptimizationSettings();

            _inputService.Init(YandexGame.EnvironmentData.isMobile);
            await SceneManager.LoadSceneAsync($"Level_{YandexGame.savesData.LevelNumber}");
            YandexGame.GameReadyAPI();
        }
        
        private void SetOptimizationSettings()
        {
            Screen.SetResolution(Screen.width / 2, Screen.height / 2, true);
        }
    }
}