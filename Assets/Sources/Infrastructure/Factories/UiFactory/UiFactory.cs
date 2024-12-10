using Assets.Sources.UI;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Assets.Sources.Infrastructure.Factories.UiFactory
{
    public class UiFactory : IUiFactory
    {
        private readonly Window.Factory _windowFactory;
        private readonly DiContainer _container;

        public UiFactory(Window.Factory windowFactory, DiContainer container)
        {
            _windowFactory = windowFactory;
            _container = container;
        }

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
