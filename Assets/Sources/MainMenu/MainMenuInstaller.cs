using Assets.Sources.Infrastructure.Factories.UiFactory;
using System;
using Zenject;

namespace Assets.Sources.MainMenu
{
    public class MainMenuInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindMainMenuBootstrapper();
            BindUiFactory();
        }

        private void BindUiFactory() =>
            UiFactoryInstaller.Install(Container);

        private void BindMainMenuBootstrapper() =>
            Container.BindInterfacesTo<MainMenuBootstrapper>().AsSingle().NonLazy();
    }
}
