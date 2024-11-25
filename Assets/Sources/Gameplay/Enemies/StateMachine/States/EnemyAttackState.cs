using Assets.Sources.Services.CoroutineRunner;
using Assets.Sources.Services.StateMachine;
using Cysharp.Threading.Tasks;
using System.Collections;
using UnityEngine;

namespace Assets.Sources.Gameplay.Enemies.StateMachine.States
{
    public class EnemyAttackState : IState
    {
        private const float _shootingCoolDown = 1;

        private readonly Enemy _enemy;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly PlayerTank _playerTank;

        private Coroutine _shoter;
        private bool _isShoted;

        public EnemyAttackState(Enemy enemy, ICoroutineRunner coroutineRunner, PlayerTank playerTank)
        {
            _enemy = enemy;
            _coroutineRunner = coroutineRunner;
            _playerTank = playerTank;

            _isShoted = false;
        }

        public UniTask Enter()
        {
            _shoter = _coroutineRunner.StartCoroutine(Shooter());

            return default;
        }

        public UniTask Exit()
        {
            _isShoted = false;
            return default;
        }

        private IEnumerator Shooter()
        {
            WaitForSeconds duration = new(_shootingCoolDown);
            _isShoted = true;

            while (_isShoted)
            {
                _enemy.Shoot((_playerTank.transform.position - _enemy.ShootPoint.position).normalized);

                yield return duration;
            }     
        }
    }
}
