using Assets.Sources.Services.StateMachine;
using Cysharp.Threading.Tasks;
using System;

namespace Assets.Sources.Gameplay.Enemies.StateMachine.States
{
    public class EnemyWalkingState : IState, IDisposable
    {
        private readonly PlayerTank _playerTank;
        private readonly EnemyStateMachine _enemyStateMachine;
        private readonly Walking _walking;

        public EnemyWalkingState(PlayerTank playerTank, EnemyStateMachine enemyStateMachine, Walking walking)
        {
            _playerTank = playerTank;
            _enemyStateMachine = enemyStateMachine;
            _walking = walking;

            _playerTank.Attacked += OnPlayerTankAttacked;
        }

        public void Dispose() =>
            _playerTank.Attacked -= OnPlayerTankAttacked;

        public UniTask Enter()
        {
            _walking.StartWalking();

            return default;
        }

        public UniTask Exit()
        {
            _playerTank.Attacked -= OnPlayerTankAttacked;

            return default;
        }

        private void OnPlayerTankAttacked()
        {
            _walking.StopWalking();
            _enemyStateMachine.Enter<StaticEnemyAttackState>().Forget();
        }
    }
}