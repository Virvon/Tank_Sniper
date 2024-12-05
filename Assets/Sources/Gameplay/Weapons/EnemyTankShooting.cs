using Assets.Sources.Infrastructure.Factories.GameplayFactory;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.Weapons
{
    public class EnemyTankShooting : MonoBehaviour
    {
        private const int AngleDelta = 2;
        private const int RayCastDistance = 300;

        [SerializeField] private WeaponType _weaponType;
        [SerializeField] private Transform _shootPoint;
        [SerializeField] private uint _rotationSpeed;

        private PlayerTank _playerTank;
        private IGameplayFactory _gameplayFactory;

        private bool _isTurnedToPlayerTank;

        private float _reloadDuration;
        private float _shootCooldown;
        private uint _bulletsCapacity;

        private uint _bulletsCount;

        [Inject]
        private void Construct(PlayerTank playerTank, IStaticDataService staticDataService, IGameplayFactory gameplayFactory)
        {
            _playerTank = playerTank;
            _gameplayFactory = gameplayFactory;

            _isTurnedToPlayerTank = false;

            WeaponConfig weaponConfig = staticDataService.GetWeapon(_weaponType);

            _reloadDuration = weaponConfig.ReloadDuration;
            _shootCooldown = weaponConfig.ShootCooldown;
            _bulletsCapacity = weaponConfig.BulletsCapacity;

            _bulletsCount = _bulletsCapacity;

            _playerTank.Attacked += OnPlayerTankAttacked;
        }

        private void OnDestroy()
        {
            _playerTank.Attacked -= OnPlayerTankAttacked;
        }

        private void OnPlayerTankAttacked()
        {
            StartCoroutine(Shooter());
            StartCoroutine(Rotater());
        }

        private bool CheckPlayerTankVisibility()
        {
            return Physics.Raycast(_shootPoint.position, (_playerTank.transform.position - _shootPoint.position).normalized, out RaycastHit hitInfo, RayCastDistance)
                && hitInfo.transform.TryGetComponent(out PlayerTank _);
        }

        private void Shoot() =>
            _gameplayFactory.CreateBullet(_weaponType, _shootPoint.position, Quaternion.LookRotation((_playerTank.transform.position - _shootPoint.position).normalized));

        private IEnumerator Shooter()
        {
            bool isShooted = true;
            WaitForSeconds reloadDuration = new WaitForSeconds(_reloadDuration);
            WaitForSeconds shootCooldown = new WaitForSeconds(_shootCooldown);

            while (isShooted)
            {
                yield return new WaitWhile(() => _isTurnedToPlayerTank == false || CheckPlayerTankVisibility() == false);

                _bulletsCount--;
                Shoot();

                if (_bulletsCount > 0 == false)
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
            bool isRotated = true;

            while (isRotated)
            {
                Vector3 shootPointForward = _shootPoint.forward;
                Vector3 targetDirection = (_playerTank.transform.position - _shootPoint.position).normalized;

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
