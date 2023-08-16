using UnityEngine;

namespace MR
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ConfettiController : MonoBehaviour
    {
        ParticleSystem _particleSystem;

        ParticleSystem ParticleSystem
        {
            get
            {
                if (_particleSystem == null)
                {
                    _particleSystem = GetComponent<ParticleSystem>();
                }

                return _particleSystem;
            }
        }

        public System.Action onEnd;

        bool _isPlaying;

        public void Play()
        {
            ParticleSystem.Play();
            _isPlaying = true;
        }

        void Update()
        {
            if (_isPlaying && !ParticleSystem.isPlaying)
            {
                _isPlaying = false;
                onEnd?.Invoke();
            }
        }
    }
}
