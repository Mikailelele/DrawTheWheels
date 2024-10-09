using System;
using Code.Scripts.TimeManagement;
using DrawCar.Data;
using DrawCar.Level.Messages;
using DrawCar.YandexIntegration;
using MessagePipe;
using VContainer.Unity;
using YG;

namespace DrawCar.Level
{
    public interface IStageService
    {
        void RestartStage();
        void RestartFromFirstStage();
        void StartNextStage(in int stageIndex);
        
        bool LastStageIsAchieved { get; }
        
        event Action OnAchievedFinalStage;
        event Action OnRestartedFromFirstStage;
    }

    public sealed class StageService : IStageService, IInitializable, IStartable, IDisposable
    {
        private readonly StagesData _stagesData;
        private readonly IRecordTimer _recordTimer;

        private int _currentStageIndex;
        public bool LastStageIsAchieved { get; private set; }

        private int CurrentStageIndex
        {
            get => _currentStageIndex;
            set
            {
                _currentStageIndex = value;
                SavingService.SetInt(ESaveType.CurrentStage, value);
            }
        }

        private bool StageCompleted(int stageIndex) => CurrentStageIndex >= stageIndex;
        private bool IsLastStage(int stageIndex) => stageIndex >= _stagesData.TriggerZones.Length;
        

        private IPublisher<OnStageResetMessage> _stageResetPublisher;

        public event Action OnAchievedFinalStage = () => { };
        public event Action OnRestartedFromFirstStage = () => { };

        public StageService(
            StagesData stagesData,
            IPublisher<OnStageResetMessage> stageResetPublisher,
            IRecordTimer recordTimer)
        {
            _stagesData = stagesData;
            _recordTimer = recordTimer;
            _stageResetPublisher = stageResetPublisher;
        }

        public void Initialize()
        {
            YandexGame.GetDataEvent += OnYandexInitialized;
        }

        public void Start()
        {
            if (YandexGame.SDKEnabled)
            {
                OnYandexInitialized();
            }
        }
        
        public void RestartStage()
        {
            _stageResetPublisher.Publish(
                new OnStageResetMessage(_stagesData.TriggerZones[CurrentStageIndex].CenterPosition));
            
            _recordTimer.RestartToLastSavedTime();
        }
        
        public void RestartFromFirstStage()
        {
            _recordTimer.Reset();
            _recordTimer.Stop(false, true);
            
            OnRestartedFromFirstStage.Invoke();
            LastStageIsAchieved = false;
            RestartStage();
        }

        public void StartNextStage(in int stageIndex)
        {
            if (StageCompleted(stageIndex - 1) || LastStageIsAchieved) return;

            CurrentStageIndex = stageIndex - 1;
            _recordTimer.SaveCurrentTime();

            if (IsLastStage(stageIndex))
            {
                LastStageIsAchieved = true;
                OnAchievedFinalStage.Invoke();
                CurrentStageIndex = 0;
                _recordTimer.Stop(true, true);
            }
        }

        private void OnYandexInitialized()
        {
            CurrentStageIndex = YandexGame.savesData.CurrentStage;
            
            if(IsLastStage(CurrentStageIndex))
                RestartFromFirstStage();
            else
                RestartStage();
        }
        
        public void Dispose()
        {
            YandexGame.GetDataEvent -= OnYandexInitialized;
        }
    }
}