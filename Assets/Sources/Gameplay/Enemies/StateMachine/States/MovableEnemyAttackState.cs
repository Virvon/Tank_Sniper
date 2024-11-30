using Assets.Sources.Gameplay.Enemies.Animation;
using Assets.Sources.Services.StateMachine;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Gameplay.Enemies.StateMachine.States
{
    public class MovableEnemyAttackState : IState
    {
        private readonly Car _car;

        public MovableEnemyAttackState(Car car)
        {
            _car = car;
        }

        public UniTask Enter()
        {
            _car.Animator.SetBool(AnimationPath.IsShooted, true);
            _car.StartShooting();

            return default;
        }

        public UniTask Exit()
        {
            return default;
        }
    }
}
