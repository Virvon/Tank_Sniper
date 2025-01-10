using Assets.Sources.Gameplay.Destructions;
using Assets.Sources.Gameplay.Enemies.Submarine;
using Assets.Sources.Infrastructure.Factories.BulletFactory;
using Assets.Sources.Types;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.Weapons
{
    public class EnemySubmarineShooting : EnemyShooting
    {
        [SerializeField] private Transform _shootPoint;
        [SerializeField] private float _shootDuration;
        [SerializeField] private BossDestructionPart _bossDestructionPart;
        [SerializeField] private EnemySubmarine _enemySubmarine;
        [SerializeField] private MuzzleType _muzzleType;

        private IBulletFactory _bulletFactory;

        protected override bool CanShoot => base.CanShoot && _bossDestructionPart.IsDesturcted == false && _enemySubmarine.IsSurfaced && _enemySubmarine.IsDestructed == false;

        [Inject]
        private void Construct(IBulletFactory bulletFactory) =>
            _bulletFactory = bulletFactory;

        protected override Vector3 GetCurrentShootingPosition() =>
            _shootPoint.position;

        protected override IEnumerator Shooter()
        {
            WaitForSeconds delay = new WaitForSeconds(_shootDuration);

            bool isShooted = true;

            while (isShooted)
            {
                if(CanShoot)
                {
                    _bulletFactory.CreateHomingBullet(HomingBulletType.SubmarineRocket, _shootPoint.position, _shootPoint.rotation);
                    _bulletFactory.CreateMuzzle(_muzzleType, _shootPoint.position, _shootPoint.rotation);
                }
                yield return delay;
            }
        }
    }
}