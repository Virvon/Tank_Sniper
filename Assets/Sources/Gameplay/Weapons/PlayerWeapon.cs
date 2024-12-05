using Assets.Sources.Infrastructure.Factories.GameplayFactory;
using Assets.Sources.Services.InputService;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.Weapons
{
    public class PlayerWeapon : MonoBehaviour
    {
        [SerializeField] private uint _bulletsCapacity;
        [SerializeField] private uint _requireShotsNumberToSuperShot;

        private IGameplayFactory _gameplayFactory;
        private IInputService _inputService;

        private uint _shootsNumberToSuperShot;
        private uint _bulletsCount;

        [Inject]
        private void Construct(IGameplayFactory gameplayFactory, IInputService inputService)
        {
            _gameplayFactory = gameplayFactory;
            _inputService = inputService;

            _shootsNumberToSuperShot = _requireShotsNumberToSuperShot;
            _bulletsCount = _bulletsCapacity;

            _inputService.Shooted += OnShoted;
        }

        private void OnDestroy()
        {
            _inputService.Shooted -= OnShoted;
        }

        private void OnShoted()
        {
            if (_bulletsCount == 0)
                _bulletsCount = _bulletsCapacity;

            _bulletsCount--;

            if(_shootsNumberToSuperShot == 0)
            {
                _shootsNumberToSuperShot = _requireShotsNumberToSuperShot;

                SuperShoot();
            }
            else
            {
                _shootsNumberToSuperShot--;

                Shoot();
            }
        }

        private void Shoot()
        {

        }

        private void SuperShoot()
        {

        }
    }
}
