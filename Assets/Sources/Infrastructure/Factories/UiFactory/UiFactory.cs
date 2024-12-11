using Assets.Sources.UI;
using Assets.Sources.UI.MainMenu;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Infrastructure.Factories.UiFactory
{
    public class UiFactory : IUiFactory
    {
        private readonly Window.Factory _windowFactory;
        private readonly DiContainer _container;
        private readonly TankPanel.Factory _tankPanelFactory;

        public UiFactory(Window.Factory windowFactory, DiContainer container, TankPanel.Factory tankPanelFactory)
        {
            _windowFactory = windowFactory;
            _container = container;
            _tankPanelFactory = tankPanelFactory;
        }

        public async UniTask<TankPanel> CreateTankPanel(Transform parent) =>
            await _tankPanelFactory.Create(UiFactoryAssets.TankPanel, parent);

        public async UniTask CreateWindow() =>
            await _windowFactory.Create(UiFactoryAssets.Window);

        public async UniTask<MainMenuWindow> CreateMainMenu()
        {
            MainMenuWindow mainMenuWindow = await _windowFactory.Create(UiFactoryAssets.MainMenuWindow) as MainMenuWindow;

            _container.BindInstance(mainMenuWindow).AsSingle();

            return mainMenuWindow;
        }
    }
}
