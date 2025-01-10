using Assets.Sources.Gameplay.Weapons;
using System.Collections;
using UnityEngine;

namespace Assets.Sources.Gameplay.Enemies.Helicopter.BossHelicopter
{
    public class BossHelicopterShooting : EnemyForwartFlyingBulletsShooting
    {
        [SerializeField] private float _shootDuration;
        [SerializeField] private Transform _bulletPoint;
        [SerializeField] private Transform _helicopter;

        private bool _isDestructed = false;

        protected override bool CanShoot => _isDestructed == false && base.CanShoot && Vector2.Angle(new Vector2(PlayerWrapper.transform.position.x - LookStartPosition.x, PlayerWrapper.transform.position.z - LookStartPosition.z), new Vector2(_helicopter.transform.forward.x, _helicopter.transform.forward.z)) < AngleDelta;

        protected override Vector3 LookStartPosition => transform.position;

        public void Destruct() =>
            _isDestructed = true;

        protected override Vector3 GetCurrentShootPointPosition() =>
            _bulletPoint.position;

        protected override void Shoot()
        {
            StartCoroutine(Waiter());
        }

        private IEnumerator Waiter()
        {
            yield return new WaitForSeconds(_shootDuration);

            base.Shoot();
        }
    }
}