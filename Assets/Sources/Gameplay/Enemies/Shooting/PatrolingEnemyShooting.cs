using Assets.Sources.Gameplay.Enemies.Movement;
using System.Collections;
using UnityEngine;

namespace Assets.Sources.Gameplay.Weapons
{
    public class PatrolingEnemyShooting : EnemyCharacterShooting
    {
        [SerializeField] private EnemyPatroling _patroling;

        protected override void StartShooting() =>
            StartCoroutine(ShootingWaiter());

        private IEnumerator ShootingWaiter()
        {
            yield return new WaitWhile(() => _patroling.CanShoot == false);

            base.StartShooting();
        }
    }
}
