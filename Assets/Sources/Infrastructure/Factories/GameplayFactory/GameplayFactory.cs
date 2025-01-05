using Assets.Sources.Gameplay.Cameras;
using Assets.Sources.Gameplay.Enemies;
using Assets.Sources.Gameplay.Handlers;
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
        private readonly GameplayCamera.Factory _gameplayCameraFactory;
        private readonly Enemy.Factory _enemyFactory;
        private readonly AimingCamera.Factory _aimingFactory;
        private readonly WictoryHandler _winHandler;

        public GameplayFactory(
            IStaticDataService staticDataService,
            DiContainer container,
            GameplayCamera.Factory gameplayCameraFactory,
            Enemy.Factory enemyFactory,
            AimingCamera.Factory aimingFactory,
            WictoryHandler winHandler)
        {
            _staticDataService = staticDataService;
            _container = container;
            _gameplayCameraFactory = gameplayCameraFactory;
            _enemyFactory = enemyFactory;
            _aimingFactory = aimingFactory;
            _winHandler = winHandler;
        }

        public async UniTask CreateAimingVirtualCamera(Vector3 position, Quaternion rotation) =>
            await _aimingFactory.Create(GameplayFactoryAssets.AimingCamera, position, rotation);

        public async UniTask CreateCamera()
        {
            GameplayCamera gameplayCamera = await _gameplayCameraFactory.Create(GameplayFactoryAssets.Camera);

            _container.BindInstance(gameplayCamera).AsSingle();
        }

        public async UniTask<Enemy> CreateEnemy(EnemyType type, Vector3 position, Quaternion rotation)
        {
            Enemy enemy = await _enemyFactory.Create(_staticDataService.GetEnemy(type).AssetReference, position, rotation);

            _winHandler.AddEnemy(enemy);

            return enemy;
        }
    }
}
