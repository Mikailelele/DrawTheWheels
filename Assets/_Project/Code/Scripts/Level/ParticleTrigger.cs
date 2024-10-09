using DrawCar.Utils;
using UnityEngine;

namespace DrawCar.Level
{
    public sealed class ParticleTrigger : MonoBehaviour
    {
        [SerializeField] private Transform _particlesTransform;
        [SerializeField] private GameObject _particlesPrefab;
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(Constants.Tags.Car)) return;
            
            var particles = Instantiate(_particlesPrefab, _particlesTransform.position, Quaternion.identity);
            Destroy(particles, 10);
        }
    }
}