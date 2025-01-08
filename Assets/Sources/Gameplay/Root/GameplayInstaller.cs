using Assets.Sources.Gameplay.Cameras;
using Assets.Sources.Gameplay.Handlers;
using Assets.Sources.Gameplay.Player;
using Assets.Sources.Infrastructure.Factories.BulletFactory;
using Assets.Sources.Infrastructure.Factories.GameplayFactory;
using Assets.Sources.Infrastructure.Factories.TankFactory;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.Root
{
    public abstract class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private PlayerPoint _playerPoint;
        [SerializeField] private AimingCameraPoint _aimingCameraPoint;

        public override void InstallBindings()
        {
            BindGameplayBootstrapper();
            BindUiFactory();
            BindGameplayFactory();
            BindBulletFactory();
            BindTankFactory();
            BindPlayerPoint();
            BindAimingCamera();
            BindAiming();
            BindDefeatHandler();
            BindWinHandler();
        }

        protected abstract void BindGameplayBootstrapper();

        protected abstract void BindAiming();

        private void BindDefeatHandler() =>
            Container.Bind<DefeatHandler>().AsSingle();

        private void BindWinHandler() =>
            Container.BindInterfacesAndSelfTo<WictoryHandler>().AsSingle();           

        private void BindAimingCamera() =>
            Container.BindInstance(_aimingCameraPoint);

        private void BindPlayerPoint() =>
            Container.BindInstance(_playerPoint).AsSingle();

        private void BindTankFactory() =>
            TankFactoryInstaller.Install(Container);

        private void BindBulletFactory() =>
            BulletFactoryInstaller.Install(Container);

        private void BindGameplayFactory() =>
            GameplayFactoryInstaller.Install(Container);

        private void BindUiFactory() =>
            UiFactoryInstaller.Install(Container);
    }
}
