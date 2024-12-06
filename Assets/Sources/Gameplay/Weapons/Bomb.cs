using Cysharp.Threading.Tasks;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Assets.Sources.Gameplay.Weapons
{
    public class Bomb : MonoBehaviour
    {
        private readonly Collider[] _overlapColliders = new Collider[32];

        [SerializeField] private Rigidbody _rigidBody;
        [SerializeField] private Collider _collider;
        [SerializeField] private ParticleSystem _explosionPrefab;
        [SerializeField] private GameObject _projectile;

        private float _explosionLifeTime;
        private uint _explosionForce;
        private float _explosionRadius;
        private float _projectileLifeTime;

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.transform.TryGetComponent(out IDamageable damageable))
                Explode();
        }

        private void Explode()
        {
            _rigidBody.velocity = Vector3.zero;
            _rigidBody.isKinematic = true;
            _collider.enabled = false;

            ParticleSystem explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity, transform);
            explosion.Play();

            int overlapCount = Physics.OverlapSphereNonAlloc(transform.position, _explosionRadius, _overlapColliders);

            for (int i = 0; i < overlapCount; i++)
            {
                if (_overlapColliders[i].TryGetComponent(out IDamageable damageable))
                    damageable.TakeDamage(transform.position, _explosionForce);
            }

            Destroy(gameObject, _explosionLifeTime);
            Destroy(_projectile);
        }

        public void Initialize(float explosionLifetime, uint explosionForce, float explosionRadius, float projectileLifeTime, uint flyingSpeed)
        {
            _explosionLifeTime = explosionLifetime;
            _explosionForce = explosionForce;
            _explosionRadius = explosionRadius;
            _projectileLifeTime = projectileLifeTime;

            _rigidBody.AddForce(transform.forward * flyingSpeed, ForceMode.Impulse);

            StartCoroutine(Waiter());
        }

        private IEnumerator Waiter()
        {
            yield return new WaitForSeconds(_projectileLifeTime);

            Explode();
        }

        public class Factory : PlaceholderFactory<AssetReferenceGameObject, Vector3, Quaternion, UniTask<Bomb>>
        {
        }
    }
}
