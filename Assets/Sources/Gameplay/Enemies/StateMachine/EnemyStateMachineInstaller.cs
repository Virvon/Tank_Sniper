using Assets.Sources.Services.StateMachine;
using Zenject;

namespace Assets.Sources.Gameplay.Enemies.StateMachine
{
    public class EnemyStateMachineInstaller : Installer<EnemyStateMachineInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<StatesFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemyStateMachine>().AsSingle();
        }
    }
}
