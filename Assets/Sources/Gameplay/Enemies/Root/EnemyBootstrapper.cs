using Assets.Sources.Gameplay.Enemies.StateMachine;
using Assets.Sources.Gameplay.Enemies.StateMachine.States;
using Assets.Sources.Services.StateMachine;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Assets.Sources.Gameplay.Enemies.Root
{
    public class EnemyBootstrapper : IInitializable
    {
        private readonly EnemyStateMachine _enemyStateMachine;
        private readonly StatesFactory _statesFactory;

        public EnemyBootstrapper(EnemyStateMachine enemyStateMachine, StatesFactory statesFactory)
        {
            _enemyStateMachine = enemyStateMachine;
            _statesFactory = statesFactory;
        }

        public void Initialize()
        {
            _enemyStateMachine.RegisterState(_statesFactory.Create<EnemyIdleState>());
            _enemyStateMachine.RegisterState(_statesFactory.Create<EnemyAttackState>());

            _enemyStateMachine.Enter<EnemyIdleState>().Forget();
        }
    }
}
