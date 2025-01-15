using Assets.Sources.Data;
using Assets.Sources.Gameplay.Cameras;
using Assets.Sources.Gameplay.Player;
using Assets.Sources.Gameplay.Player.Wrappers;
using Assets.Sources.Infrastructure.Factories.GameplayFactory;
using Assets.Sources.Infrastructure.Factories.TankFactory;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Tanks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Gameplay.Root
{
    public class TankLevelBootstrapper : GameplayBootstrapper
    {
        public TankLevelBootstrapper(
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
            await gameplayFactory.CreateAimingVirtualCamera(position, rotation);

        protected async override UniTask CreateDefeatWndow(IUiFactory uiFactory) =>
            await uiFactory.CreateTankDefeatWindow();

        protected override async UniTask CreateGameplayWindow(IUiFactory uiFactory) =>
            await uiFactory.CreateTankGameplayWindow();

        protected override async UniTask CreatePlayerWrapper(ITankFactory tankFactory, PlayerPoint playerPoint)
        {
            TankData tankData = PersistentProgressService.Progress.GetSelectedTank();

            PlayerTankWrapper playerTankWrapper = await tankFactory.CreatePlayerTankWrapper(
                tankData.Level,
                playerPoint.transform.position,
                playerPoint.transform.rotation);

            Tank tank = await tankFactory.CreateTank(
                tankData.Level,
                playerTankWrapper.transform.position,
                playerTankWrapper.transform.rotation,
                playerTankWrapper.transform,
                tankData.SkinId,
                tankData.DecalType,
                false);

            playerTankWrapper.Initialize(tank.BulletPoints, tank.Turret);
        }
    }
}
