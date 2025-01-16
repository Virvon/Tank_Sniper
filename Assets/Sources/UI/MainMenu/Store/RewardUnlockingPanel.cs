using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.UI.MainMenu.Store
{
    public class RewardUnlockingPanel : SelectingPanelElement
    {
        [SerializeField] private CanvasGroup _adButton;

        public override void Unlock()
        {
            _adButton.alpha = 0;
            _adButton.blocksRaycasts = false;
            _adButton.interactable = false;
        }
    }
}
