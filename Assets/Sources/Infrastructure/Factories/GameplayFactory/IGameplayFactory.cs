using Assets.Sources.Gameplay.Enemies;
using Assets.Sources.Gameplay.Weapons;
using Assets.Sources.Types;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Infrastructure.Factories.GameplayFactory
{
    public interface IGameplayFactory
    {
        UniTask CreateBullet(BulletType type, Vector3 position, Quaternion rotation);
        UniTask CreateCamera();
        UniTask<Enemy> CreateEnemy(EnemyType type, Vector3 position, Quaternion rotation);
        UniTask CreatePlayerTank();
        UniTask<TankRocket> CreateTankRocked(BulletType type, Vector3 position, Quaternion rotation);
    }
}