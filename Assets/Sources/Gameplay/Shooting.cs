using Assets.Sources.Infrastructure.Factories.GameplayFactory;
using Assets.Sources.Services.InputService;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay
{
    public class Shooting : MonoBehaviour
    {
        private IInputService _inputService;
        private IGameplayFactory _gameplayFactory;

        [Inject]
        private void Construct(IInputService inputService, IGameplayFactory gameplayFactory)
        {
            _inputService = inputService;
            _gameplayFactory = gameplayFactory;

            _inputService.Shooted += OnShooted;
        }

        private void OnDestroy()
        {
            _inputService.Shooted -= OnShooted;
        }

        private void OnShooted()
        {
            _gameplayFactory.CreateBullet(transform.position, transform.rotation);
        }
    }
}
