using Code.Scripts.TimeManagement;
using Cysharp.Threading.Tasks;
using DrawCar.Car;
using DrawCar.Ui.Messages;
using MessagePipe;
using UnityEngine;
using VContainer;

namespace DrawCar.Level
{
    public sealed class EndOfGameController : MonoBehaviour
    { 
        private IPauseController _pauseController;
        private IStageService _stageService;
        private ICarController _carController;
        
        private IPublisher<OnUiActionMessage> _uiActionPublisher;

        [Inject]
        private void Construct(
            IPublisher<OnUiActionMessage> uiActionPublisher,
            IPauseController pauseController,
            IStageService stageService,
            ICarController carController)
        {
            _uiActionPublisher = uiActionPublisher;
            _pauseController = pauseController;
            _stageService = stageService;
            _carController = carController;
            
            SubscribeEvents();
        }
        
        private void OnDestroy()
        {
            UnsubscribeEvents();
        }
        
        private void OnAchievedGameFinished()
        {
            _pauseController.SetPause(true, true, true, true, false);
            _carController.ResetCar();
            
            ShowLeaderboardAsync().Forget();
        }

        private async UniTaskVoid ShowLeaderboardAsync()
        {
            _uiActionPublisher.Publish(new OnUiActionMessage(EUiActionType.SwitchRestartToReplay));

            await UniTask.Delay(1000);
            _uiActionPublisher.Publish(new OnUiActionMessage(EUiActionType.OpenLeaderboard));
        }
        
        private void OnStartedNewGame()
        {
            _pauseController.SetPause(false, true, true, false, false);

            // _carController.Stop(false);
        }

        private void SubscribeEvents()
        {
            _stageService.OnAchievedFinalStage += OnAchievedGameFinished;
            _stageService.OnRestartedFromFirstStage += OnStartedNewGame;
        }
        
        private void UnsubscribeEvents()
        {
            _stageService.OnAchievedFinalStage -= OnAchievedGameFinished;
            _stageService.OnRestartedFromFirstStage -= OnStartedNewGame;
        }
    }
}