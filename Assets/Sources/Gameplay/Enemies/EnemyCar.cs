using Assets.Sources.Gameplay.Destructions;
using Assets.Sources.Infrastructure.Factories.GameplayFactory;
using Assets.Sources.Types;
using System;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.Enemies
{
    public class EnemyCar : DamagebleEnemy
    {
        [SerializeField] private uint _health;
        [SerializeField] private DestructionedMaterialsRenderer _destructionedMaterialRenderer;
        [SerializeField] private Collider _collider;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private DestructionPart[] _destructionParts;
        [SerializeField] private EnemyEngineryExplsion _enemyEngineryExplsion;
        [SerializeField] private EnemyType _attackedEnemyType;
        [SerializeField] private Transform _attackedEnemyPoint;

        private bool _isDestructed;
        private DestructedEnemy _attackedEnemy;

        public uint MaxHealth { get; private set; }

        public event Action<uint, uint> Damaged;

        [Inject]
        private async void Construct(IGameplayFactory gameplayFactory)
        {
            MaxHealth = _health;
            _isDestructed = false;

            Enemy enemy = await gameplayFactory.CreateEnemy(_attackedEnemyType, _attackedEnemyPoint.position, transform.rotation);
            
            _attackedEnemy = enemy.GetComponent<DestructedEnemy>();
            _attackedEnemy.transform.parent = transform;
            _attackedEnemy.transform.position = _attackedEnemyPoint.position;
        }

        public override void TakeDamage(ExplosionInfo explosionInfo)
        {
            if (_isDestructed)
                return;

            TakeDamage(explosionInfo.IsDamageableCollided ? explosionInfo.Damage : explosionInfo.Damage / 2);

            if(_health == 0)
            {
                OnDestructed();

                _isDestructed = true;
                _destructionedMaterialRenderer.Render();

                _collider.enabled = false;
                _rigidbody.isKinematic = true;

                foreach(DestructionPart destructionPart in _destructionParts)
                {
                    destructionPart.transform.parent = null;
                    destructionPart.Destruct(
                        (explosionInfo.ExplosionPosition + transform.position) / 2,
                        explosionInfo.ExplosionForce + _enemyEngineryExplsion.ExplosionForce);
                }

                _attackedEnemy.Destruct(
                        (explosionInfo.ExplosionPosition + transform.position) / 2,
                        explosionInfo.ExplosionForce + _enemyEngineryExplsion.ExplosionForce);

                _enemyEngineryExplsion.Explode();
            }
        }

        private void TakeDamage(uint damage)
        {
            if(damage > _health)
                damage = _health;

            _health -= damage;

            Damaged?.Invoke(_health, damage);
        }
    }
}
