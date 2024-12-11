using Assets.Sources.Infrastructure.Factories.MainMenuFactory;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Infrastructure.GameStateMachine;
using Assets.Sources.Infrastructure.GameStateMachine.States;
using Assets.Sources.UI.MainMenu;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Zenject;

namespace Assets.Sources.MainMenu
{
    public class MainMenuBootstrapper : IInitializable, IDisposable
    {
        private readonly IUiFactory _uiFactory;
        private readonly GameStateMachine _gameStateMachine;
        private readonly IMainMenuFactory _mainMenuFactory;

        private MainMenuWindow _mainMenuWindow;

        public MainMenuBootstrapper(IUiFactory uiFactory, GameStateMachine gameStateMachine, IMainMenuFactory mainMenuFactory)
        {
            _uiFactory = uiFactory;
            _gameStateMachine = gameStateMachine;
            _mainMenuFactory = mainMenuFactory;
        }

        public void Dispose() =>
            _mainMenuWindow.FightButtonClicked -= OnFightButtonClicked;

        public async void Initialize()
        {
            await _mainMenuFactory.CreateDesk();
            _mainMenuWindow = await _uiFactory.CreateMainMenu();

            _mainMenuWindow.FightButtonClicked += OnFightButtonClicked;
        }

        private void OnFightButtonClicked() =>
            _gameStateMachine.Enter<GameplayLoopState>().Forget();
    }
}
