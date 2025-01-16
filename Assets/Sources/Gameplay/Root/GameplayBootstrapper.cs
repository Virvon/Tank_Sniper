using Assets.Sources.Gameplay.Cameras;
using Assets.Sources.Gameplay.Player;
using Assets.Sources.Infrastructure.Factories.GameplayFactory;
using Assets.Sources.Infrastructure.Factories.TankFactory;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.Level.EnemyPoints;
using Assets.Sources.Types;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.Root
{
    public abstract class GameplayBootstrapper : IInitializable
    {
        private readonly IUiFactory _uiFactory;
        private readonly IGameplayFactory _gameplayFactory;
        private readonly ITankFactory _tankFactory;
        private readonly AimingCameraPoint _aimingCameraPoint;
        private readonly IStaticDataService _staticDataService;
        private readonly PlayerPoint _playerPoint;

        protected readonly IPersistentProgressService PersistentProgressService;

        public GameplayBootstrapper(
            IUiFactory uiFactory,
            IGameplayFactory gameplayFactory,
            ITankFactory tankFactory,
            PlayerPoint playerPoint,
            AimingCameraPoint aimingCameraPoint,
            IStaticDataService staticDataService,
            IPersistentProgressService persistentProgressService)
        {
            _uiFactory = uiFactory;
            _gameplayFactory = gameplayFactory;
            _tankFactory = tankFactory;
            _playerPoint = playerPoint;
            _aimingCameraPoint = aimingCameraPoint;
            _staticDataService = staticDataService;
            PersistentProgressService = persistentProgressService;
        }

        public async void Initialize()
        {
            uint levelIndex = PersistentProgressService.Progress.CurrentLevelIndex;
            BiomeType biomeType = PersistentProgressService.Progress.CurrentBiomeType;
            LevelConfig levelConfig = _staticDataService.GetLevel(_staticDataService.GetLevelsSequence(biomeType).GetLevel(levelIndex));

            await CreateCamera(_gameplayFactory);
            await CreateAimingVirtualCamera(_gameplayFactory, _aimingCameraPoint.transform.position, _aimingCameraPoint.transform.rotation);

            await CreatePlayerWrapper(_tankFactory, _playerPoint);
            await CreateEnemies(levelConfig);
            await _uiFactory.CreateRestartWindow();
            await _uiFactory.CreateOptionsWindow();
            await CreateGameplayWindow(_uiFactory);
            await CreateDefeatWndow(_uiFactory);
            await _uiFactory.CreateLoadingCurtain();
            await _uiFactory.CreateWictroyWindow();
        }

        protected virtual async UniTask<GameplayCamera> CreateCamera(IGameplayFactory gameplayFactory) =>
            await gameplayFactory.CreateCamera();

        protected abstract UniTask CreateDefeatWndow(IUiFactory uiFactory);

        protected abstract UniTask CreateAimingVirtualCamera(IGameplayFactory gameplayFactory, Vector3 position, Quaternion rotation);

        protected abstract UniTask CreatePlayerWrapper(ITankFactory tankFactory, PlayerPoint playerPoint);

        protected abstract UniTask CreateGameplayWindow(IUiFactory uiFactory);

        private async UniTask CreateEnemies(LevelConfig levelConfig)
        {
            List<UniTask> tasks = new();

            foreach (HelicopterPointConfig helicopterPointConfig in levelConfig.HelicopterPoints)
                tasks.Add(helicopterPointConfig.Create(_gameplayFactory));

            foreach (EnemyMovementEngineryPointConfig enemyCarPointConfig in levelConfig.MovementEngineryPoints)
                tasks.Add(enemyCarPointConfig.Create(_gameplayFactory));

            foreach (PatrolingEnemyPointConfig patrolingEnemyPointConfig in levelConfig.PatrolingEnemyPoints)
                tasks.Add(patrolingEnemyPointConfig.Create(_gameplayFactory));

            foreach (StaticEnemyPointConfig staticEnemyPointConfig in levelConfig.StaticEnemyPoints)
                tasks.Add(staticEnemyPointConfig.Create(_gameplayFactory));

            await UniTask.WhenAll(tasks);
        }
    }
}
