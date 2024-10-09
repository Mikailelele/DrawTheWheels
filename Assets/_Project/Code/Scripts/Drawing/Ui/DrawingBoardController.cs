using DrawCar.ScriptableObjects;
using DrawCar.Ui.Messages;
using MessagePipe;
using UnityEngine;
using VContainer;

namespace DrawCar.Drawing.Ui
{
    public sealed class DrawingBoardController : MonoBehaviour
    {
        [SerializeField] private WheelGenerator wheelsGenerator;
        [SerializeField] private PointsDrawer _pointsDrawer;
        [SerializeField] private DrawingBoardCanvas _drawingBoard;
        
        private IPublisher<OnUiActionMessage> _uiActionPublisher;

        [Inject]
        private void Construct(IPublisher<OnUiActionMessage> uiActionPublisher)
        {
            _uiActionPublisher = uiActionPublisher;
        }

        private void OnEnable()
        { 
            _drawingBoard.OnPointerUpAction += OnGenerateWheels;

            _drawingBoard.OnMove += DrawOnCanvas;
            _drawingBoard.OnPointerDownAction += DrawOnCanvas;
        }
        
        private void OnDisable()
        { 
            _drawingBoard.OnPointerUpAction -= OnGenerateWheels;

            _drawingBoard.OnMove -= DrawOnCanvas;
            _drawingBoard.OnPointerDownAction -= DrawOnCanvas;
        }

        private void OnGenerateWheels()
        {
            wheelsGenerator.GenerateWheels();
            _pointsDrawer.ClearAllPoints();
            
            _uiActionPublisher.Publish(new OnUiActionMessage(EUiActionType.CloseDrawCanvas));
        }
        
        private void DrawOnCanvas()
        {
            wheelsGenerator.DrawOnCanvas();
            _pointsDrawer.SpawnDrawingPoint();
        }
    }
}