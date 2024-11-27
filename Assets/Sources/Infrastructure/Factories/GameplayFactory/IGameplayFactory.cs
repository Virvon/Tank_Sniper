using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Infrastructure.Factories.GameplayFactory
{
    public interface IGameplayFactory
    {
        UniTask CreateBullet(BulletType type, Vector3 position, Quaternion rotation);
        UniTask CreateCamera();
        UniTask CreateEnemy(EnemyType type, Vector3 position, Quaternion rotation);
        UniTask CreatePlayerTank();
    }
}