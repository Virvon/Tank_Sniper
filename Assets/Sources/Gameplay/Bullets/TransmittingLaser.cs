using Assets.Sources.Gameplay.Enemies;
using Assets.Sources.Gameplay.Handlers;
using Assets.Sources.Infrastructure.Factories.BulletFactory;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.Bullets
{
    public class TransmittingLaser : DirectionalLaser
    {
        private readonly Vector3 _offset = new Vector3(0, 1, 0);

        private IBulletFactory _bulletFactory;
        private WictoryHandler _wictoryHandler;

        [Inject]
        private void Construct(IBulletFactory bulletFactory, WictoryHandler wictoryHandler)
        {
            _bulletFactory = bulletFactory;
            _wictoryHandler = wictoryHandler;
        }

        public TransmittingLaser BindTargetsCount(int targetsCount)
        {
            CreateLaser(targetsCount);

            return this;
        }

        private async void CreateLaser(int targetsCount)
        {
            if (Launch(out RaycastHit hitInfo))
            {
                IReadOnlyList<Enemy> enemies = _wictoryHandler.Enemies;

                enemies = enemies.Where(enemy => enemy.IsDestructed == false).OrderBy(enemy => Vector3.Distance(hitInfo.point, enemy.transform.position)).Take(targetsCount).ToArray();

                if (enemies.Count == 0)
                    return;

                Vector3 fitsPoint = hitInfo.point;

                if (hitInfo.transform.TryGetComponent(out Enemy _))
                    enemies = enemies.Skip(1).ToArray();

                await _bulletFactory.CreateTargetingLaser(fitsPoint, enemies[1].transform.position + _offset);

                for (int i = 0; i < enemies.Count - 1; i++)
                    await _bulletFactory.CreateTargetingLaser(enemies[i].transform.position + _offset, enemies[i + 1].transform.position + _offset);
            }
        }
    }
}
