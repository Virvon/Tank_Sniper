using Assets.Sources.Data;
using Assets.Sources.Infrastructure.Factories.GameplayFactory;
using Assets.Sources.Infrastructure.Factories.TankFactory;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.Level;
using Assets.Sources.Tanks;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay
{
    public class GameplayBootstrapper : IInitializable
    {
        private readonly IUiFactory _uiFactory;
        private readonly IGameplayFactory _gameplayFactory;
        private readonly IStaticDataService _staticDataService;
        private readonly ITankFactory _tankFactory;
        private readonly IPersistentProgressService _persistentProgressService;
        private readonly PlayerPoint _playerPoint;
        private readonly AimingCameraPoint _aimingCameraPoint;

        public GameplayBootstrapper(
            IUiFactory uiFactory,
            IGameplayFactory gameplayFactory,
            IStaticDataService staticDataService,
            ITankFactory tankFactory,
            IPersistentProgressService persistentProgressService,
            PlayerPoint playerPoint,
            AimingCameraPoint aimingCameraPoint)
        {
            _uiFactory = uiFactory;
            _gameplayFactory = gameplayFactory;
            _staticDataService = staticDataService;
            _tankFactory = tankFactory;
            _persistentProgressService = persistentProgressService;
            _playerPoint = playerPoint;
            _aimingCameraPoint = aimingCameraPoint;
        }

        public async void Initialize()
        {
            LevelConfig levelConfig = _staticDataService.GetLevel("GameplayScene");
            TankData tankData = _persistentProgressService.Progress.GetSelectedTank();

            await _gameplayFactory.CreateCamera();
            await _gameplayFactory.CreateAimingVirtualCamera(_aimingCameraPoint.transform.position);
            await _gameplayFactory.CreatePlayerTank();//

            await CreateTank(tankData);

            await CreateEnemies(levelConfig);
            await _uiFactory.CreateGameplayWindow();
        }

        private async Task CreateTank(TankData tankData)
        {
            PlayerTankWrapper playerTankWrapper = await _tankFactory.CreatePlayerTankWrapper(
                tankData.Level,
                _playerPoint.transform.position,
                _playerPoint.transform.rotation);

            Tank tank = await _tankFactory.CreateTank(
                tankData.Level,
                playerTankWrapper.transform.position,
                playerTankWrapper.transform.rotation,
                playerTankWrapper.transform,
                tankData.SkinType,
                tankData.DecalType,
                false);

            playerTankWrapper.Initialize(tank.BulletPoints, tank.Turret);
        }

        private async UniTask CreateEnemies(LevelConfig levelConfig)
        {
            List<UniTask> tasks = new();

            foreach (EnemyCarPointConfig enemyCarPointConfig in levelConfig.EnemyCarPoints)
                tasks.Add(enemyCarPointConfig.Create(_gameplayFactory));
            
            foreach (HelicopterPointConfig helicopterPointConfig in levelConfig.HelicopterPoints)
                tasks.Add(helicopterPointConfig.Create(_gameplayFactory));

            await UniTask.WhenAll(tasks);
        }
    }
}
