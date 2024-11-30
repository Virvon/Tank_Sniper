using Assets.Sources.Gameplay.Enemies.Animation;
using Assets.Sources.Services.StateMachine;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Gameplay.Enemies.StateMachine.States
{
    public class StaticEnemyAttackState : IState
    {
        private readonly StaticEnemy _enemy;
        private readonly PlayerTank _playerTank;

        public StaticEnemyAttackState(StaticEnemy enemy, PlayerTank playerTank)
        {
            _enemy = enemy;
            _playerTank = playerTank;
        }

        public UniTask Enter()
        {
            _enemy.Animator.SetBool(AnimationPath.IsShooted, true);
            _enemy.Rotate(_playerTank, callback: () => _enemy.StartShooting());

            return default;
        }

        public UniTask Exit()
        {
            return default;
        }
    }
}
