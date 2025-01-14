using Assets.Sources.MainMenu.Weapons;
using UnityEngine;

namespace Assets.Sources.Gameplay.Audio
{
    public class MainMenuShootSound : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private Weapon _weapon;

        private void OnEnable() =>
            _weapon.BulletCreated += OnBulletCreated;

        private void OnDisable() =>
            _weapon.BulletCreated -= OnBulletCreated;

        private void OnBulletCreated() =>
            _audioSource.Play();
    }
}
