using Assets.Sources.UI.MainMenu;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Infrastructure.Factories.UiFactory
{
    public interface IUiFactory
    {
        UniTask<MainMenuWindow> CreateMainMenu();
        UniTask<TankPanel> CreateTankPanel(Transform parent);
        UniTask CreateWindow();
    }
}