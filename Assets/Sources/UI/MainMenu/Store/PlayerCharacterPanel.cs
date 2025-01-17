using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.UI.MainMenu.Store
{
    public class PlayerCharacterPanel : SelectingPanelElement
    {
        [SerializeField] private CanvasGroup _adButton;
        [SerializeField] private CanvasGroup _background;
        [SerializeField] private CanvasGroup _fon;

        public void RemoveBackground()
        {
            _background.alpha = 0;
            _background.blocksRaycasts = false;
            _background.interactable = false;
            _fon.alpha = 1;
            _fon.blocksRaycasts = true;
            _fon.interactable = true;
        }

        public override void Unlock()
        {
            _adButton.alpha = 0;
            _adButton.blocksRaycasts = false;
            _adButton.interactable = false;
        }
    }
}
