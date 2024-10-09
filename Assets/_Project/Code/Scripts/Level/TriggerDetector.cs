using System;
using DrawCar.Utils;
using UnityEngine;
using VContainer;

namespace DrawCar.Level
{
    public sealed class TriggerDetector : MonoBehaviour
    {
        private IStageService _stageService;
        private TriggerZone _triggerZone;
        
        [Inject]
        private void Construct(IStageService stageService)
        {
            _stageService = stageService;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out _triggerZone))
            {
                _stageService.StartNextStage(_triggerZone.StageIndex);
            }
            else if (other.CompareTag(Constants.Tags.DeadZoneTrigger))
            {
                _stageService.RestartStage();
            }
        }
    }
}