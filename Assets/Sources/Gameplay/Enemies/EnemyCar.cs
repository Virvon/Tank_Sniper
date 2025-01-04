using UnityEngine;

namespace Assets.Sources.Gameplay.Enemies
{
    public class EnemyCar : EnemyEnginery
    {
        [SerializeField] private DestructedEnemy _attackedEnemy;

        protected override uint CalculateDamga(ExplosionInfo explosionInfo) =>
            explosionInfo.IsDamageableCollided ? explosionInfo.Damage : explosionInfo.Damage / 2;

        protected override void Destruct(ExplosionInfo explosionInfo)
        {
            base.Destruct(explosionInfo);

            _attackedEnemy.Destruct(
                    (explosionInfo.ExplosionPosition + transform.position) / 2,
                    explosionInfo.ExplosionForce + EnemyEngineryExplosion.ExplosionForce);
        }
    }
}
