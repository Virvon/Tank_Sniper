using Assets.Sources.Gameplay.Destructions;
using System;
using UnityEngine;

namespace Assets.Sources.Gameplay.Enemies
{
    public abstract class EnemyEnginery : Enemy, IDamageable, IHealthable
    {
        [SerializeField] private uint _health;
        [SerializeField] private DestructionedMaterialsRenderer _destructionedMaterialRenderer;
        [SerializeField] private Collider _collider;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private DestructionPart[] _destructionParts;
        [SerializeField] private EnemyEngineryExplosion _enemyEngineryExplosion;
        
        private bool _isDestructed;
        
        public uint MaxHealth { get; private set; }

        protected EnemyEngineryExplosion EnemyEngineryExplosion => _enemyEngineryExplosion;

        public event Action<uint, uint> Damaged;

        private void Start()
        {
            MaxHealth = _health;
            _isDestructed = false;
        }

        public void TakeDamage(ExplosionInfo explosionInfo)
        {
            if (_isDestructed)
                return;

            TakeDamage(CalculateDamga(explosionInfo));

            if (_health == 0)
            {
                _isDestructed = true;
                Destruct(explosionInfo);
            }
        }

        protected virtual void Destruct(ExplosionInfo explosionInfo)
        {
            OnDestructed();
           
            _destructionedMaterialRenderer.Render();

            _collider.enabled = false;
            _rigidbody.isKinematic = true;

            foreach (DestructionPart destructionPart in _destructionParts)
            {
                destructionPart.transform.parent = null;
                destructionPart.Destruct(
                    (explosionInfo.ExplosionPosition + transform.position) / 2,
                    explosionInfo.ExplosionForce + _enemyEngineryExplosion.ExplosionForce);
            }

            _enemyEngineryExplosion.Explode();
        }

        protected abstract uint CalculateDamga(ExplosionInfo explosionInfo);

        private void TakeDamage(uint damage)
        {
            if (damage > _health)
                damage = _health;

            _health -= damage;

            Damaged?.Invoke(_health, damage);
        }
    }
}
