using UnityEngine;

namespace Assets.Sources.Gameplay.Bullets
{
    public class CollidingBullet : ExplodingBullet
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Collider _collider;
        [SerializeField] private bool _isExplosionAlongMoveDiretion;

        private float _explosionLifeTime;
        private uint _flightSpeed;

        private bool _isDamageableCollided;

        protected virtual void OnCollisionEnter(Collision collision)
        {
            _isDamageableCollided = collision.transform.TryGetComponent(out IDamageable _);
            Explode();
        }

        public CollidingBullet BindSettings(float explosionLifeTime, uint flightSpeed, float lifeTimeLimit)
        {
            _explosionLifeTime = explosionLifeTime;
            _flightSpeed = flightSpeed;

            ChangeTrajectory();
            DestroyAfterLifeTimeLimit(lifeTimeLimit);

            return this;
        }

        protected virtual void Explode()
        {
            Stop();
            CreateExplosionParticle(transform.position, _isExplosionAlongMoveDiretion ? Quaternion.LookRotation(-transform.forward) : Quaternion.identity);
            DestroyProjectile();
            Explode(transform.position);
            Destroy();
        }

        protected void ChangeTrajectory() =>
            _rigidbody.velocity = transform.forward * _flightSpeed;

        private void Destroy() =>
            Destroy(gameObject, _explosionLifeTime);

        private void DestroyProjectile() =>
            Destroy(Projectile);

        protected virtual void DestroyAfterLifeTimeLimit(float lifeTimeLimt) =>
            Destroy(gameObject, lifeTimeLimt);

        private void Stop()
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.isKinematic = true;
            _collider.enabled = false;
        }
    }
}
