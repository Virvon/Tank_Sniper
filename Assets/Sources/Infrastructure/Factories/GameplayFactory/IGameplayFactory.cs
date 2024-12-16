using Assets.Sources.Gameplay.Enemies;
using Assets.Sources.Types;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Infrastructure.Factories.GameplayFactory
{
    public interface IGameplayFactory
    {
        UniTask CreateAimingVirtualCamera(Vector3 position);
        UniTask CreateCamera();
        UniTask<Enemy> CreateEnemy(EnemyType type, Vector3 position, Quaternion rotation);
        UniTask CreatePlayerTank();
    }
}