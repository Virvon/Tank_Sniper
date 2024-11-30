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

        public void Dispose() =>
            _playerTank.Attacked -= OnPlayerTankAttacked;

        public UniTask Enter() =>
            default;

        public UniTask Exit()
        {
            _playerTank.Attacked -= OnPlayerTankAttacked;

            return default;
        }

        private void OnPlayerTankAttacked() =>
            _enemyStateMachine.Enter<StaticEnemyAttackState>().Forget();
    }
    public class EnemyPatrolState : IState
    {
        public UniTask Enter()
        {
            throw new NotImplementedException();
        }

        public UniTask Exit()
        {
            throw new NotImplementedException();
        }
    }
    public class EnemyMovementState : IState
    {
        public UniTask Enter()
        {
            throw new NotImplementedException();
        }

        public UniTask Exit()
        {
            throw new NotImplementedException();
        }
    }
}