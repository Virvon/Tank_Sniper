using UnityEngine;

namespace Assets.Sources.Gameplay.Bullets
{
    public class ExplosionBullet : Bullet
    {
        private readonly Collider[] _overlapColliders = new Collider[32];
        
        [SerializeField] private float _radius;

        protected override void OnCollisionEnter(Collision collision)
        {
            int overlapCount = Physics.OverlapSphereNonAlloc(transform.position, _radius, _overlapColliders);

            for (int i = 0; i < overlapCount; i++)
            {
                if (_overlapColliders[i].TryGetComponent(out IDamageable damageable))
                    damageable.TakeDamage(transform.position, 1000);
            }

            Destroy(gameObject);
        }
    }
}
