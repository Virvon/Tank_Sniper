using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.UI
{
    public class OptionsWindow : OpenableWindow
    {
        [SerializeField] private Button _hideButton;

        private void OnEnable() =>
            _hideButton.onClick.AddListener(Hide);

        private void OnDisable() =>
            _hideButton.onClick.RemoveListener(Hide);
    }
}
