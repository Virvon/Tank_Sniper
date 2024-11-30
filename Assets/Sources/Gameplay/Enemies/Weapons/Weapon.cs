using Assets.Sources.Gameplay.Enemies.Animation;
using Assets.Sources.Infrastructure.Factories.GameplayFactory;
using Assets.Sources.Services.StaticDataService.Configs;
using System;
using UnityEngine;

namespace Assets.Sources.Gameplay.Enemies
{
    public class Weapon : IDisposable
    {
        private readonly WeaponType _type;
        private readonly uint _bulletsCapacity;
        private readonly float _shootCooldown;
        private readonly float _reloadDuration;
        private readonly Animator _animator;
        private readonly EnemyAnimation _enemyAnimation;
        private readonly Transform _shootPoint;
        private readonly IGameplayFactory _gameplayFactory;
        private readonly PlayerTank _playerTank;

        private uint _bulletsCount;

        public Weapon(
            WeaponConfig weaponConfig,
            Animator animator,
            EnemyAnimation enemyAnimation,
            Transform shootPoint,
            IGameplayFactory gameplayFactory,
            PlayerTank playerTank)
        {
            _type = weaponConfig.Type;
            _bulletsCapacity = weaponConfig.BulletsCapacity;
            _shootCooldown = weaponConfig.ShootCooldown;
            _reloadDuration = weaponConfig.ReloadDuration;
            _animator = animator;
            _enemyAnimation = enemyAnimation;
            _shootPoint = shootPoint;
            _gameplayFactory = gameplayFactory;

            _enemyAnimation.BulletCreated += CreateBullet;
            _playerTank = playerTank;
        }

        public void Dispose() =>
            _enemyAnimation.BulletCreated -= CreateBullet;

        public void Shoot()
        {
            if (_bulletsCount == 0)
                return;

            _bulletsCount--;
            _animator.SetTrigger(AnimationPath.Shoot);
        }

        public void Reload() =>
            _bulletsCount = _bulletsCapacity;

        private void CreateBullet() =>
            _gameplayFactory.CreateBullet(_type, _shootPoint.position, Quaternion.LookRotation((_playerTank.transform.position - _shootPoint.position).normalized));
    }
}
