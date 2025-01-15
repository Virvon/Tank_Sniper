using Assets.Sources.Services.PersistentProgress;
using TMPro;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI
{
    public class WalletPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _value;
        private IPersistentProgressService _persistetnProgressService;

        [Inject]
        private void Construct(IPersistentProgressService persistentProgressService)
        {
            _persistetnProgressService = persistentProgressService;

            ChangeValue();

            _persistetnProgressService.Progress.Wallet.ValueChanged += ChangeValue;
        }

        private void OnDestroy() =>
            _persistetnProgressService.Progress.Wallet.ValueChanged -= ChangeValue;

        private void ChangeValue() =>
            _value.text = _persistetnProgressService.Progress.Wallet.Value.ToString();
    }
}
