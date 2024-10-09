using Code.Scripts.TimeManagement;
using DG.Tweening;
using DrawCar.Audio;
using DrawCar.Car.Input;
using DrawCar.Drawing;
using DrawCar.Level;
using DrawCar.Ui.Messages;
using MessagePipe;
using UnityEngine;
using VContainer;

namespace DrawCar.Ui.Tutorials
{
    [DisallowMultipleComponent]
    public sealed class DrawingTutorial : MonoBehaviour
    {
        [SerializeField, Range(0, 360)] private float _endValueDegrees = 360f;
        [SerializeField] private float _duration = 2f;
        
        [SerializeField] private RectTransform _drawCanvas;
        [SerializeField] private RectTransform _tutorialImage;

        [SerializeField] private GameObject _tutorialUi;
        [SerializeField] private CanvasGroup _cancelDrawingButton;

        [SerializeField] private WheelGenerator _wheelGenerator;

        private IPauseController _pauseController;
        
        private Vector3 _cachedImageRotation;

        [Inject]
        private void Construct(IPauseController pauseController)
        {
            _pauseController = pauseController;

        }

        private void Awake()
        {
            _cachedImageRotation = new Vector3(0, 0, -_endValueDegrees);
            SubscribeEvents();
        }

        private void Start()
        {
            SetActiveCancelDrawingButton(false);
            StartTutorial();
        }
        
        private void OnDestroy()
        {
            UnsubscribeEvents();
        }
        
        private void StartTutorial()
        {
            _pauseController.SetPause(true, false, false, true, false);
            
            _tutorialUi.SetActive(true);
            
            _tutorialImage.DOShapeCircle(_drawCanvas.localPosition, _endValueDegrees, _duration)
                .SetLoops(-1, LoopType.Restart);
            
            _tutorialImage.DORotate(_cachedImageRotation, _duration, RotateMode.FastBeyond360)
                .SetLoops(-1, LoopType.Restart);
        }

        private void OnStartedDrawing(int value)
        {
            FinishTutorial();
            
            _wheelGenerator.OnPointAdded -= OnStartedDrawing;
        }
        
        private void FinishTutorial()
        {
            _pauseController.SetPause(false, false, false, true, false);
            
            _tutorialImage.DOKill();

            Destroy(_tutorialUi);
            _tutorialUi = null;
            _tutorialImage = null;
            
            SetActiveCancelDrawingButton(true);
        }
        
        private void SetActiveCancelDrawingButton(bool value)
        {
            _cancelDrawingButton.interactable = value;
            _cancelDrawingButton.blocksRaycasts = value;
        }
        
        private void SubscribeEvents()
        {
            _wheelGenerator.OnPointAdded += OnStartedDrawing;
        }
        
        private void UnsubscribeEvents()
        {
            _wheelGenerator.OnPointAdded -= OnStartedDrawing;
        }
    }
}