using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace DrawCar.Level
{
    public class TriggerZone : MonoBehaviour
    {
        [field: SerializeField, Min(0)] 
        public int StageIndex;
        
        [SerializeField, Required] private TextMeshPro _text;
        [SerializeField, Required] private Transform _center;
        
        public Vector3 CenterPosition => _center.position;

#if UNITY_EDITOR
        private void OnValidate()
        {
            UpdateText();
        }
        
        [Button("Print World Position", EButtonEnableMode.Playmode)]
        private void PrintWorldPosition()
        {
            Debug.Log(CenterPosition);
        }
#endif
        
        private void Start()
        {
            UpdateText();
        }

        private void UpdateText()
        {
            _text.SetText(StageIndex.ToString());
        }
    }
}