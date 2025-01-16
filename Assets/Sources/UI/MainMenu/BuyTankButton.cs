using Assets.Sources.MainMenu.Desk;
using Assets.Sources.Services.PersistentProgress;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.MainMenu
{
    public class BuyTankButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _costValue;
        [SerializeField] private CanvasGroup _buyingMarkCanvasGroup;

        private IPersistentProgressService _persistentProgressService;
        private Desk _desk;

        private bool _deskHasEmptyCells;
        private bool _isWalletValueEnough;

        public event Action Clicked;

        [Inject]
        private void Construct(IPersistentProgressService persistentProgressService, Desk desk)
        {
            _persistentProgressService = persistentProgressService;
            _desk = desk;

            _deskHasEmptyCells = _desk.HasEmptyCells;

            UpdateCostValue();
            OnWalletValueChanged();

            _button.onClick.AddListener(OnButtonClicked);
            _persistentProgressService.Progress.TankBuyingData.CostUpdated += UpdateCostValue;
            _desk.EmploymentChanged += OnDeskEmploymentChanged;
            _persistentProgressService.Progress.Wallet.ValueChanged += OnWalletValueChanged;
        }
       
        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnButtonClicked);
            _persistentProgressService.Progress.TankBuyingData.CostUpdated -= UpdateCostValue;
            _desk.EmploymentChanged -= OnDeskEmploymentChanged;
            _persistentProgressService.Progress.Wallet.ValueChanged -= OnWalletValueChanged;
        }

        private void UpdateCostValue() =>
            _costValue.text = _persistentProgressService.Progress.TankBuyingData.CurrentCost.ToString();

        private void OnButtonClicked() =>
            Clicked?.Invoke();

        private void OnDeskEmploymentChanged(bool hasEmptyCells)
        {
            _deskHasEmptyCells = hasEmptyCells;
            ChangeInteractable();
        }

        private void OnWalletValueChanged()
        {
            _isWalletValueEnough = _persistentProgressService.Progress.Wallet.Value >= _persistentProgressService.Progress.TankBuyingData.CurrentCost;
            ChangeInteractable();
        }

        private void ChangeInteractable()
        {
            _button.interactable = _isWalletValueEnough && _deskHasEmptyCells;
            _buyingMarkCanvasGroup.alpha = _isWalletValueEnough && _deskHasEmptyCells ? 1 : 0;
        }
    }
}
