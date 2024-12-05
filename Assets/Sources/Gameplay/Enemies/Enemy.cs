using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Assets.Sources.Gameplay.Enemies
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        [SerializeField] private Vector3 _additionalDestructionDirection;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private ForceMode _forceMode;
        [SerializeField] private float _rotationForce = 1;

        public void TakeDamage(Vector3 bulletPosition, uint explosionForce)
        {
            Vector3 explosionDirection = (transform.position - bulletPosition).normalized;
            explosionDirection += _additionalDestructionDirection;
            explosionDirection.Normalize();

            _rigidbody.AddForce(explosionDirection * explosionForce, _forceMode);
            _rigidbody.AddTorque(explosionDirection * _rotationForce, _forceMode);
        }

        public class Factory : PlaceholderFactory<AssetReferenceGameObject, Vector3, Quaternion, UniTask<Enemy>>
        {
        }
    }
}
