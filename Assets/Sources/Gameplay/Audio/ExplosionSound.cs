using UnityEngine;

namespace Assets.Sources.Gameplay.Audio
{
    public class ExplosionSound : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private Explosion _explosion;

        private void OnEnable() =>
            _explosion.Exploded += OnExploded;

        private void OnDisable() =>
            _explosion.Exploded -= OnExploded;

        private void OnExploded() =>
            _audioSource.Play();
    }
}
