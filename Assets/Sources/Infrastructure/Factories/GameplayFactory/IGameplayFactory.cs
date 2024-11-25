using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Infrastructure.Factories.GameplayFactory
{
    public interface IGameplayFactory
    {
        UniTask CreateBullet(Vector3 position, Quaternion rotation);
        UniTask CreateCamera();
        UniTask CreateEnemy();
        UniTask CreatePlayerTank();
    }
}