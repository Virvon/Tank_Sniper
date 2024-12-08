﻿using Assets.Sources.Gameplay.Enemies;
using Assets.Sources.Gameplay.Weapons;
using Assets.Sources.Types;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Infrastructure.Factories.GameplayFactory
{
    public interface IGameplayFactory
    {
        UniTask CreateBomb(BulletType type, Vector3 position, Quaternion rotation);
        UniTask CreateBullet(BulletType type, Vector3 position, Quaternion rotation);
        UniTask CreateCamera();
        UniTask CreateCompositeBullet(BulletType type, Vector3 position, Quaternion rotation);
        UniTask<Enemy> CreateEnemy(EnemyType type, Vector3 position, Quaternion rotation);
        UniTask<HomingBullet> CreateHomingBullet(BulletType type, Vector3 position, Quaternion rotation);
        UniTask<Laser> CreateLaser(BulletType type, Vector3 position, Quaternion rotation);
        UniTask CreatePlayerTank();
        UniTask<TankRocket> CreateTankRocked(BulletType type, Vector3 position, Quaternion rotation);
        UniTask CreateTransmittingLaser(BulletType type, Vector3 position, Quaternion rotation);
        UniTask CreteLaser2(BulletType type, Vector3 position, Vector3 targetPosition);
    }
}