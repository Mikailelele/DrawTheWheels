using System;
using System.Collections.Generic;
using DrawCar.Drawing.Messages;
using DrawCar.ScriptableObjects;
using DrawCar.Utils;
using MessagePipe;
using NTC.Pool;
using UnityEngine;
using VContainer;

namespace DrawCar.Drawing
{ 
    public sealed class WheelGenerator : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private GameObject _wheelPartPrefab;
        [SerializeField] private GameObject _wheelParentPrefab;
        [SerializeField] private List<Vector3> _spawnPoints;
        
        private IDrawSettings _drawSettings;
        
        private MaterialPropertyBlock _propertyBlock;
        private MeshRenderer _cachedRenderer;

        private IPublisher<OnWheelGeneratedMessage> _onMeshGeneratedMessage;
        public event Action<int> OnPointAdded;

        [Inject]
        private void Construct(IPublisher<OnWheelGeneratedMessage> onWheelGeneratedMessage, IDrawSettings drawSettings)
        {
            _onMeshGeneratedMessage = onWheelGeneratedMessage;
            _drawSettings = drawSettings;
            
            _propertyBlock = new MaterialPropertyBlock();
        }
        
        public void DrawOnCanvas()
        {
            if(_spawnPoints.Count >= _drawSettings.MaxPointsAmount) return;
            
            Vector3 position = Input.mousePosition * _drawSettings.PointsDistanceMultiplier;
            position.x /= Screen.height;
            position.y /= Screen.height;
            position.z = 0;
            Draw(position);
        }

        public void GenerateWheels()
        {
            Generate();
            ClearBoard();
        }

        private void ClearBoard()
        {
            _lineRenderer.positionCount = 0;
            _spawnPoints.Clear();
        }

        private void Generate()
        {
            GameObject[] generatedWheels = new GameObject[4];
            
            for (int i = 0; i < 4; i++)
            {
                generatedWheels[i] = Instantiate(_wheelParentPrefab);
                generatedWheels[i].transform.position = Vector3.zero;
            
                foreach (var spawnPoint in _spawnPoints)
                {
                    GenerateWheelPart(spawnPoint, generatedWheels[i].transform);
                }

                generatedWheels[i].transform.FixCenterPivot(out GameObject tempPivot);
                Destroy(tempPivot);
            }
            
            _onMeshGeneratedMessage.Publish(new OnWheelGeneratedMessage(generatedWheels));
        }
        
        private void Draw(in Vector3 position)
        {
            _spawnPoints.Add(position);
            _lineRenderer.gameObject.SetActive(true);
            
            OnPointAdded?.Invoke(_spawnPoints.Count);
        }

        private void GenerateWheelPart(in Vector3 spawnPoint, in Transform createdMesh)
        {
            var part = NightPool.Spawn(_wheelPartPrefab, spawnPoint, Quaternion.identity);
            part.transform.parent = createdMesh.transform;

            if (part.gameObject.TryGetComponent(out _cachedRenderer))
            {
                _cachedRenderer.SetPropertyBlock(_propertyBlock);
            }
        }
    }
}