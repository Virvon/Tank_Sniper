using UnityEngine;

namespace Assets.Sources.Gameplay.Destructions
{
    public class DroneExplosion : Explosion
    {
        [SerializeField] private float _radius;
        [SerializeField] private uint _force;
        [SerializeField] private uint _damage;
        [SerializeField] private float _lifeTime;

        public void Explode()
        {
            CreateExplosionParticle(transform.position, Quaternion.identity);
            Explode(transform.position, _radius, _force, _damage);
            Destroy(gameObject, _lifeTime);
        }
    }
}