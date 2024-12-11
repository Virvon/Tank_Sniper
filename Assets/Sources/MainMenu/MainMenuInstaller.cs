using Assets.Sources.Infrastructure.Factories.MainMenuFactory;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using UnityEngine;
using Zenject;

namespace Assets.Sources.MainMenu
{
    public class MainMenuInstaller : MonoInstaller
    {
        [SerializeField] MainMenuCamera _camera;
         
        public override void InstallBindings()
        {
            BindMainMenuBootstrapper();
            BindUiFactory();
            BindMainMenuFactory();
            BindCamera();
            BindDeskHandler();
        }

        private void BindCamera() =>
            Container.BindInstance(_camera).AsSingle();

        private void BindDeskHandler() =>
            Container.Bind<DeskHandler>().AsSingle().NonLazy();

        private void BindMainMenuFactory() =>
            MainMenuFactoryInstaller.Install(Container);

        private void BindUiFactory() =>
            UiFactoryInstaller.Install(Container);

        private void BindMainMenuBootstrapper() =>
            Container.BindInterfacesTo<MainMenuBootstrapper>().AsSingle().NonLazy();
    }
}
