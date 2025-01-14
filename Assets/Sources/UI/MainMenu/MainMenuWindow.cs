using Assets.Sources.MainMenu;
using Assets.Sources.MainMenu.Desk;
using Assets.Sources.MainMenu.TanksBuying;
using Assets.Sources.UI.MainMenu.Store;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.MainMenu
{
    public class MainMenuWindow : Window
    {
        [SerializeField] private Button _fightButton;
        [SerializeField] private BuyTankButton _buyTankButton;
        [SerializeField] private Button _openStoreButton;
        [SerializeField] private Button _closeStoreButton;
        [SerializeField] private Button _optionsWindowButton;

        [SerializeField] private CanvasGroup _mainCanvasGroup;
        [SerializeField] private CanvasGroup _storeCanvasGroup;

        [SerializeField] private Canvas _canvas;

        [SerializeField] private UiSelectedTankPoint _selectedTankPoint;

        private Desk _desk;
        private OptionsWindow _optionsWindow;
        private TankBuyer _tankBuyer;

        public event Action FightButtonClicked;

        [Inject]
        public void Construct(Desk desk, MainMenuCamera mainMenuCamera, OptionsWindow optionsWindow, TankBuyer tankBuyer)
        {
            _desk = desk;
            _optionsWindow = optionsWindow;
            _tankBuyer = tankBuyer;

            _canvas.worldCamera = mainMenuCamera.UiCamera;

            _fightButton.onClick.AddListener(OnFightButtonClicked);
            _buyTankButton.Clicked += OnBuyTankButtonClicked;
            _openStoreButton.onClick.AddListener(OnOpenStoreButtonClicked);
            _closeStoreButton.onClick.AddListener(OnCloseStoreButtonClicked);
            _optionsWindowButton.onClick.AddListener(OnOptionsWindowButtonClicked);
        }

        private void OnDestroy()
        {
            _fightButton.onClick.RemoveListener(OnFightButtonClicked);
            _buyTankButton.Clicked -= OnBuyTankButtonClicked;
            _openStoreButton.onClick.RemoveListener(OnOpenStoreButtonClicked);
            _closeStoreButton.onClick.RemoveListener(OnCloseStoreButtonClicked);
            _optionsWindowButton.onClick.RemoveListener(OnOptionsWindowButtonClicked);
        }

        private void OnFightButtonClicked() =>
            FightButtonClicked?.Invoke();

        private async void OnBuyTankButtonClicked()
        {
            if(_tankBuyer.TryBuyTank(out uint level))
                await _desk.CreateTank(level);
        }

        private void OnCloseStoreButtonClicked()
        {
            SetCanvasGroupActive(_mainCanvasGroup, true);
            SetCanvasGroupActive(_storeCanvasGroup, false);

            _selectedTankPoint.Hide();
        }

        private void OnOpenStoreButtonClicked()
        {
            SetCanvasGroupActive(_mainCanvasGroup, false);
            SetCanvasGroupActive(_storeCanvasGroup, true);

            _selectedTankPoint.Show();
        }

        private void SetCanvasGroupActive(CanvasGroup canvasGroup, bool isActive)
        {
            canvasGroup.alpha = isActive ? 1 : 0;
            canvasGroup.interactable = isActive;
            canvasGroup.blocksRaycasts = isActive;
        }

        private void OnOptionsWindowButtonClicked() =>
            _optionsWindow.Show();
    }
}
