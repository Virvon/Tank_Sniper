using Assets.Sources.Infrastructure.Factories.GameplayFactory;
using Assets.Sources.Types;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.Enemies
{
    public class EnemyCar : EnemyEnginery
    {
        [SerializeField] private EnemyType _attackedEnemyType;
        [SerializeField] private Transform _attackedEnemyPoint;

        private DestructedEnemy _attackedEnemy;

        [Inject]
        private async void Construct(IGameplayFactory gameplayFactory)
        {
            Enemy enemy = await gameplayFactory.CreateEnemy(_attackedEnemyType, _attackedEnemyPoint.position, transform.rotation);

            _attackedEnemy = enemy.GetComponent<DestructedEnemy>();
            _attackedEnemy.transform.parent = transform;
            _attackedEnemy.transform.position = _attackedEnemyPoint.position;
        }

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
