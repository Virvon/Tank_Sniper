using Assets.Sources.Gameplay.Destructions;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Assets.Sources.Gameplay.Enemies
{
    public class Enemy : DestructionPart, IDamageable
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Collider _mainColider;
        [SerializeField] private Rigidbody _destructionRigidbody;
        [SerializeField] private DestructionPart _weapon;

        public event Action Destructed;

        public void TakeDamage(Vector3 bulletPosition, uint explosionForce)
        {
            Destructed?.Invoke();

            _animator.enabled = false;
            _mainColider.enabled = false;

            _weapon.transform.parent = null;
            _weapon.Destruct(bulletPosition, explosionForce);

            Destruct(bulletPosition, explosionForce);
        }

        protected override Rigidbody GetDestructionRigidbody() =>
            _destructionRigidbody;

        public class Factory : PlaceholderFactory<AssetReferenceGameObject, Vector3, Quaternion, UniTask<Enemy>>
        {
        }
    }
}
