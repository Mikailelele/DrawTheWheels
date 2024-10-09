using System.Runtime.CompilerServices;
using DrawCar.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace DrawCar.Drawing.Ui
{
    public sealed class TotalPointsSlider : MonoBehaviour
    {
        [SerializeField] private WheelGenerator _pointsDrawer;

        [SerializeField] private Slider _slider;
        [SerializeField] private TMP_Text _sliderText;
        
        private int _maxPointsAmount;
        
        [Inject]
        private void Construct(IDrawSettings drawSettings)
        {
            _slider.maxValue = drawSettings.MaxPointsAmount;
            _maxPointsAmount = drawSettings.MaxPointsAmount;
        }

        private void OnEnable()
        {
            _pointsDrawer.OnPointAdded += UpdateSliderValue;
            
            Reset();
        }
        
        private void OnDisable()
        {
            _pointsDrawer.OnPointAdded -= UpdateSliderValue;
        }

        public void Reset()
        {
            _slider.value = 0;
            
            SetSliderText(0);
        }

        private void UpdateSliderValue(int totalPoints)
        {
            _slider.value = totalPoints;
            SetSliderText(totalPoints);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetSliderText(in int totalPoints)
        {
            _sliderText.SetText($"{_maxPointsAmount} / {totalPoints}");
        }
    }
}