using UnityEngine;

namespace DrawCar.Data
{
    [System.Serializable]
    public struct LevelData
    {
        [field: SerializeField] 
        public int LevelNumber { get; private set; }
        
        [field: SerializeField]
        public int TotalStages { get; private set; }
    }
}