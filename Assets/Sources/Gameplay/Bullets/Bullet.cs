using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Assets.Sources.Gameplay.Bullets
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidBody;
        [SerializeField] private float _speed;

        private void Start()
        {
            _rigidBody.velocity = transform.forward * _speed;
        }

        protected virtual void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.TryGetComponent(out IDamageable damageable))
                damageable.TakeDamage(transform.position, 1000);

            Destroy(gameObject);
        }

        public class Factory : PlaceholderFactory<AssetReferenceGameObject, Vector3, Quaternion, UniTask<Bullet>>
        {
        }
    }
}
