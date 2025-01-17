using UnityEngine;

namespace Assets.Sources.UI.MainMenu.Store
{
    public class TexturePanel : SelectingPanelElement
    {
        [SerializeField] private CanvasGroup _lockPanel;
        [SerializeField] private CanvasGroup _adButton;

        public override void Unlock()
        {
            _adButton.alpha = 0;
            _adButton.blocksRaycasts = false;
            _adButton.interactable = false;
            _lockPanel.alpha = 0;
        }
    }
}
