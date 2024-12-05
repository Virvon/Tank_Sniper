using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.VFX;
using Zenject;

namespace Assets.Sources.Gameplay.Weapons
{
    public class Laser : MonoBehaviour
    {
        private readonly Collider[] _overlapColliders = new Collider[32];

        private const float MaxDistance = 200;

        [SerializeField] private ParticleSystem _explosionParticlePrefab;
        [SerializeField] private GameObject _projetile;

        private float _explosionLifeTime;
        private float _explosionRadius;
        private uint _explosionForce;

        public void Initialize(float explosionLifeTime, float explosionRadius, uint explosionForce, float projectileLifeTime)
        {
            _explosionLifeTime = explosionLifeTime;
            _explosionRadius = explosionRadius;
            _explosionForce = explosionForce;

            List<IDamageable> hitedTargets = new();

            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, MaxDistance))
            {
                ParticleSystem explosionParticle = Instantiate(_explosionParticlePrefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal, transform.forward), transform);
                explosionParticle.Play();

                if (hitInfo.transform.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(hitInfo.point, _explosionForce);
                    hitedTargets.Add(damageable);
                }

                int overlapCount = Physics.OverlapSphereNonAlloc(hitInfo.point, _explosionRadius, _overlapColliders);

                for (int i = 0; i < overlapCount; i++)
                {
                    if (_overlapColliders[i].TryGetComponent(out damageable) && hitedTargets.Contains(damageable) == false)
                    {
                        damageable.TakeDamage(hitInfo.point, _explosionForce);
                        hitedTargets.Add(damageable);
                    }
                }
            }

            Destroy(gameObject, explosionLifeTime);
            Destroy(_projetile, projectileLifeTime);
        }

        public class Factory : PlaceholderFactory<AssetReferenceGameObject, Vector3, Quaternion, UniTask<Laser>>
        {
        }
    }
}
