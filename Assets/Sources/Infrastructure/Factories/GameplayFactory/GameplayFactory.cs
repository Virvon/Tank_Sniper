using Assets.Sources.Gameplay.Cameras;
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
        private readonly GameplayCamera.Factory _gameplayCameraFactory;
        private readonly Enemy.Factory _enemyFactory;
        private readonly Car.Factory _carFactory;
        private readonly AimingCamera.Factory _aimingFactory;

        public GameplayFactory(
            IStaticDataService staticDataService,
            DiContainer container,
            GameplayCamera.Factory gameplayCameraFactory,
            Enemy.Factory enemyFactory,
            Car.Factory carFactory,
            AimingCamera.Factory aimingFactory)
        {
            _staticDataService = staticDataService;
            _container = container;
            _gameplayCameraFactory = gameplayCameraFactory;
            _enemyFactory = enemyFactory;
            _carFactory = carFactory;
            _aimingFactory = aimingFactory;
        }

        public async UniTask CreateAimingVirtualCamera(Vector3 position) =>
            await _aimingFactory.Create(GameplayFactoryAssets.AimingCamera, position);

        public async UniTask CreateCamera()
        {
            GameplayCamera gameplayCamera = await _gameplayCameraFactory.Create(GameplayFactoryAssets.Camera);

            _container.BindInstance(gameplayCamera).AsSingle();
        }

        public async UniTask<Enemy> CreateEnemy(EnemyType type, Vector3 position, Quaternion rotation) =>
            await _enemyFactory.Create(_staticDataService.GetEnemy(type).AssetReference, position, rotation);
    }
}
