using DrawCar.Level;
using UnityEngine;

namespace DrawCar.Data
{
    [System.Serializable]
    public struct StagesData
    {
        [field: SerializeField]
        public TriggerZone[] TriggerZones { get; set; } 
    }
}