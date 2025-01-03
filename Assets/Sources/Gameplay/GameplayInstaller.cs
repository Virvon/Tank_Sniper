using Assets.Sources.Gameplay.Cameras;
using Assets.Sources.Gameplay.Player;
using Assets.Sources.Infrastructure.Factories.BulletFactory;
using Assets.Sources.Infrastructure.Factories.GameplayFactory;
using Assets.Sources.Infrastructure.Factories.TankFactory;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay
{
    public class GameplayInstaller : MonoInstaller
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
        }

        private void BindAiming() =>
            Container.BindInterfacesAndSelfTo<Aiming>().AsSingle();

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

        private void BindGameplayBootstrapper() =>
            Container.BindInterfacesTo<GameplayBootstrapper>().AsSingle().NonLazy();
    }
}
