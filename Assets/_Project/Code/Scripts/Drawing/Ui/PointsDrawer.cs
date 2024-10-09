using System;
using System.Collections.Generic;
using DrawCar.ScriptableObjects;
using DrawCar.Ui.Messages;
using MessagePipe;
using NTC.Pool;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace DrawCar.Drawing.Ui
{
    public sealed class PointsDrawer : MonoBehaviour, IPointerMoveHandler
    {
        [SerializeField] private GameObject pointPrefab;
        
        private readonly List<GameObject> _points = new();
        private RectTransform _rectTransform;
        private PointerEventData _pointerEventData;
        
        private IDrawSettings _drawSettings;
        
        private Vector3 _cachedScreenToWorldPointPosition;
        private GameObject _cachedPoint;
        private Transform _transform;

        private IPublisher<OnUiActionMessage> _uiActionPublisher;
        private OnUiActionMessage _drawActionMessage;
        
        [Inject]
        private void Construct(
            IPublisher<OnUiActionMessage> uiActionPublisher, 
            IDrawSettings drawSettings)
        {
            _uiActionPublisher = uiActionPublisher;
            _drawSettings = drawSettings;
        }

        private void Awake()
        {
            _transform = transform;
            _rectTransform = GetComponent<RectTransform>();
            _drawActionMessage = new OnUiActionMessage(EUiActionType.Draw);
        }

        public void OnPointerMove(PointerEventData eventData)
        { 
            _pointerEventData = eventData;
        }

        public void SpawnDrawingPoint()
        {
            if(_points.Count >= _drawSettings.MaxPointsAmount) return;
            
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                _rectTransform,
                _pointerEventData.position,
                _pointerEventData.pressEventCamera,
                out _cachedScreenToWorldPointPosition
            );
            
            _uiActionPublisher.Publish(_drawActionMessage);
            _cachedPoint = NightPool.Spawn(pointPrefab, _cachedScreenToWorldPointPosition, Quaternion.identity, _transform);
            _points.Add(_cachedPoint);
        }
        

        public void ClearAllPoints()
        {
            foreach (var point in _points)
            {
                NightPool.Despawn(point);
            }
            _points.Clear();
        }
    }
}