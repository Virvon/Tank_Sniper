using Assets.Sources.Infrastructure.Factories.GameplayFactory;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.Level;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using Zenject;

namespace Assets.Sources.Gameplay
{
    public class GameplayBootstrapper : IInitializable
    {
        private readonly IUiFactory _uiFactory;
        private readonly IGameplayFactory _gameplayFactory;
        private readonly IStaticDataService _staticDataService;

        public GameplayBootstrapper(IUiFactory uiFactory, IGameplayFactory gameplayFactory, IStaticDataService staticDataService)
        {
            _uiFactory = uiFactory;
            _gameplayFactory = gameplayFactory;
            _staticDataService = staticDataService;
        }

        public async void Initialize()
        {
            await _gameplayFactory.CreatePlayerTank();
            await CreateEnemies();
            await _gameplayFactory.CreateCamera();
            await _uiFactory.CreateWindow();
        }

        private async UniTask CreateEnemies()
        {
            LevelConfig levelConfig = _staticDataService.GetLevel("GameplayScene");

            List<UniTask> tasks = new();

            foreach(EnemyPointConfig enemyPointConfig in levelConfig.EnemyPoints)
                tasks.Add(_gameplayFactory.CreateEnemy(enemyPointConfig.EnemyType, enemyPointConfig.Position, enemyPointConfig.Rotation));

            await UniTask.WhenAll(tasks);
        }
    }
}
