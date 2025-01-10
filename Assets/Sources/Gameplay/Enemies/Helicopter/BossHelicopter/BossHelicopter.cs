using System;
using UnityEngine;

namespace Assets.Sources.Gameplay.Enemies.Helicopter.BossHelicopter
{
    public class BossHelicopter : Helicopter, IHealthable
    {
        private const float Delta = 1.5f;

        [SerializeField] private uint _health;
        [SerializeField] BossHelicopterWeaponPart[] _weaponParts;

        public uint MaxHealth { get; private set; }

        public event Action<uint, uint> Damaged;

        protected override void Start()
        {
            MaxHealth = _health;
            base.Start();
        }

        protected override void OnDamaged(ExplosionInfo explosionInfo)
        {
            if (IsDestructed)
                return;

            TakeDamage(explosionInfo.Damage);

            foreach(BossHelicopterWeaponPart weaponPart in _weaponParts)
            {
                if (weaponPart == null)
                    continue;

                if (Vector3.Distance(weaponPart.transform.position, explosionInfo.ExplosionPosition) < Delta
                    && weaponPart.IsDesturcted == false)
                    weaponPart.Destruct(explosionInfo.ExplosionPosition, explosionInfo.ExplosionForce);
            }

            if (_health == 0)
                base.OnDamaged(explosionInfo);
        }

        private void TakeDamage(uint damage)
        {
            if (damage > _health)
                damage = _health;

            _health -= damage;

            Damaged?.Invoke(_health, damage);
        }
    }
}
