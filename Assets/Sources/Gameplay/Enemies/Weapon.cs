using Assets.Sources.Gameplay.Enemies.Animation;
using Assets.Sources.Infrastructure.Factories.GameplayFactory;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.Enemies
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private uint _bulletsCapacity;
        [SerializeField] private float _shootCooldown;
        [SerializeField] private float _reloadDuration;
        [SerializeField] private Animator _animator;
        [SerializeField] private EnemyAnimation _enemyAnimation;
        [SerializeField] private Transform _shootPoint;

        private IGameplayFactory _gameplayFactory;
        private PlayerTank _playerTank;

        private uint _bulletsCount;
        private bool _isShooted;

        Vector3 test;

        [Inject]
        private void Construct(IGameplayFactory gameplayFactory, PlayerTank playerTank)
        {
            _gameplayFactory = gameplayFactory;
            _playerTank = playerTank;

            _bulletsCount = _bulletsCapacity;
            _isShooted = false;

            _enemyAnimation.BulletCreated += CreateBullet;
        }           

        private void OnDestroy() =>
            _enemyAnimation.BulletCreated -= CreateBullet;


        public void StartShooting() =>
            StartCoroutine(Shooter());

        private void CreateBullet()
        {
            test = (_playerTank.transform.position - _shootPoint.position).normalized;
            _gameplayFactory.CreateBullet(BulletType.MachineGunBullet, _shootPoint.position, Quaternion.LookRotation((_playerTank.transform.position - _shootPoint.position).normalized));
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(_shootPoint.transform.position, _shootPoint.transform.position + (test * 3));
        }

        private IEnumerator Shooter()
        {
            WaitForSeconds reloadDuration = new WaitForSeconds(_reloadDuration);
            WaitForSeconds shootCooldown = new WaitForSeconds(_shootCooldown);

            _isShooted = true;

            while (_isShooted)
            {
                if(_bulletsCount == 0)
                {
                    _bulletsCount = _bulletsCapacity;

                    yield return reloadDuration;
                }

                _bulletsCount--;

                _animator.SetTrigger(AnimationPath.Shoot);

                yield return shootCooldown;
            }
        }
    }
}
