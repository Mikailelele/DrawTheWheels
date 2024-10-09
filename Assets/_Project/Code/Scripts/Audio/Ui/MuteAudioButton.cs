using DrawCar.Ui;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using YG;

namespace DrawCar.Audio.Ui
{
    public sealed class MuteAudioButton : UiButton
    {
        [SerializeField, Required] private Sprite _mutedSprite;
        [SerializeField, Required] private Sprite _unmutedSprite;

        [SerializeField, Required] private Image _image;
        
        private void OnEnable() => YandexGame.GetDataEvent += SetImage;
        private void OnDisable() => YandexGame.GetDataEvent -= SetImage;
        
        internal override void Awake()
        {
            base.Awake();

            if (YandexGame.SDKEnabled)
            {
                SetImage();
            }
        }
        
        private void SetImage()
        {
            SetMuteState(YandexGame.savesData.IsAudioMuted);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            SetMuteState(YandexGame.savesData.IsAudioMuted);
        }
        
        private void SetMuteState(in bool value)
        {
            _image.sprite = value ? _mutedSprite : _unmutedSprite;
        }
    }
}