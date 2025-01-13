using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.MainMenu.Store
{
    public abstract class SelectingPanelElement : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Button _button;
        [SerializeField] private CanvasGroup _selectionFrame;

        public event Action<SelectingPanelElement> Clicked;

        public Button Button => _button;

        private void Start() =>
            _button.onClick.AddListener(OnButtonClicked);

        private void OnDestroy() =>
            _button.onClick.RemoveListener(OnButtonClicked);

        public void Initialize(string text) =>
            _text.text = text;

        public abstract void Unlock();

        private void OnButtonClicked() =>
            Clicked?.Invoke(this);

        public void SetSelectionFrameActive(bool isActive) =>
            _selectionFrame.alpha = isActive ? 1 : 0;

        public class Factory : PlaceholderFactory<string, Transform, UniTask<SelectingPanelElement>>
        {
        }
    }
}
