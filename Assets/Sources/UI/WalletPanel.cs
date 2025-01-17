using Assets.Sources.Services.PersistentProgress;
using System.Collections;
using TMPro;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI
{
    public class WalletPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _value;

        private IPersistentProgressService _persistetnProgressService;

        private bool _isAnimated;

        [Inject]
        private void Construct(IPersistentProgressService persistentProgressService)
        {
            _persistetnProgressService = persistentProgressService;

            _isAnimated = false;

            ChangeValue();

            _persistetnProgressService.Progress.Wallet.ValueChanged += ChangeValue;
        }

        private void OnDestroy() =>
            _persistetnProgressService.Progress.Wallet.ValueChanged -= ChangeValue;

        private void ChangeValue()
        {
            if(_isAnimated == false)
                _value.text = _persistetnProgressService.Progress.Wallet.Value.ToString();
        }

        public void CreditReward(uint reward, float duration) =>
            StartCoroutine(Animator(_persistetnProgressService.Progress.Wallet.Value + reward, duration));

        private IEnumerator Animator(uint targetValue, float duration)
        {
            float progress;
            float passedTime = 0;
            uint startValue = _persistetnProgressService.Progress.Wallet.Value;

            _isAnimated = true;

            while (_isAnimated)
            {
                progress = passedTime / duration;
                passedTime += Time.deltaTime;

                int value = (int)Mathf.Lerp(startValue, targetValue, progress);
                _value.text = value.ToString();

                if (value == targetValue)
                    _isAnimated = false;

                yield return null;
            }
        }
    }
}
