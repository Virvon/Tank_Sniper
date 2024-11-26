using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.UI
{
    public class MainMenuWindow : Window
    {
        [SerializeField] private Button _fightButton;

        public event Action FightButtonClicked;

        private void OnEnable() =>
            _fightButton.onClick.AddListener(OnFightButtonClicked);

        private void OnDisable() =>
            _fightButton.onClick.RemoveListener(OnFightButtonClicked);

        private void OnFightButtonClicked() =>
            FightButtonClicked?.Invoke();
    }
}
