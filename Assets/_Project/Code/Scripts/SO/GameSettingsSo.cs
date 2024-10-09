using System.Collections.Generic;
using DrawCar.Data;
using UnityEngine;
using VInspector;

namespace DrawCar.ScriptableObjects
{
    public interface IDrawSettings
    {
        float MinDrawDistancePerMove { get; } 
        int MaxPointsAmount { get; }
        float PointsDistanceMultiplier { get; }
        bool DrawAfterRelease { get; }
    }
    
    public interface ILevelSettings
    { 
        List<LevelData> Levels { get; }
    }

    public interface ICarSettings
    {
        int Acceleration { get; }
        
        int SteeringSpeed { get; }
        
        int SteeringAngle { get; }
        
        Vector3 CarSpawnOffset { get; }
    }

    public interface IInputSettings
    {
        int XMobileLookSensitivity { get; }
        
        float YMobileLookSensitivity { get; }
        
        int XMouseLookSensitivity { get; }
        
        float YMouseLookSensitivity { get; }
    }
    
    public interface IAudioSettings
    {
        AudioData[] AudioData { get; }
    }
    
    [CreateAssetMenu(fileName = "GameSettings", menuName = "DrawCube/SO/Game Settings")]
    public sealed class GameSettingsSo : 
        ScriptableObject, 
        IDrawSettings, 
        ILevelSettings, 
        ICarSettings, 
        IInputSettings,
        IAudioSettings
    {
        [field: Foldout("Drawing")]
        [field: SerializeField]
        public float MinDrawDistancePerMove { get; private set; }
        
        [field: SerializeField]
        public int MaxPointsAmount { get; private set; }
        
        [field: SerializeField]
        public float PointsDistanceMultiplier { get; private set; }
        
        [field: SerializeField]
        public bool DrawAfterRelease { get; private set; }
        
        [field: Foldout("Level")]
        [field: SerializeField]
        public List<LevelData> Levels { get; private set; }
        

        [field: Foldout("Car")]
        [field: SerializeField]
        public int Acceleration { get; private set; }
        
        [field: SerializeField]
        public int SteeringSpeed { get; private set; }
        
        [field: SerializeField]
        public int SteeringAngle { get; private set; }
        
        [field: SerializeField]
        public Vector3 CarSpawnOffset { get; private set; }
        
        [field: Foldout("Input")]
        [field: SerializeField]
        public int XMobileLookSensitivity { get; private set; }
        
        [field: SerializeField]
        public float YMobileLookSensitivity { get; private set; }
        
        [field: Space(10)]
        [field: SerializeField]
        public int XMouseLookSensitivity { get; private set; }
        
        [field: SerializeField]
        public float YMouseLookSensitivity { get; private set; }
        
        [field: Foldout("Audio")]
        [field: SerializeField]
        public AudioData[] AudioData { get; private set; }
    }
}