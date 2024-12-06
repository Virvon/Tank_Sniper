using Assets.Sources.Infrastructure.Factories.BulletFactory;
using Assets.Sources.Infrastructure.Factories.GameplayFactory;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using Zenject;

namespace Assets.Sources.Gameplay
{
    public class GameplayInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindGameplayBootstrapper();
            BindUiFactory();
            BindGameplayFactory();
            BindBulletFactory();
        }

        private void BindBulletFactory() =>
            BulletFactoryInstaller.Install(Container);

        private void BindGameplayFactory() =>
            GameplayFactoryInstaller.Install(Container);

        private void BindUiFactory() =>
            UiFactoryInstaller.Install(Container);

        private void BindGameplayBootstrapper() =>
            Container.BindInterfacesTo<GameplayBootstrapper>().AsSingle().NonLazy();
    }
}
