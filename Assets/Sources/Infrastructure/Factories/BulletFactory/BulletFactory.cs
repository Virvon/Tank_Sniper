﻿using Assets.Sources.Gameplay.Bullets;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs;
using Assets.Sources.Services.StaticDataService.Configs.Bullets.Colliding;
using Assets.Sources.Services.StaticDataService.Configs.Bullets.Laser;
using Assets.Sources.Types;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Assets.Sources.Infrastructure.Factories.BulletFactory
{
    public class BulletFactory : IBulletFactory
    {
        private readonly IStaticDataService _staticDataService;
        private readonly Factory<CollidingBullet> _forwardFlyingBulletFactory;
        private readonly Factory<HomingBullet> _homingBulletFactory;
        private readonly Factory<DirectionalLaser> _directionalLaserFactory;
        private readonly Factory<TargetingLaser> _targetingLaserFactory;
        private readonly Factory<TransmittingLaser> _transmittedLaserFactory;
        private readonly Factory<CompositeBullet> _compositeBulletFactory;
        private readonly Muzzle.Factory _muzzleFactory;

        public BulletFactory(
            IStaticDataService staticDataService,
            Factory<CollidingBullet> forwardFlyingBulletFactory,
            Factory<DirectionalLaser> directionalLaserFactory,
            Muzzle.Factory muzzleFactory,
            Factory<HomingBullet> homingBulletFactory,
            Factory<CompositeBullet> compositeBulletFactory,
            Factory<TransmittingLaser> transmittedLaserFactory,
            Factory<TargetingLaser> targetingLaserFactory)
        {
            _staticDataService = staticDataService;
            _forwardFlyingBulletFactory = forwardFlyingBulletFactory;
            _directionalLaserFactory = directionalLaserFactory;
            _muzzleFactory = muzzleFactory;
            _homingBulletFactory = homingBulletFactory;
            _compositeBulletFactory = compositeBulletFactory;
            _transmittedLaserFactory = transmittedLaserFactory;
            _targetingLaserFactory = targetingLaserFactory;
        }

        public async UniTask CreateMuzzle(MuzzleType type, Vector3 position, Quaternion rotation)
        {
            MuzzleConfig muzzleConfig = _staticDataService.GetMuzzle(type);
            Muzzle muzzle = await _muzzleFactory.Create(muzzleConfig.AssetReference, position, rotation);

            muzzle.SetLifeTime(muzzleConfig.LifeTime);
        }

        public async UniTask CreateTransmittingLaser(Vector3 positoin, Quaternion rotation)
        {
            TransmittingLaserConfig config = _staticDataService.TransmittedLaserConfig;
            TransmittingLaser laser = await _transmittedLaserFactory.Create(config.AssetReference, positoin, rotation);

            laser
                .BindTargetsCount(config.TargetsCount)
                .BindLifeTimes(config.ExplosionLifeTime, config.ProjectileLifeTime)
                .BindExplosionSettings(config.ExplosionRadius, config.ExplosionForce, config.Damage);
        }

        public async UniTask CreateTargetingLaser(Vector3 position, Vector3 targetPosition)
        {
            LaserConfig config = _staticDataService.TargetingLaserConfig;
            TargetingLaser laser = await _targetingLaserFactory.Create(config.AssetReference, position, Quaternion.identity);

            laser
                .BindTarget(targetPosition, position)
                .BindLifeTimes(config.ExplosionLifeTime, config.ProjectileLifeTime)
                .BindExplosionSettings(config.ExplosionRadius, config.ExplosionForce, config.Damage);
        }

        public UniTask CreateDirectionalLaser(Vector3 position, Quaternion rotation)
        {
            //Debug.Log("create direction laser");
            //LaserConfig config = _staticDataService.DiretionalLaserConfig;
            //Debug.Log(config != null);
            //Debug.Log(_directionalLaserFactory != null);
            //DirectionalLaser laser = await _directionalLaserFactory.Create(config.AssetReference, position, rotation);

            //laser
            //    .BindLifeTimes(config.ExplosionLifeTime, config.ProjectileLifeTime)
            //    .BindExplosionSettings(config.ExplosionRadius, config.ExplosionForce, config.Damage);
            return default;
        }

        public async UniTask CreateForwardFlyingBullet(ForwardFlyingBulletType type, Vector3 position, Quaternion rotation)
        {
            ForwardFlyingBulletConfig config = _staticDataService.GetBullet(type);
            CollidingBullet bullet = await _forwardFlyingBulletFactory.Create(config.AssetReference, position, rotation);

            bullet
                .BindSettings(config.ExplosionLifeTime, config.FlightSpeed, config.LifeTimeLimit)
                .BindExplosionSettings(config.ExplosionRadius, config.ExplosionForce, config.Damage);
        }

        public async UniTask CreateHomingBullet(HomingBulletType type, Vector3 position, Quaternion rotation)
        {
            HomingBulletConfig config = _staticDataService.GetBullet(type);
            HomingBullet bullet = await _homingBulletFactory.Create(config.AssetReference, position, rotation);

            bullet
                .BindHomingSettings(config.SearchRadius, config.RotationSpeed, config.TargetingDelay)
                .BindSettings(config.ExplosionLifeTime, config.FlightSpeed, config.LifeTimeLimit)
                .BindExplosionSettings(config.ExplosionRadius, config.ExplosionForce, config.Damage);
        }

        public async UniTask CreateCompositeBullet(Vector3 position, Quaternion rotation)
        {
            CompositeBulletConfig config = _staticDataService.CompositeBulletConfig;
            CompositeBullet bullet = await _compositeBulletFactory.Create(config.AssetReference, position, rotation);

            bullet
                .BindBombsCount(config.BombsCount)
                .BindSettings(config.ExplosionLifeTime, config.FlightSpeed, config.LifeTimeLimit)
                .BindExplosionSettings(config.ExplosionRadius, config.ExplosionForce, config.Damage);
        }

        public class Factory<TBullet> : PlaceholderFactory<AssetReferenceGameObject, Vector3, Quaternion, UniTask<TBullet>>
            where TBullet : ExplodingBullet
        {
        }
    }
}