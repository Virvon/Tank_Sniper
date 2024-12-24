using Assets.Sources.Gameplay.Destructions;
using Assets.Sources.Gameplay.Enemies.Animation;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Assets.Sources.Gameplay.Enemies
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        [SerializeField] private Animator _animator;        
        [SerializeField] private DestructionPart _weapon;
        [SerializeField] private Collider _weaponCollider;
        [SerializeField] private Ragdoll _ragdoll;

        public event Action Destructed;

        private void Start() =>
            _ragdoll.SetActive(false);

        public void TakeDamage(Vector3 bulletPosition, uint explosionForce)
        {
            Destructed?.Invoke();

            _animator.enabled = false;

            _ragdoll.transform.parent = null;
            _ragdoll.Destruct(bulletPosition, explosionForce);

            _weaponCollider.enabled = true;
            _weapon.transform.parent = null;
            _weapon.Destruct(bulletPosition, explosionForce);

            Destroy(gameObject);
        }

        public class Factory : PlaceholderFactory<AssetReferenceGameObject, Vector3, Quaternion, UniTask<Enemy>>
        {
        }
    }
}
