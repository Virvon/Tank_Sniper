using Assets.Sources.Gameplay;
using Assets.Sources.Gameplay.Bullets;
using Assets.Sources.Gameplay.Enemies;
using Assets.Sources.Services.StaticDataService;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Infrastructure.Factories.GameplayFactory
{
    public class GameplayFactory : IGameplayFactory
    {
        private readonly IStaticDataService _staticDataService;
        private readonly DiContainer _container;
        private readonly ExplosionBullet.Factory _bulletFactory;
        private readonly PlayerTank.Factory _playerTankFactory;
        private readonly GameplayCamera.Factory _gameplayCameraFactory;
        private readonly Enemy.Factory _enemyFactory;

        public GameplayFactory(IStaticDataService staticDataService, DiContainer container, ExplosionBullet.Factory bulletFactory, PlayerTank.Factory playerTankFactory, GameplayCamera.Factory gameplayCameraFactory, Enemy.Factory enemyFactory)
        {
            _staticDataService = staticDataService;
            _container = container;
            _bulletFactory = bulletFactory;
            _playerTankFactory = playerTankFactory;
            _gameplayCameraFactory = gameplayCameraFactory;
            _enemyFactory = enemyFactory;
        }

        public async UniTask CreateBullet(BulletType type, Vector3 position, Quaternion rotation) =>
            await _bulletFactory.Create(_staticDataService.GetBullet(type).AssetReference, position, rotation);

        public async UniTask CreatePlayerTank()
        {
            PlayerTank playerTank = await _playerTankFactory.Create(GameplayFactoryAssets.PlayerTank);

            _container.BindInstance(playerTank).AsSingle();
        }

        public async UniTask CreateCamera() =>
            await _gameplayCameraFactory.Create(GameplayFactoryAssets.Camera);

        public async UniTask CreateEnemy(Vector3 position, Quaternion rotation) =>
            await _enemyFactory.Create(GameplayFactoryAssets.Enemy, position, rotation);
    }
}
