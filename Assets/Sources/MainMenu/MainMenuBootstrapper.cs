using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Infrastructure.GameStateMachine;
using Assets.Sources.Infrastructure.GameStateMachine.States;
using Assets.Sources.UI;
using Cysharp.Threading.Tasks;
using System;
using Zenject;

namespace Assets.Sources.MainMenu
{
    public class MainMenuBootstrapper : IInitializable, IDisposable
    {
        private readonly IUiFactory _uiFactory;
        private readonly GameStateMachine _gameStateMachine;

        private MainMenuWindow _mainMenuWindow;

        public MainMenuBootstrapper(IUiFactory uiFactory, GameStateMachine gameStateMachine)
        {
            _uiFactory = uiFactory;
            _gameStateMachine = gameStateMachine;
        }

        public void Dispose() =>
            _mainMenuWindow.FightButtonClicked -= OnFightButtonClicked;

        public async void Initialize()
        {
            _mainMenuWindow = await _uiFactory.CreateMainMenu();

            _mainMenuWindow.FightButtonClicked += OnFightButtonClicked;
        }

        private void OnFightButtonClicked() =>
            _gameStateMachine.Enter<GameplayLoopState>().Forget();
    }
}
