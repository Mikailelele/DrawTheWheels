using System.Runtime.CompilerServices;
using YG;

namespace DrawCar.YandexIntegration
{
    public enum ESaveType
    {
        CurrentStage,
        IsAudioPaused,
        LastStageTime,
        BestStageTime,
    }

    public static class SavingService
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetInt(ESaveType saveType, int value)
        {
            switch (saveType)
            {
                case ESaveType.CurrentStage:
                    YandexGame.savesData.CurrentStage = value;
                    break;
                case ESaveType.LastStageTime:
                    YandexGame.savesData.LastStageTime = value;
                    break;
                case ESaveType.BestStageTime:
                    YandexGame.savesData.BestStageTime = value;
                    break;
            }
            YandexGame.SaveProgress();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetBool(ESaveType saveType, bool value)
        {
            switch (saveType)
            {
                case ESaveType.IsAudioPaused:
                    YandexGame.savesData.IsAudioMuted = value;
                    break;
            }
            YandexGame.SaveProgress();
        }
        
        public static void ClearSave()
        {
            YandexGame.ResetSaveProgress();
            YandexGame.SaveProgress();
        }
    }
}