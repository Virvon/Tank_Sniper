using Assets.Sources.Infrastructure.Factories.GameplayFactory;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Assets.Sources.Gameplay.Bullets
{
    public class CompositeBullet : CollidingBullet
    {
        private readonly Vector3 BombDiretionOffset = new Vector3(0, 2, 0);

        private IGameplayFactory _gameplayFactory;
        private uint _bombsCount;

        [Inject]
        private void Construct(IGameplayFactory gameplayFactory) =>
            _gameplayFactory = gameplayFactory;

        public CompositeBullet BindBombsCount(uint bombsCount)
        {
            _bombsCount = bombsCount;

            return this;
        }

        protected override void Explode()
        {
            base.Explode();
            CreateBombs();
        }

        private void CreateBombs()
        {
            for (int i = 0; i < _bombsCount; i++)
            {
                Vector3 bombDirection = (Random.onUnitSphere + BombDiretionOffset).normalized;

                //_gameplayFactory.CreateBomb(Types.BulletType.Bomb, transform.position, Quaternion.LookRotation(bombDirection));
            }
        }
    }
}
