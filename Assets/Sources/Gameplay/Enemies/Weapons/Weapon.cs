using Assets.Sources.Gameplay.Enemies.Animation;
using Assets.Sources.Infrastructure.Factories.GameplayFactory;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.Enemies
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private BulletType _bulletType;
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

        private void CreateBullet() =>
            _gameplayFactory.CreateBullet(_bulletType, _shootPoint.position, Quaternion.LookRotation((_playerTank.transform.position - _shootPoint.position).normalized));

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
