using System;
using UnityEngine;

namespace Assets.Sources.UI.MainMenu.Store
{
    public abstract class StorePanelTab : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private StorePanelButton _button;
        [SerializeField] private string _name;

        public event Action<StorePanelTab> Choosed;

        public string Name => _name;

        private void OnEnable() =>
            _button.Clicked += OnButtonClicked;

        private void OnDisable() =>
            _button.Clicked -= OnButtonClicked;

        public virtual void Open()
        {
            _button.SetActive(true);
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }

        public virtual void Hide()
        {
            _button.SetActive(false);
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }

        private void OnButtonClicked() =>
            Choosed?.Invoke(this);
    }
}
