using Assets.Sources.Services.StaticDataService;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.MainMenu.Store
{
    public abstract class SelectingPanelElement : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private CanvasGroup _selectionFrame;
        [SerializeField] private Image _icon;

        public event Action<SelectingPanelElement> Clicked;

        public Button Button => _button;

        protected IStaticDataService StaticDataService { get; private set; }

        [Inject]
        private void Construct(IStaticDataService staticDataService)
        {
            StaticDataService = staticDataService;

            _button.onClick.AddListener(OnButtonClicked);
        }

        private void OnDestroy() =>
            _button.onClick.RemoveListener(OnButtonClicked);

        public void Initialize(Sprite sprite) =>
            _icon.sprite = sprite;

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
