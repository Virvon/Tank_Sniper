using Assets.Sources.Gameplay.Enemies.StateMachine;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.Enemies
{
    public class EnemyInstaller : MonoInstaller
    {
        [SerializeField] private Enemy _enemy;

        public override void InstallBindings()
        {
            BindEnemyBootstrapper();
            BindEnemyStateMachine();
            BindEnemy();
        }

        private void BindEnemy() =>
            Container.BindInstance(_enemy).AsSingle();

        private void BindEnemyStateMachine() =>
            EnemyStateMachineInstaller.Install(Container);

        private void BindEnemyBootstrapper() =>
            Container.BindInterfacesTo<EnemyBootstrapper>().AsSingle().NonLazy();
    }
}
