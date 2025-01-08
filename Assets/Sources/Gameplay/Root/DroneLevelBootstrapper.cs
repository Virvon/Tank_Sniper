using Assets.Sources.Gameplay.Cameras;
using Assets.Sources.Gameplay.Player;
using Assets.Sources.Infrastructure.Factories.GameplayFactory;
using Assets.Sources.Infrastructure.Factories.TankFactory;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Gameplay.Root
{
    public class DroneLevelBootstrapper : GameplayBootstrapper
    {
        public DroneLevelBootstrapper(
            IUiFactory uiFactory,
            IGameplayFactory gameplayFactory,
            ITankFactory tankFactory,
            PlayerPoint playerPoint,
            AimingCameraPoint aimingCameraPoint,
            IStaticDataService staticDataService,
            IPersistentProgressService persistentProgressService)
            : base(uiFactory, gameplayFactory, tankFactory, playerPoint, aimingCameraPoint, staticDataService, persistentProgressService)
        {
        }

        protected override async UniTask CreateAimingVirtualCamera(IGameplayFactory gameplayFactory, Vector3 position, Quaternion rotation) =>
            await gameplayFactory.CreateRotationVirtualCamera(position, rotation);

        protected override async UniTask CreateDefeatWndow(IUiFactory uiFactory) =>
            await uiFactory.CreateDroneDefeatWindow();

        protected override async UniTask CreateGameplayWindow(IUiFactory uiFactory) =>
            await uiFactory.CreateDroneGameplayWindow();

        protected override async UniTask CreatePlayerWrapper(ITankFactory tankFactory, PlayerPoint playerPoint) =>
            await tankFactory.CreatePlayerDroneWrapper(playerPoint.transform.position, playerPoint.transform.rotation);
    }
}
