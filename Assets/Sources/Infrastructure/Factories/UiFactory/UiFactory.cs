using Assets.Sources.UI;
using Assets.Sources.UI.Gameplay;
using Assets.Sources.UI.Gameplay.GameplayWindows;
using Assets.Sources.UI.MainMenu;
using Assets.Sources.UI.MainMenu.Store;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Infrastructure.Factories.UiFactory
{
    public class UiFactory : IUiFactory
    {
        private readonly Window.Factory _windowFactory;
        private readonly DiContainer _container;
        private readonly SelectingPanelElement.Factory _selectingPanelElementFactory;

        public UiFactory(Window.Factory windowFactory, DiContainer container, SelectingPanelElement.Factory selectingPanelElementFactory)
        {
            _windowFactory = windowFactory;
            _container = container;
            _selectingPanelElementFactory = selectingPanelElementFactory;
        }

        public async UniTask CreateLoadingCurtain()
        {
            LoadingCurtain loadingCurtain = await _windowFactory.Create(UiFactoryAssets.LoadingCurtain) as LoadingCurtain;
            _container.BindInstance(loadingCurtain).AsSingle();
        }

        public async UniTask CreateWictroyWindow() =>
            await _windowFactory.Create(UiFactoryAssets.WictoryWindow);

        public async UniTask CreateTankDefeatWindow() =>
            await _windowFactory.Create(UiFactoryAssets.TankDefeatWindow);

        public async UniTask CreateDroneDefeatWindow() =>
            await _windowFactory.Create(UiFactoryAssets.DroneDefeatWindow);

        public async UniTask<SelectingPanelElement> CreateUnlockingPanel(Transform parent) =>
            await _selectingPanelElementFactory.Create(UiFactoryAssets.UnlockingPanel, parent);

        public async UniTask<SelectingPanelElement> CreateTankPanel(Transform parent) =>
            await _selectingPanelElementFactory.Create(UiFactoryAssets.TankPanel, parent);

        public async UniTask CreateTankGameplayWindow() =>
            await CreateGameplayWindow(UiFactoryAssets.TankGameplayWindow);

        public async UniTask CreateDroneGameplayWindow() =>
            await CreateGameplayWindow(UiFactoryAssets.DroneGameplayWindow);

        public async UniTask<MainMenuWindow> CreateMainMenu()
        {
            MainMenuWindow mainMenuWindow = await _windowFactory.Create(UiFactoryAssets.MainMenuWindow) as MainMenuWindow;

            _container.BindInstance(mainMenuWindow).AsSingle();

            return mainMenuWindow;
        }

        private async UniTask CreateGameplayWindow(string assetPath)
        {
            GameplayWindow gameplayWindow = await _windowFactory.Create(assetPath) as GameplayWindow;

            _container.BindInstance(gameplayWindow).AsSingle();
        }
    }
}
