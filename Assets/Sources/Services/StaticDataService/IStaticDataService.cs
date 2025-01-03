﻿using Assets.Sources.Services.StaticDataService.Configs;
using Assets.Sources.Services.StaticDataService.Configs.Bullets.Colliding;
using Assets.Sources.Services.StaticDataService.Configs.Bullets.Laser;
using Assets.Sources.Services.StaticDataService.Configs.Level;
using Assets.Sources.Types;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Services.StaticDataService
{
    public interface IStaticDataService
    {
        LaserConfig DiretionalLaserConfig { get; }
        TargetingLaserConfig TargetingLaserConfig { get; }
        TransmittingLaserConfig TransmittedLaserConfig { get; }
        TankConfig[] TankConfigs { get; }
        TankSkinConfig[] TankSkinConfigs { get; }
        DecalConfig[] DecalConfigs { get; }
        AnimationsConfig AnimationsConfig { get; }
        AimingConfig AimingConfig { get; }
        DestructionConfig DestructionConfig { get; }
        EnemiesSettingsConfig EnemiesSettingsConfig { get; }

        EnemyConfig GetEnemy(EnemyType type);
        LevelConfig GetLevel(string key);
        UniTask InitializeAsync();
        ForwardFlyingBulletConfig GetBullet(ForwardFlyingBulletType type);
        HomingBulletConfig GetBullet(HomingBulletType type);
        TankConfig GetTank(uint level);
        TankSkinConfig GetSkin(TankSkinType type);
        DecalConfig GetDecal(DecalType type);
        MuzzleConfig GetMuzzle(MuzzleType type);
    }
}