using Assets.Sources.Gameplay.Player.Weapons;
using System;
using UnityEngine;

namespace Assets.Sources.Gameplay.Audio
{
    public class PlayerShootSound : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private PlayerTankWeapon _playerTankWeapon;

        private void OnEnable() =>
            _playerTankWeapon.BulletCreated += OnBulletCreated;

        private void OnDisable() =>
            _playerTankWeapon.BulletCreated -= OnBulletCreated;

        private void OnBulletCreated() =>
            _audioSource.Play();
    }
}
