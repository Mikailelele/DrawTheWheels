using System;
using DrawCar.ScriptableObjects;
using DrawCar.Ui;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using VContainer;

namespace DrawCar.Drawing.Ui
{
    [RequireComponent(typeof(Button))]
    public sealed class DrawingBoardCanvas : BaseButtonController, IDragHandler, IPointerEnterHandler
    {
        private Vector2 _previousPointerPosition = Vector2.zero;
        private float _distanceMoved;
        private bool _canDrag;
        
        private IDrawSettings _drawSettings;
        
        public event Action OnMove = () => { };
        
        [Inject]
        private void Construct(IDrawSettings drawSettings)
        {
            _drawSettings = drawSettings;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_canDrag || !IsLeftButton(eventData)) return;
            
            _distanceMoved = Vector2.Distance(_previousPointerPosition, eventData.position);
            
            if (_distanceMoved > _drawSettings.MinDrawDistancePerMove)
            {
                OnMove.Invoke();
                _previousPointerPosition = eventData.position;
            }
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            if(IsLeftButton(eventData))
                _canDrag = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _canDrag = true;
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            _canDrag = false;
        }
    }
}
