using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.Enemies
{
    public class EnemyPatrol : EnemyMovement
    {
        [SerializeField] private float _stoppingDuration;

        //private PlayerTank _playerTank;

        protected override float StoppingDuration => _stoppingDuration; 

        [Inject]
        private void Construct()
        {
            //_playerTank = playerTank;

            //_playerTank.Attacked += StopMovement;
        }

        private void Start() =>
            StartMovement();

        //private void OnDestroy() =>
        //    _playerTank.Attacked -= StopMovement;
    }
}
