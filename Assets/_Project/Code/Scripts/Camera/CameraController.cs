using System.Runtime.CompilerServices;
using Cinemachine;
using Cysharp.Threading.Tasks;
using DrawCar.ScriptableObjects;
using UnityEngine;
using VContainer;

namespace DrawCar.Camera
{
    public sealed class CameraController : MonoBehaviour
    {
        [SerializeField] CinemachineFreeLook _camera;
        
        private IInputSettings _inputSettings;
        
        [Inject]
        private void Construct(IInputSettings inputSettings)
        {
            _inputSettings = inputSettings;
        }

        public void SetMobileSensitivity()
        {
            SetSensitivity(
                _inputSettings.YMobileLookSensitivity,
                _inputSettings.XMobileLookSensitivity);
        }

        public void LockCamera(bool value)
        {
            SetSensitivity(
                value ? 0 : _inputSettings.YMouseLookSensitivity,
                value ? 0 : _inputSettings.XMouseLookSensitivity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetSensitivity(in float y, in float x)
        {
            _camera.m_YAxis.m_MaxSpeed = y;
            _camera.m_XAxis.m_MaxSpeed = x;
        }
        
        public async UniTaskVoid ResetRotation()
        {
            _camera.m_YAxisRecentering.m_enabled = true;
            _camera.m_RecenterToTargetHeading.m_enabled = true;
            _camera.m_YAxisRecentering.RecenterNow();
            _camera.m_RecenterToTargetHeading.RecenterNow();

            await UniTask.Delay(1000);
            _camera.m_RecenterToTargetHeading.m_enabled = false;
            _camera.m_YAxisRecentering.m_enabled = false;
        }
    }
}