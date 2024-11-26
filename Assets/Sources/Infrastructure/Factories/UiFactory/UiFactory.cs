using Assets.Sources.UI;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Infrastructure.Factories.UiFactory
{
    public class UiFactory : IUiFactory
    {
        private readonly Window.Factory _windowFactory;

        public UiFactory(Window.Factory windowFactory)
        {
            _windowFactory = windowFactory;
        }

        public async UniTask CreateWindow() =>
            await _windowFactory.Create(UiFactoryAssets.Window);

        public async UniTask<MainMenuWindow> CreateMainMenu()
        {
            return await _windowFactory.Create(UiFactoryAssets.MainMenuWindow) as MainMenuWindow;
        }
    }
}
