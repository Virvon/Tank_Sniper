using Assets.Sources.Gameplay.Enemies;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Infrastructure.Factories.GameplayFactory
{
    public interface IGameplayFactory
    {
        UniTask CreateBullet(WeaponType type, Vector3 position, Quaternion rotation);
        UniTask CreateCamera();
        UniTask<Enemy> CreateEnemy(EnemyType type, Vector3 position, Quaternion rotation);
        UniTask CreatePlayerTank();
    }
}