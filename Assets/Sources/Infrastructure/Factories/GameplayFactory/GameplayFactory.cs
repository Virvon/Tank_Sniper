using Assets.Sources.Gameplay;
using Assets.Sources.Gameplay.Bullets;
using Assets.Sources.Gameplay.Enemies;
using Assets.Sources.Gameplay.Weapons;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs;
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
        //private readonly ExplosionBullet.Factory _bulletFactory;
        private readonly PlayerTank.Factory _playerTankFactory;
        private readonly GameplayCamera.Factory _gameplayCameraFactory;
        private readonly Enemy.Factory _enemyFactory;
        private readonly Car.Factory _carFactory;
        private readonly TankRocket.Factory _tankRocketFactory;
        private readonly Laser.Factory _laserFactory;

        public GameplayFactory(IStaticDataService staticDataService, DiContainer container, PlayerTank.Factory playerTankFactory, GameplayCamera.Factory gameplayCameraFactory, Enemy.Factory enemyFactory, Car.Factory carFactory, TankRocket.Factory tankRocketFactory, Laser.Factory laserFactory)
        {
            _staticDataService = staticDataService;
            _container = container;
            //_bulletFactory = bulletFactory;
            _playerTankFactory = playerTankFactory;
            _gameplayCameraFactory = gameplayCameraFactory;
            _enemyFactory = enemyFactory;
            _carFactory = carFactory;
            _tankRocketFactory = tankRocketFactory;
            _laserFactory = laserFactory;
        }

        public async UniTask<Laser> CreateLaser(BulletType type, Vector3 position, Quaternion rotation)
        {
            BulletConfig laserConfig = _staticDataService.GetBullet(type);
            Laser laser = await _laserFactory.Create(laserConfig.AssetReference, position, rotation);

            laser.Initialize(laserConfig.ExplosionLifeTime, laserConfig.ExplosionRadius, laserConfig.ExplosionForce, laserConfig.ProjectileLifeTime);

            return laser;
        }

        public async UniTask<TankRocket> CreateTankRocked(BulletType type, Vector3 position, Quaternion rotation)
        {
            BulletConfig tankRocketConfig = _staticDataService.GetBullet(type);
            TankRocket tankRocket = await _tankRocketFactory.Create(tankRocketConfig.AssetReference, position, rotation);

            tankRocket.Initialize(tankRocketConfig.ExplosionRadius, tankRocketConfig.FlightSpeed, tankRocketConfig.ExplosionLifeTime, tankRocketConfig.ExplosionForce);

            return tankRocket;
        }

        public async UniTask CreateBullet(BulletType type, Vector3 position, Quaternion rotation)
        {
            //await _bulletFactory.Create(_staticDataService.GetWeapon(type).BulletAssetReference, position, rotation);
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
