using Assets.Sources.Infrastructure.Factories.GameplayFactory;
using Assets.Sources.Services.InputService;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.Weapons
{
    public class Shooting : MonoBehaviour
    {
        private IInputService _inputService;
        private IGameplayFactory _gameplayFactory;
        private PlayerTank _playerTank;

        [Inject]
        private void Construct(IInputService inputService, IGameplayFactory gameplayFactory, PlayerTank playerTank)
        {
            _inputService = inputService;
            _gameplayFactory = gameplayFactory;
            _playerTank = playerTank;

            _inputService.Shooted += OnShooted;
        }

        private void OnDestroy()
        {
            _inputService.Shooted -= OnShooted;
        }

        private void OnShooted()
        {
            _playerTank.Attack();
            //_gameplayFactory.CreateBullet(WeaponType.TestBullet, transform.position, transform.rotation);
        }
    }
}
