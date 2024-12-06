using Assets.Sources.Gameplay;
using Assets.Sources.Gameplay.Enemies;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Types;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Infrastructure.Factories.GameplayFactory
{
    public class GameplayFactory : IGameplayFactory
    {
        private readonly IStaticDataService _staticDataService;
        private readonly DiContainer _container;
        private readonly PlayerTank.Factory _playerTankFactory;
        private readonly GameplayCamera.Factory _gameplayCameraFactory;
        private readonly Enemy.Factory _enemyFactory;
        private readonly Car.Factory _carFactory;

        public GameplayFactory(
            IStaticDataService staticDataService,
            DiContainer container,
            PlayerTank.Factory playerTankFactory,
            GameplayCamera.Factory gameplayCameraFactory,
            Enemy.Factory enemyFactory,
            Car.Factory carFactory)
        {
            _staticDataService = staticDataService;
            _container = container;
            _playerTankFactory = playerTankFactory;
            _gameplayCameraFactory = gameplayCameraFactory;
            _enemyFactory = enemyFactory;
            _carFactory = carFactory;
        }

        public async UniTask CreatePlayerTank()
        {
            PlayerTank playerTank = await _playerTankFactory.Create(GameplayFactoryAssets.PlayerTank);

            _container.BindInstance(playerTank).AsSingle();
        }

        public async UniTask CreateCamera()
        {
            GameplayCamera gameplayCamera = await _gameplayCameraFactory.Create(GameplayFactoryAssets.Camera);

            _container.BindInstance(gameplayCamera).AsSingle();
        }

        public async UniTask<Enemy> CreateEnemy(EnemyType type, Vector3 position, Quaternion rotation) =>
            await _enemyFactory.Create(_staticDataService.GetEnemy(type).AssetReference, position, rotation);
    }
}
