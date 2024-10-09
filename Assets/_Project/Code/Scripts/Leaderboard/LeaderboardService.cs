using System;
using DrawCar.Level;
using DrawCar.Utils;
using DrawCar.YandexIntegration;
using YG;

namespace Code.Scripts.Leaderboard
{
    public sealed class LeaderboardService : IDisposable
    {
        private readonly IStageService _stageService;
        
        public LeaderboardService(IStageService stageService)
        {
            _stageService = stageService;
            
            SubscribeEvents();
            
            this.LogInjectSuccess();
        }
        
        private void CheckForNewTimeRecord()
        {
            var savesData = YandexGame.savesData;
            if (savesData.LastStageTime < savesData.BestStageTime || savesData.BestStageTime == -1)
            {
                SavingService.SetInt(ESaveType.BestStageTime, savesData.LastStageTime);
                YandexGame.NewLBScoreTimeConvert(Constants.Leaderboard.LeaderboardName, YandexGame.savesData.LastStageTime * 1000);
                this.Log($"New record: {YandexGame.savesData.LastStageTime}");
            }
        }
        
        private void SubscribeEvents()
        {
            _stageService.OnAchievedFinalStage += CheckForNewTimeRecord;
        }
        
        private void UnsubscribeEvents()
        {
            _stageService.OnAchievedFinalStage -= CheckForNewTimeRecord;
        }

        public void Dispose()
        {
            UnsubscribeEvents();
        }
    }
}