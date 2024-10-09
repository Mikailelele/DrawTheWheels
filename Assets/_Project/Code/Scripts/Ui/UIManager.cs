using System;
using Code.Scripts.TimeManagement;
using DrawCar.Audio;
using DrawCar.Drawing.Ui;
using DrawCar.Level;
using DrawCar.Ui.Messages;
using MessagePipe;
using NaughtyAttributes;
using UnityEngine;
using VContainer;
using YG;

namespace DrawCar.Ui
{
    public sealed class UIManager : MonoBehaviour
    {
        [SerializeField, Required] private Canvas _drawCanvas;
        [SerializeField, Required] private Canvas _gameplayCanvas;
        [SerializeField, Required] private GameObject _leaderboardCanvas;
        [SerializeField, Required] private Canvas _joystickCanvas;
        [SerializeField, Required] private TotalPointsSlider _totalPointsSlider;
        [SerializeField, Required] private GameObject _restartButton;
        [SerializeField, Required] private GameObject _drawButton;
        [SerializeField, Required] private GameObject _replayGameButton;

        private IStageService _stageService;
        private IAudioService _audioService;
        
        private ISubscriber<OnUiActionMessage> _buttonsActionSubscriber;
        private IPauseController _pauseController;
        private IDisposable _disposable;

        [Inject]
        private void Construct(
            ISubscriber<OnUiActionMessage> buttonsActionSubscriber, 
            IPauseController pauseController,
            IStageService stageService,
            IAudioService audioService)
        {
            _buttonsActionSubscriber = buttonsActionSubscriber;
            _pauseController = pauseController;
            _stageService = stageService;
            _audioService = audioService;
            
            var bag = DisposableBag.CreateBuilder();
            _buttonsActionSubscriber.Subscribe(HandleButtonMessage).AddTo(bag);
            _disposable = bag.Build();
        }

        private void Awake()
        {
            CheckForJoystick();
        }

        private void OnDestroy()
        {
            _disposable.Dispose();
        }

        private void HandleButtonMessage(OnUiActionMessage message)
        {
            switch (message.Type)
            {
                case EUiActionType.RestartStage:
                    _stageService.RestartStage();
                    break;
                case EUiActionType.RestartGame:
                    _stageService.RestartFromFirstStage();
                    SwitchRestartToReplayButtons(false);
                    break;
                case EUiActionType.OpenDrawCanvas:
                    _totalPointsSlider.Reset();
                    ShowDrawCanvas(true);
                    break;
                case EUiActionType.CloseDrawCanvas:
                    ShowDrawCanvas(false);
                    break;
                case EUiActionType.OpenLeaderboard:
                    ShowLeaderboardCanvas(true);
                    break;
                case EUiActionType.CloseLeaderboard:
                    ShowLeaderboardCanvas(false);
                    break;
                case EUiActionType.MuteAudio:
                    _audioService.PauseAll(!YandexGame.savesData.IsAudioMuted);
                    break;
                case EUiActionType.Draw:
                    PlayAudio(EAudioType.Draw);
                    break;
                case EUiActionType.SwitchRestartToReplay:
                    SwitchRestartToReplayButtons(true);
                    break;
            }
            if(message.ByButton)
                PlayAudio(EAudioType.Button);
        }
        
        private void PlayAudio(EAudioType audioType)
        {
            _audioService.Play(audioType);
        }

        private void ShowDrawCanvas(bool value)
        {
            _drawCanvas.enabled = value;
            _gameplayCanvas.enabled = !value;
        }
        
        private void ShowLeaderboardCanvas(bool value)
        {
            _leaderboardCanvas.SetActive(value);
            _gameplayCanvas.enabled = !value;
            
            if(_stageService.LastStageIsAchieved && !value) return;
            _pauseController.SetPause(value, true, true, true);
        }
        
        private void SwitchRestartToReplayButtons(bool toReplay)
        {
            _restartButton.SetActive(!toReplay);
            _drawButton.SetActive(!toReplay);
            _replayGameButton.SetActive(toReplay);
        }
        
        private void CheckForJoystick()
        { 
            if (YandexGame.EnvironmentData.isMobile && _joystickCanvas != null) 
                _joystickCanvas.enabled = true;
        }
    }
}
