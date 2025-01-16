using UnityEngine;

namespace Assets.Sources.UI.MainMenu.Store
{
    public class TexturePanel : RewardUnlockingPanel
    {
        [SerializeField] private CanvasGroup _lockPanel;

        public override void Unlock()
        {
            base.Unlock();
            _lockPanel.alpha = 0;
        }
    }
}
