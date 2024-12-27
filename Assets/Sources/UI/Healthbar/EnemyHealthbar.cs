using Assets.Sources.Gameplay.Enemies;
using Assets.Sources.Gameplay.Player;
using Assets.Sources.Utils;
using MPUIKIT;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI
{
    public class EnemyHealthbar : MonoBehaviour
    {
        [SerializeField] private EnemyEnginery _enemyEnginery;
        [SerializeField] private float _deltaRemoveDuration;
        [SerializeField] private float _showDuration;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private MPImage _healthFill;
        [SerializeField] private MPImage _healthDeltaFill;

        private PlayerTankWrapper _playerTankWrapper;

        private Coroutine _barChanger;

        [Inject]
        private void Construct(PlayerTankWrapper playerTankWrapper)
        {
            _playerTankWrapper = playerTankWrapper;

            SetActive(false);

            _healthFill.fillAmount = 1;
            _healthDeltaFill.fillAmount = 1;

            _enemyEnginery.Damaged += OnEnemyDamaged;
        }

        private void OnDestroy() =>
            _enemyEnginery.Damaged -= OnEnemyDamaged;

        private void Update() =>
            transform.rotation = Quaternion.LookRotation((_playerTankWrapper.transform.position - transform.position).normalized);

        private void OnEnemyDamaged(uint health, uint damage)
        {
            if(health == 0)
            {
                SetActive(false);
                return;
            }
            else if (_barChanger != null)
            {
                StopCoroutine(_barChanger);
            }

            _barChanger = StartCoroutine(BarChanger(health, damage));
        }

        private void SetActive(bool isActive) =>
            _canvasGroup.alpha = isActive ? 1 : 0;

        private float RemapToFillValue(uint value) =>
            Extensions.Remap(value, 0, _enemyEnginery.MaxHealth, 0, 1);

        private IEnumerator BarChanger(uint health, uint damage)
        {
            float progress;
            float passedTime = 0;

            float healthValue = RemapToFillValue(health);
            float deltaValue = RemapToFillValue(health + damage);

            _healthFill.fillAmount = healthValue;

            SetActive(true);

            while(_healthFill.fillAmount != _healthDeltaFill.fillAmount)
            {
                progress = passedTime / _deltaRemoveDuration;
                passedTime += Time.deltaTime;

                _healthDeltaFill.fillAmount = Mathf.Lerp(deltaValue, healthValue, progress);

                yield return null;
            }

            yield return new WaitForSeconds(_showDuration);

            SetActive(false);
        }
    }
}
