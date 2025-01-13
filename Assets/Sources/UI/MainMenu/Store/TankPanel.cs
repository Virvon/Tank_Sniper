using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.UI.MainMenu.Store
{
    public class TankPanel : SelectingPanelElement
    {
        [SerializeField] private CanvasGroup _fon;

        public override void Unlock()
        {
            _fon.alpha = 1;
            _fon.blocksRaycasts = true;
            _fon.interactable = true;

            Button.interactable = true;
        }
    }
}
