using Assets.Sources.Types;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Infrastructure.Factories.BulletFactory
{
    public interface IBulletFactory
    {
        UniTask CreateDirectionalLaser(Vector3 position, Quaternion rotation);
        UniTask CreateForwardFlyingBullet(ForwardFlyingBulletType type, Vector3 position, Quaternion rotation);
        UniTask CreateHomingBullet(HomingBulletType type, Vector3 position, Quaternion rotation);
        UniTask CreateMuzzle(MuzzleType type, Vector3 position, Quaternion rotation);
        UniTask CreateTargetingLaser(Vector3 position, Vector3 targetPosition);
        UniTask CreateTransmittingLaser(Vector3 positoin, Quaternion rotation);
    }
}