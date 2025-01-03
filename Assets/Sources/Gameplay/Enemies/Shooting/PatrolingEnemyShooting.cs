using Assets.Sources.Gameplay.Enemies.Movement;
using System.Collections;
using UnityEngine;

namespace Assets.Sources.Gameplay.Weapons
{
    public class PatrolingEnemyShooting : EnemyCharacterShooting
    {
        [SerializeField] private EnemyPatroling _patroling;

        protected override void StartShooting() 
        {
        }

        public void CanShooting() =>
            base.StartShooting();
    }
}
