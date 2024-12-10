using Assets.Sources.MainMenu;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI
{
    public class MainMenuWindow : Window
    {
        [SerializeField] private Button _fightButton;
        [SerializeField] private Button _buyTankButton;

        private Desk _desk;

        public event Action FightButtonClicked;

        [Inject]
        public void Construct(Desk desk)
        {
            _desk = desk;

            _desk.EmploymentChanged += OnDeskEmploymentChanged;
            _fightButton.onClick.AddListener(OnFightButtonClicked);
            _buyTankButton.onClick.AddListener(OnBuyTankButtonClicked);
        }

        private void OnDestroy()
        {
            _desk.EmploymentChanged -= OnDeskEmploymentChanged;
            _fightButton.onClick.RemoveListener(OnFightButtonClicked);
            _buyTankButton.onClick.RemoveListener(OnBuyTankButtonClicked);
        }

        private void OnDeskEmploymentChanged(bool hasEmptyCells) =>
            _buyTankButton.interactable = hasEmptyCells;

        private void OnFightButtonClicked() =>
            FightButtonClicked?.Invoke();

        private async void OnBuyTankButtonClicked() =>
            await _desk.CreateTank();
    }
}
