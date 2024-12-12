using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.UI.MainMenu.Store
{
    public class StorePanelButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Color _activeColor;
        [SerializeField] private Color _deactiveColor;
        [SerializeField] private Image _fon;
        [SerializeField] private CanvasGroup _background;

        public event Action Clicked;

        private void OnEnable() =>
            _button.onClick.AddListener(OnButtonClicked);

        private void OnDisable() =>
            _button.onClick.RemoveListener(OnButtonClicked);

        private void OnButtonClicked() =>
            Clicked?.Invoke();

        public void SetActive(bool isActive)
        {
            _fon.color = isActive ? _activeColor : _deactiveColor;
            _background.alpha = isActive ? 1 : 0;
        }
    }
}
