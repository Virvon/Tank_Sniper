using Assets.Sources.UI;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Infrastructure.Factories.UiFactory
{
    public interface IUiFactory
    {
        UniTask<MainMenuWindow> CreateMainMenu();
        UniTask CreateWindow();
    }
}