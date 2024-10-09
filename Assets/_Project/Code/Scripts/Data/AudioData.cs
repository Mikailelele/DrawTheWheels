using DrawCar.Audio;
using NaughtyAttributes;
using UnityEngine;

namespace DrawCar.Data
{
    [System.Serializable]
    public struct AudioData
    {
        [field: SerializeField, Required]
        public AudioClip[] AudioClip { get; private set; }
        
        [field: SerializeField]
        public EAudioType Type { get; private set; }
        
        [field: Range(0, 2f)]
        [field: SerializeField]
        public float DefaultVolume { get; private set; }
        
        [field: Range(-1, 2f)]
        [field: SerializeField]
        public float DefaultPitch { get; private set; }
        
        [field: SerializeField]
        public bool Loop { get; private set; }
        
        [field: SerializeField]
        public bool PlayOnAwake { get; private set; }
    }
}