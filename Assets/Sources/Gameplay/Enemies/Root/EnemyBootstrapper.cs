using Assets.Sources.Gameplay.Enemies.StateMachine;
using Assets.Sources.Gameplay.Enemies.StateMachine.States;
using Assets.Sources.Services.StateMachine;
using Assets.Sources.Services.StaticDataService.Configs.Level;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Assets.Sources.Gameplay.Enemies.Root
{
    public class EnemyBootstrapper : IInitializable
    {
        private readonly EnemyStateMachine _enemyStateMachine;
        private readonly StatesFactory _statesFactory;
        private readonly EnemyPointConfig _enemyPointConfig;

        public EnemyBootstrapper(EnemyStateMachine enemyStateMachine, StatesFactory statesFactory, EnemyPointConfig enemyPointConfig)
        {
            _enemyStateMachine = enemyStateMachine;
            _statesFactory = statesFactory;
            _enemyPointConfig = enemyPointConfig;
        }

        public void Initialize()
        {
            //_enemyStateMachine.RegisterState(_statesFactory.Create<EnemyIdleState>());
            //_enemyStateMachine.RegisterState(_statesFactory.Create<StaticEnemyAttackState>());

            //_enemyStateMachine.Enter<EnemyIdleState>().Forget();

            //if(_enemyPointConfig.IsStatic)
            //{
            //    _enemyStateMachine.RegisterState(_statesFactory.Create<EnemyIdleState>());
            //}
            //else
            //{
            //    if (_enemyPointConfig.IsWaitedAttackOnStartPoint)
            //    {
            //        _enemyStateMachine.RegisterState(_statesFactory.Create<EnemyIdleState>());
            //    }
                
            //    //if(_enemyPointConfig.IsMovedToPath)
            //}
        }
    }
}
