using Assets.Sources.Gameplay.Weapons;
using UnityEngine;

namespace Assets.Sources.Gameplay.Audio
{
    public class EnemyRobotSound : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private EnemyRobotShooting _enemyRobotShooting;

        private void OnEnable()
        {
            _enemyRobotShooting.ShootingStarted += OnShootingStarted;
            _enemyRobotShooting.ShootingFinished += OnShootingFinished;
        }

        private void OnDisable()
        {
            _enemyRobotShooting.ShootingStarted -= OnShootingStarted;
            _enemyRobotShooting.ShootingFinished -= OnShootingStarted;
        }

        private void OnShootingFinished() =>
            _audioSource.Stop();

        private void OnShootingStarted() =>
            _audioSource.Play();
    }
}
