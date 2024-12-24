using Assets.Sources.Gameplay.Enemies;
using Assets.Sources.Gameplay.Enemies.Animation;
using Assets.Sources.Gameplay.Player;
using Assets.Sources.Infrastructure.Factories.BulletFactory;
using Assets.Sources.Types;
using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.Weapons
{
    public class EnemyCharacterShooting : MonoBehaviour
    {
        private const int AngleDelta = 3;
        private const int RayCastDistance = 300;

        private readonly Vector3 TargetOffset = new Vector3(0, 2, 0);

        [SerializeField] private Animator _animator;
        [SerializeField] private EnemyAnimation _enemyAnimation;
        [SerializeField] private Transform _shootPoint;
        [SerializeField] private uint _rotationSpeed;
        [SerializeField] private float _reloadDuration;
        [SerializeField] private float _shootCooldown;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private ForwardFlyingBulletType _bulletType;
        [SerializeField] private uint _bulletsCapacity;
        [SerializeField] private MuzzleType _muzzleType;
        [SerializeField] private Enemy _enemy;

        private PlayerTankWrapper _playerTankWrapper;
        private Aiming _aiming;
        private IBulletFactory _bulletFactory;

        private bool _isTurnedToPlayerTank;
        private bool _isStartedShoot;
        private uint _bulletsCount;
        private bool _isShooted;
        private bool _isRotated;

        private Quaternion TargetRotation => Quaternion.LookRotation(((_playerTankWrapper.transform.position + TargetOffset) - _shootPoint.position).normalized);

        [Inject]
        private void Construct(PlayerTankWrapper playerTankWrapper, Aiming aiming, IBulletFactory bulletFactory)
        {
            _playerTankWrapper = playerTankWrapper;
            _aiming = aiming;
            _bulletFactory = bulletFactory;

            _isTurnedToPlayerTank = false;
            _isStartedShoot = false;
            _bulletsCount = _bulletsCapacity;
            _isShooted = false;
            _isRotated = false;

            _aiming.Shooted += OnPlayerTankAttacked;
            _enemyAnimation.BulletNeedetToCreate += OnBulletNeedetToCreate;
            _enemy.Destructed += OnEnemyDestructed;
        }

        private void OnDestroy()
        {
            _aiming.Shooted -= OnPlayerTankAttacked;
            _enemyAnimation.BulletNeedetToCreate -= OnBulletNeedetToCreate;
            _enemy.Destructed -= OnEnemyDestructed;
        }

        protected virtual void StartShooting()
        {
            _animator.SetTrigger(AnimationPath.IsShooted);

            StartCoroutine(Shooter());
            StartCoroutine(Rotater());
        }

        private void OnEnemyDestructed()
        {
            _isShooted = false;
            _isRotated = false;
        }

        private void OnBulletNeedetToCreate() =>
            _bulletFactory.CreateForwardFlyingBullet(_bulletType, _shootPoint.position, TargetRotation);

        private void OnPlayerTankAttacked()
        {
            if (_isStartedShoot)
                return;

            StartShooting();
        }

        private bool CheckPlayerTankVisibility()
        {
            return Physics.Raycast(_shootPoint.position, (_playerTankWrapper.transform.position - _shootPoint.position).normalized, out RaycastHit hitInfo, RayCastDistance, _layerMask)
                && hitInfo.transform.TryGetComponent(out PlayerTankWrapper _);
        }

        private IEnumerator Shooter()
        {
            _isShooted = true;
            WaitForSeconds reloadDuration = new WaitForSeconds(_reloadDuration);
            WaitForSeconds shootCooldown = new WaitForSeconds(_shootCooldown);

            while (_isShooted)
            {
                yield return new WaitWhile(() => _isTurnedToPlayerTank == false || CheckPlayerTankVisibility() == false);

                _animator.SetTrigger(AnimationPath.Shoot);
                _bulletFactory.CreateMuzzle(_muzzleType, _shootPoint.position, TargetRotation);
                _bulletsCount--;

                if (_bulletsCount == 0)
                {
                    _bulletsCount = _bulletsCapacity;
                    yield return reloadDuration;
                }
                else
                {
                    yield return shootCooldown;
                }
            }
        }

        private IEnumerator Rotater()
        {
            _isRotated = true;

            while (_isRotated)
            {
                Vector3 shootPointForward = _shootPoint.forward;
                Vector3 targetDirection = (_playerTankWrapper.transform.position - _shootPoint.position).normalized;

                Quaternion targetRotation = Quaternion.Euler(
                0,
                transform.rotation.eulerAngles.y + Quaternion.FromToRotation(shootPointForward, targetDirection).eulerAngles.y,
                0);

                shootPointForward.y = 0;
                targetDirection.y = 0;

                if (Vector3.Angle(shootPointForward, targetDirection) > AngleDelta)
                {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
                    _isTurnedToPlayerTank = false;
                }
                else
                {
                    _isTurnedToPlayerTank = true;
                }

                yield return null;
            }
        }
    }
}
