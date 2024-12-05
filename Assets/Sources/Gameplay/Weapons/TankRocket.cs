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

        private uint _explosionRadius;
        private uint _flightSpeed;
        private float _explosionSpreadDuraion;
        private List<IDamageable> _hitedTargets;
        private float _gizmosRadius;

        public void Initialize(uint explosionRadius, uint flightSpeed, float explosionSpreadSpeed)
        {
            _explosionRadius = explosionRadius;
            _flightSpeed = flightSpeed;
            _explosionSpreadDuraion = explosionSpreadSpeed;

            _rigidBody.velocity = transform.forward * _flightSpeed;
            _hitedTargets = new();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.transform.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(transform.position);
                _hitedTargets.Add(damageable);
            }

            StartCoroutine(Explosition());
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            Gizmos.DrawSphere(transform.position, _gizmosRadius);
        }

        private IEnumerator Explosition()
        {
            float currentExplosionRadius = 0;
            float passedTime = 0;
            float progress;

            while(currentExplosionRadius < _explosionRadius)
            {
                passedTime += Time.deltaTime;
                progress = passedTime / _explosionSpreadDuraion;

                currentExplosionRadius = Mathf.Lerp(0, _explosionRadius, progress);

                int overlapCount = Physics.OverlapSphereNonAlloc(transform.position, currentExplosionRadius, _overlapColliders);

                _gizmosRadius = currentExplosionRadius;

                for (int i = 0; i < overlapCount; i++)
                {
                    if (_overlapColliders[i].TryGetComponent(out IDamageable damageable) && _hitedTargets.Contains(damageable) == false)
                    {
                        damageable.TakeDamage(transform.position);
                        _hitedTargets.Add(damageable);
                    }
                }

                yield return null;
            }

            _gizmosRadius = 0;
        }

        public class Factory : PlaceholderFactory<AssetReferenceGameObject, Vector3, Quaternion, UniTask<TankRocket>>
        {
        }
    }
}
