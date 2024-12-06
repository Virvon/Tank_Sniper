using Assets.Sources.Gameplay.Enemies;
using Assets.Sources.Infrastructure.Factories.GameplayFactory;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.Bullets
{
    public class TransmittingLaser : DirectionalLaser
    {
        private IGameplayFactory _gamePlayFactory;

        [Inject]
        private void Construct(IGameplayFactory gameplayFactory)
        {
            _gamePlayFactory = gameplayFactory;
        }

        public TransmittingLaser BindTargetsCount(int targetsCount)
        {
            CreateLaser(targetsCount);

            return this;
        }

        private void CreateLaser(int targetsCount)
        {
            if (Launch(out RaycastHit hitInfo))
            {
                Enemy[] enemies = FindObjectsOfType<Enemy>();

                enemies = enemies.OrderBy(enemy => Vector3.Distance(hitInfo.point, enemy.transform.position)).Take(targetsCount).ToArray();

                Vector3 fitsPoint;

                if (hitInfo.transform.TryGetComponent(out Enemy _))
                {
                    fitsPoint = enemies.First().transform.position;
                    enemies = enemies.Skip(1).ToArray();
                }
                else
                {
                    fitsPoint = hitInfo.point;
                }

                //_gamePlayFactory.CreteLaser2(Types.BulletType.Laser2, fitsPoint, enemies[1].transform.position);

                for (int i = 1; i < enemies.Length - 1; i++)
                {
                    //_gamePlayFactory.CreteLaser2(Types.BulletType.Laser2, enemies[i].transform.position, enemies[i + 1].transform.position);
                }
            }
        }
    }
}
