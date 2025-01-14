using Assets.Sources.Infrastructure.Factories.BulletFactory;
using Assets.Sources.Infrastructure.Factories.MainMenuFactory;
using Assets.Sources.Infrastructure.Factories.TankFactory;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.MainMenu.Desk;
using Assets.Sources.MainMenu.TanksBuying;
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
            BindBulletFactory();
            BindTankFactoryInstaller();
            BindTankBuyer();
        }

        private void BindTankBuyer() =>
            Container.Bind<TankBuyer>().AsSingle();

        private void BindTankFactoryInstaller() =>
            TankFactoryInstaller.Install(Container);

        private void BindBulletFactory() =>
            BulletFactoryInstaller.Install(Container);

        private void BindCamera() =>
            Container.BindInstance(_camera).AsSingle();

        private void BindDeskHandler() =>
            Container.BindInterfacesTo<DeskHandler>().AsSingle().NonLazy();

        private void BindMainMenuFactory() =>
            MainMenuFactoryInstaller.Install(Container);

        private void BindUiFactory() =>
            UiFactoryInstaller.Install(Container);

        private void BindMainMenuBootstrapper() =>
            Container.BindInterfacesTo<MainMenuBootstrapper>().AsSingle().NonLazy();
    }
}
