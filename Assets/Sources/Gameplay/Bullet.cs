using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay
{
    public class Bullet : MonoBehaviour
    {
        private readonly Collider[] _overlapColliders = new Collider[32];

        [SerializeField] private Rigidbody _rigidBody;
        [SerializeField] private float _speed;
        [SerializeField] private float _radius;

        private void Start()
        {
            _rigidBody.velocity = transform.forward * _speed;
        }

        private void OnCollisionEnter(Collision collision)
        {
            int overlapCount = Physics.OverlapSphereNonAlloc(transform.position, _radius, _overlapColliders);

            for (int i = 0; i < overlapCount; i++)
            {
                if (_overlapColliders[i].TryGetComponent(out IDamageable damageable))
                    damageable.TakeDamage(transform.position);
            }

            Destroy(gameObject);
        }

        public class Factory : PlaceholderFactory<string, Vector3, Quaternion, UniTask<Bullet>>
        {
        }
    }
}
