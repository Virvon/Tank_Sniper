using Assets.Sources.Services.StateMachine;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Assets.Sources.Gameplay.Enemies.StateMachine.States
{
    public class EnemyIdleState : IState, IDisposable
    {
        private readonly PlayerTank _playerTank;
        private readonly EnemyStateMachine _enemyStateMachine;

        public EnemyIdleState(PlayerTank playerTank, EnemyStateMachine enemyStateMachine)
        {
            _playerTank = playerTank;
            _enemyStateMachine = enemyStateMachine;

            _playerTank.Attacked += OnPlayerTankAttacked;
        }

        public void Dispose()
        {
            _playerTank.Attacked -= OnPlayerTankAttacked;

            Debug.Log("dispose enemy idle state");
        }

        public UniTask Enter() => default;

        public UniTask Exit()
        {
            _playerTank.Attacked -= OnPlayerTankAttacked;

            return default;
        }

        private void OnPlayerTankAttacked() =>
            _enemyStateMachine.Enter<EnemyAttackState>().Forget();
    }
}