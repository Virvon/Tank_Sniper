using System;
using System.Linq;
using UnityEngine;

namespace Assets.Sources.Gameplay.Enemies
{
    public class EnemyTank : EnemyEnginery
    {
        [SerializeField] private EnemyTankPart[] _parts;

        protected override uint CalculateDamga(ExplosionInfo explosionInfo)
        {
            if (explosionInfo.IsDamageableCollided)
            {
                EnemyTankPart part = _parts.OrderBy(part => part.GetDistanceTo(explosionInfo.ExplosionPosition)).First();

                return (uint)(explosionInfo.Damage * part.DamageModifier);
            }
            else
            {
                return explosionInfo.Damage / 3;
            }
        }
    }
}
