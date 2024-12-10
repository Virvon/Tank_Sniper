using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.UI
{
    public class MainMenuWindow : Window
    {
        [SerializeField] private Button _fightButton;
        [SerializeField] private Button _buyTankButton;

        public event Action FightButtonClicked;
        public event Action BuyTankButtonClicked;

        private void OnEnable()
        {
            _fightButton.onClick.AddListener(OnFightButtonClicked);
            _buyTankButton.onClick.AddListener(OnBuyTankButtonClicked);
        }

        private void OnDisable()
        {
            _fightButton.onClick.RemoveListener(OnFightButtonClicked);
            _buyTankButton.onClick.RemoveListener(OnBuyTankButtonClicked);
        }

        private void OnFightButtonClicked() =>
            FightButtonClicked?.Invoke();

        private void OnBuyTankButtonClicked() =>
            BuyTankButtonClicked?.Invoke();
    }
}
