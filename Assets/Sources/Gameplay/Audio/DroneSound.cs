using Assets.Sources.Gameplay.Player;
using UnityEngine;

namespace Assets.Sources.Gameplay.Audio
{
    public class DroneSound : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private Drone _drone;

        private void OnEnable() =>
            _drone.Exploded += OnExploded;

        private void OnDisable() =>
            _drone.Exploded -= OnExploded;

        private void OnExploded() =>
            _audioSource.Stop();
    }
}
