using Assets.Sources.Infrastructure.Factories.GameplayFactory;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using System;
using Zenject;

namespace Assets.Sources.Gameplay
{
    public class GameplayBootstrapper : IInitializable
    {
        private readonly IUiFactory _uiFactory;
        private readonly IGameplayFactory _gameplayFactory;

        public GameplayBootstrapper(IUiFactory uiFactory, IGameplayFactory gameplayFactory)
        {
            _uiFactory = uiFactory;
            _gameplayFactory = gameplayFactory;
        }

        public async void Initialize()
        {
            await _gameplayFactory.CreatePlayerTank();
            await _gameplayFactory.CreateEnemy();
            await _gameplayFactory.CreateCamera();
            await _uiFactory.CreateWindow();
        }
    }
}
