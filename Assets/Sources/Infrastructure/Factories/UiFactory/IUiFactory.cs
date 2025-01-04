using Assets.Sources.UI;
using Assets.Sources.UI.MainMenu;
using Assets.Sources.UI.MainMenu.Store;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Infrastructure.Factories.UiFactory
{
    public interface IUiFactory
    {
        UniTask<MainMenuWindow> CreateMainMenu();
        UniTask<SelectingPanelElement> CreateTankPanel(Transform parent);
        UniTask<SelectingPanelElement> CreateUnlockingPanel(Transform parent);
        UniTask CreateGameplayWindow();
        UniTask CreateDefeatWindow();
    }
}