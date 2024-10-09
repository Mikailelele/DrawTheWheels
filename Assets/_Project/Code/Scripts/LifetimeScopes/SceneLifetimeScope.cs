using System.Linq;
using Code.Scripts.Leaderboard;
using Code.Scripts.TimeManagement;
using Code.Scripts.YandexIntegration;
using DrawCar.Ad;
using DrawCar.Audio;
using DrawCar.Car;
using DrawCar.Data;
using DrawCar.Level;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using VInspector;

namespace DrawCar.LifetimeScopes
{
    public sealed class SceneLifetimeScope : LifetimeScope
    {
        [SerializeField] private GameObject _levelSetuper;
        [SerializeField] private CarController _carPrefab;
        [SerializeField] private GameObject _uiPrefab;
        
        [SerializeField] private StagesData _stagesData;

        private IContainerBuilder _builder;
        
        protected override void Configure(IContainerBuilder builder)
        {
            _builder = builder;
            
            RegisterRecordTimer();
            RegisterStagesService();

            InstantiateCar();

            RegisterPauseController();
            
            RegisterLeaderboardService();
            
            RegisterAudioService();
            RegisterAudioController();
            
            RegisterAdService();
            
            InstantiateLevelSetuper();
            InstantiateUi();
        }
        
        private void InstantiateUi()
        {
            _builder.RegisterBuildCallback(
                container =>
                {
                    container.Instantiate(_uiPrefab);
                });
        }
        
        private void InstantiateCar()
        {
            _builder.RegisterComponentInNewPrefab(_carPrefab, Lifetime.Singleton)
                .AsImplementedInterfaces()
                .AsSelf();
            
            _builder.RegisterBuildCallback(
                container =>
                {
                    var carController = container.Resolve<CarController>();
                    container.InjectGameObject(carController.gameObject);
                });
        }

        private void InstantiateLevelSetuper()
        {
            _builder.RegisterBuildCallback(
                container =>
                {
                    container.Instantiate(_levelSetuper);
                });
        }
        
        private void RegisterAudioService()
        {
            _builder.RegisterComponentOnNewGameObject<AudioService>(Lifetime.Singleton)
                .AsImplementedInterfaces();
        }
        
        private void RegisterAudioController()
        {
            _builder.Register<AudioController>(Lifetime.Singleton)
                .As<IAudioController>();
            
            _builder.RegisterBuildCallback(
                container =>
                {
                    container.Resolve<IAudioController>();
                });
        }
        
        private void RegisterAdService()
        {
            _builder.RegisterEntryPoint<AdService>()
                .As<IAdService>();
        }
        
        
        private void RegisterStagesService()
        {
            _builder.RegisterEntryPoint<StageService>()
                .As<IStageService>()
                .WithParameter(_stagesData);
        }
        
        private void RegisterRecordTimer()
        {
            _builder.RegisterEntryPoint<RecordTimer>()
                .As<IRecordTimer>();
        }
        
        private void RegisterPauseController()
        {
            _builder.RegisterEntryPoint<PauseController>()
                .As<IPauseController>();
            
            _builder.RegisterBuildCallback(
                container =>
                {
                    container.Resolve<IPauseController>();
                });
        }
        
        private void RegisterLeaderboardService()
        {
            _builder.Register<LeaderboardService>(Lifetime.Singleton);
            
            _builder.RegisterBuildCallback(
                container =>
                {
                    container.Resolve<LeaderboardService>();
                });
        }

#if UNITY_EDITOR
        [Button]
        private void FillStageData()
        {
            _stagesData.TriggerZones = FindObjectsOfType<TriggerZone>();
            
            _stagesData.TriggerZones = _stagesData.TriggerZones.OrderBy(e => e.StageIndex).ToArray();
        }
#endif
    }
}
