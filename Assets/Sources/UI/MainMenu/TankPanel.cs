using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.MainMenu
{
    public class TankPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private CanvasGroup _fon;
        [SerializeField] private Button _button;

        public event Action<TankPanel> Clicked;

        private void Start() =>
            _button.onClick.AddListener(OnButtonClicked);

        private void OnDestroy() =>
            _button.onClick.RemoveListener(OnButtonClicked);

        public void Initialize(string text) =>
            _text.text = text;

        public void Unlock()
        {
            _fon.alpha = 1;
            _fon.blocksRaycasts = true;
            _fon.interactable = true;

            _button.interactable = true;
        }

        private void OnButtonClicked() =>
            Clicked?.Invoke(this);

        public class Factory : PlaceholderFactory<string, Transform, UniTask<TankPanel>>
        {
        }
    }
}
