using Assets.Sources.Gameplay.Enemies.Helicopter;
using System;
using UnityEngine;

namespace Assets.Sources.Gameplay.Audio
{
    public class HelicopterRotorSound : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private Helicopter _helicopter;

        private void OnEnable() =>
            _helicopter.Destructed += OnDestructed;

        private void OnDisable() =>
            _helicopter.Destructed -= OnDestructed;

        private void OnDestructed() =>
            _audioSource.Stop();
    }
}
