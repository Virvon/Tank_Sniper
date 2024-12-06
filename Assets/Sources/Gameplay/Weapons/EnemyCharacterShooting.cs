using Assets.Sources.Gameplay.Enemies.Animation;
using Assets.Sources.Infrastructure.Factories.GameplayFactory;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Types;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.Weapons
{
    public class EnemyCharacterShooting : MonoBehaviour
    {
        private const int AngleDelta = 2;
        private const int RayCastDistance = 300;

        [SerializeField] private Animator _animator;
        [SerializeField] private EnemyAnimation _enemyAnimation;
        [SerializeField] private Transform _shootPoint;
        [SerializeField] private uint _rotationSpeed;

        private PlayerTank _playerTank;

        private Weapon _weapon;
        private bool _isTurnedToPlayerTank;

        [Inject]
        private void Construct(PlayerTank playerTank, IStaticDataService staticDataService, IGameplayFactory gameplayFactory)
        {
            _playerTank = playerTank;

            //_weapon = new(staticDataService.GetWeapon(_weaponType), _animator, _enemyAnimation, _shootPoint, gameplayFactory, playerTank);
            _isTurnedToPlayerTank = false;

            _playerTank.Attacked += OnPlayerTankAttacked;
        }

        private void OnDestroy()
        {
            _weapon.Dispose();

            _playerTank.Attacked -= OnPlayerTankAttacked;
        }

        private void OnPlayerTankAttacked()
        {
            _animator.SetTrigger(AnimationPath.IsShooted);

            StartCoroutine(Shooter());
            StartCoroutine(Rotater());
        }

        private bool CheckPlayerTankVisibility()
        {
            return Physics.Raycast(_shootPoint.position, (_playerTank.transform.position - _shootPoint.position).normalized, out RaycastHit hitInfo, RayCastDistance)
                && hitInfo.transform.TryGetComponent(out PlayerTank _);
        }

        private IEnumerator Shooter()
        {
            bool isShooted = true;
            WaitForSeconds reloadDuration = new WaitForSeconds(_weapon.ReloadDuration);
            WaitForSeconds shootCooldown = new WaitForSeconds(_weapon.ShootCooldown);

            while (isShooted)
            {
                yield return new WaitWhile(() => _isTurnedToPlayerTank == false || CheckPlayerTankVisibility() == false);

                _weapon.Shoot();

                if (_weapon.CanShoot == false)
                {
                    _weapon.Reload();
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
