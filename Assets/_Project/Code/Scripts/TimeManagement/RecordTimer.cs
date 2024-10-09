using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using DrawCar.YandexIntegration;
using VContainer.Unity;
using YG;
using IInitializable = VContainer.Unity.IInitializable;

namespace Code.Scripts.TimeManagement
{
    public interface IRecordTimer
    {
        void Stop(bool isStopped, bool setLock = false);
        void RestartToLastSavedTime();
        void Reset();
        void SaveCurrentTime();
        
        int Minutes { get; }
        int Seconds { get; }
        
        event Action OnTimeChanged;
    }
    
    public sealed class RecordTimer : IRecordTimer, IInitializable, IStartable
    {
        private int _time;
        private bool _isLocked;
        
        public int Minutes => _time / 60;
        public int Seconds => _time % 60;
        
        public bool IsRunning { get; private set; } = false;

        public event Action OnTimeChanged = () => { };

        public void Initialize()
        {
            RestartToLastSavedTime();
        }
        
        public void Start()
        {
            StartTimer().Forget();
        }
        
        private async UniTaskVoid StartTimer()
        {
            OnTimeChanged.Invoke();

            while (true)
            {
                await UniTask.WaitForSeconds(1);
                if (IsRunning)
                {
                    SetTime(++_time);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetTime(in int value)
        {
            _time = value;
            OnTimeChanged.Invoke();
        }
        
        public void RestartToLastSavedTime()
        {
            SetTime(YandexGame.savesData.LastStageTime);
        }
        
        public void Reset()
        {
            SetTime(0);
            SaveCurrentTime();
        }
        
        public void SaveCurrentTime()
        {
            SavingService.SetInt(ESaveType.LastStageTime, _time);
        }
        
        public void Stop(bool isStopped, bool setLock = false)
        {
            if(setLock)
                _isLocked = isStopped;
            
            if(_isLocked) return;
            IsRunning = !isStopped;
        }
    }
}