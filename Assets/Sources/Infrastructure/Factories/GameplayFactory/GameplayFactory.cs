using Assets.Sources.Gameplay;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Infrastructure.Factories.GameplayFactory
{
    public class GameplayFactory : IGameplayFactory
    {
        private readonly Bullet.Factory _bulletFactory;

        public GameplayFactory(Bullet.Factory bulletFactory)
        {
            _bulletFactory = bulletFactory;
        }

        public async UniTask CreateBullet(Vector3 position, Quaternion rotation) =>
            await _bulletFactory.Create(GameplayFactoryAssets.Bullet, position, rotation);
    }
}
