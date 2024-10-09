using NaughtyAttributes;
using TMPro;
using UnityEngine;
using VContainer;
using VInspector;

namespace Code.Scripts.TimeManagement.Ui
{
    public sealed class RecordTimerUi : MonoBehaviour
    {
        [SerializeField, Required] private TMP_Text _minutesText;
        [SerializeField, Required] private TMP_Text _secondsText;
        [SerializeField] private SerializedDictionary<int, string> _cachedMinutesStrings;
        [SerializeField] private SerializedDictionary<int, string> _cachedSecondsStrings;


        private IRecordTimer _recordTimer;

        private int _previousMinutes = -1;
        private int _cachedMinutes;

        [Inject]
        public void Inject(IRecordTimer recordTimer)
        {
            _recordTimer = recordTimer;
            
            SubscribeEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeEvents();
        }

        private void UpdateTimer()
        {
            _cachedMinutes = _recordTimer.Minutes;
            if(_previousMinutes != _cachedMinutes)
            {
                _minutesText.SetText(_cachedMinutesStrings[_cachedMinutes]);
                _previousMinutes = _cachedMinutes;
            }
            
            _secondsText.SetText(_cachedSecondsStrings[_recordTimer.Seconds]);
        }
        
        private void SubscribeEvents()
        {
            _recordTimer.OnTimeChanged += UpdateTimer;
        }
        
        private void UnsubscribeEvents()
        {
            _recordTimer.OnTimeChanged -= UpdateTimer;
        }
        
#if UNITY_EDITOR
        [NaughtyAttributes.Button("Set Cached Time", EButtonEnableMode.Editor)]
        private void SetCachedTime()
        {
            _cachedMinutesStrings.Clear();
            _cachedSecondsStrings.Clear();

            for (int i = 0; i < 150; i++)
            {
                _cachedMinutesStrings.Add(i, i.ToString());
            }
            
            for (int i = 0; i < 60; i++)
            {
                if(i < 10)
                    _cachedSecondsStrings.Add(i, "0" + i);
                else
                    _cachedSecondsStrings.Add(i, i.ToString());
            }
        }
#endif
    }
}