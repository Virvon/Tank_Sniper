using Assets.Sources.Types;
using Assets.Sources.UI;
using Assets.Sources.UI.Gameplay;
using Assets.Sources.UI.Gameplay.Aim;
using Assets.Sources.UI.Gameplay.BulletsPanel;
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
        UniTask CreateTankGameplayWindow();
        UniTask CreateTankDefeatWindow();
        UniTask CreateWictroyWindow(WictoryWindowType type);
        UniTask<GameplayLoadingCurtain> CreateLoadingCurtain();
        UniTask CreateDroneGameplayWindow();
        UniTask CreateDroneDefeatWindow();
        UniTask<SelectingPanelElement> CreateCharacterSkinPanel(Transform parent);
        UniTask CreateOptionsWindow();
        UniTask<ProgressBarElement> CreateProgressBarElement(Transform parent);
        UniTask<BulletIcon> CreateTankBulletIcon(Transform parent);
        UniTask<SuperBulletIcon> CreateSuperBulletIcon(Transform parent);
        UniTask<BulletIcon> CreateDroneBulletIcon(Transform parent);
        UniTask CreateRestartWindow();
        UniTask<SelectingPanelElement> CreateDecalPanel(Transform parent);
    }
}