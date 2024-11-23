using Assets.Sources.Ui;
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
    }
}
