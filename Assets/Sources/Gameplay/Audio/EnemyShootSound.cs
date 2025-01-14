using Assets.Sources.Gameplay.Weapons;
using System;
using UnityEngine;

namespace Assets.Sources.Gameplay.Audio
{
    public class EnemyShootSound : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private EnemyShooting _enemyShooting;

        private void OnEnable() =>
            _enemyShooting.Shooted += OnShooted;

        private void OnDisable() =>
            _enemyShooting.Shooted -= OnShooted;

        private void OnShooted() =>
            _audioSource.Play();
    }
}
