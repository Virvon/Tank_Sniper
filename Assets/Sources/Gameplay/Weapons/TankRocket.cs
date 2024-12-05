using Assets.Sources.Gameplay.Bullets;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Assets.Sources.Gameplay.Weapons
{
    public class TankRocket : MonoBehaviour
    {
        private readonly Collider[] _overlapColliders = new Collider[32];

        [SerializeField] private Rigidbody _rigidBody;
        [SerializeField] private GameObject _projectile;
        [SerializeField] private Collider _collider;
        [SerializeField] private ParticleSystem _explosionParticlePrefab;

        private uint _explosionRadius;
        private uint _flightSpeed;
        private float _explosionLifeTime;
        private uint _explosionForce;

        public void Initialize(uint explosionRadius, uint flightSpeed, float explosionLifeTime, uint explosionForce)
        {
            _explosionRadius = explosionRadius;
            _flightSpeed = flightSpeed;
            _explosionLifeTime = explosionLifeTime;
            _explosionForce = explosionForce;

            _rigidBody.velocity = transform.forward * _flightSpeed;
        }

        private void OnCollisionEnter(Collision collision)
        {
            List<IDamageable> HitedTargets = new();

            _rigidBody.velocity = Vector3.zero;
            _rigidBody.isKinematic = true;
            _collider.enabled = false;

            ParticleSystem explosionParticle = Instantiate(_explosionParticlePrefab, transform.position, Quaternion.identity, transform);
            explosionParticle.Play();

            Destroy(_projectile);

            if(collision.transform.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(transform.position, _explosionForce);
                HitedTargets.Add(damageable);
            }

            int overlapCount = Physics.OverlapSphereNonAlloc(transform.position, _explosionRadius, _overlapColliders);

            for (int i = 0; i < overlapCount; i++)
            {
                if (_overlapColliders[i].TryGetComponent(out damageable) && HitedTargets.Contains(damageable) == false)
                {
                    damageable.TakeDamage(transform.position, _explosionForce);
                    HitedTargets.Add(damageable);
                }
            }

            Destroy(gameObject, _explosionLifeTime);
        }

        public class Factory : PlaceholderFactory<AssetReferenceGameObject, Vector3, Quaternion, UniTask<TankRocket>>
        {
        }
    }
    public class Laser : MonoBehaviour
    {
        private const float MaxDistance = 200;

        [SerializeField] private ParticleSystem _explosionParticlePrefab;
        [SerializeField] private ParticleSystem _projectileParticlePrefab;

        private float _lifeTime;
        private float _explosionRadius;
        private uint _explosionForce;

        public void Initialize(float lifeTime, float explosionRadius, uint explosionForce)
        {
            _lifeTime = lifeTime;
            _explosionRadius = explosionRadius;
            _explosionForce = explosionForce;

            if(Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, MaxDistance))
            {
                ParticleSystem explosionParticle = Instantiate(_explosionParticlePrefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal, transform.forward), transform);
                explosionParticle.Play();
            }

            Destroy(gameObject, lifeTime);
        }

        private void CreateTrail(Vector3 endPosition)
        {
            float size = Vector3.Distance(transform.position, endPosition);

            Instantiate(_projectileParticlePrefab, transform.position, transform.rotation)
        }
    }
}
