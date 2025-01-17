using Assets.Sources.Gameplay.Player;
using Assets.Sources.Gameplay.Player.Aiming;
using Assets.Sources.Gameplay.Player.Wrappers;
using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI.Gameplay.GameplayWindows
{
    public class DroneGameplayWindow : GameplayWindow
    {
        [SerializeField] private CanvasGroup _enemiesCounterCanvasGroup;
        [SerializeField] private CanvasGroup _fade;
        [SerializeField] private float _fadeDuration;

        private PlayerDroneWrapper _playerDroneWrapper;
        private DroneAiming _droneAiming;
        private CameraNoise _cameraNoise;

        [Inject]
        private void Construct(PlayerDroneWrapper playerDroneWrapper, DroneAiming droneAiming, CameraNoise cameraNoise)
        {
            _playerDroneWrapper = playerDroneWrapper;
            _droneAiming = droneAiming;
            _cameraNoise = cameraNoise;

            _playerDroneWrapper.DroneExploided += OnDroneExploided;
            _droneAiming.Shooted += OnShooted;
        }

        protected override void OnDestroy()
        {
            _playerDroneWrapper.DroneExploided -= OnDroneExploided;
            _droneAiming.Shooted -= OnShooted;
        }

        private void OnShooted()
        {
            SetAimButtonActive(false);
            OverviewAimCanvasGroup.alpha = 0;

            StartCoroutine(Fader(1, _fadeDuration, _fadeDuration, callback: () =>
            {
                AimingCanvasGroup.alpha = 1;
                _enemiesCounterCanvasGroup.alpha = 0;
                _cameraNoise.SetActive(true);

                StartCoroutine(Fader(0, _fadeDuration));
            }));
        }

        private void OnDroneExploided()
        {
            SetAimButtonActive(true);
            OverviewAimCanvasGroup.alpha = 1;
            AimingCanvasGroup.alpha = 0;
            _enemiesCounterCanvasGroup.alpha = 1;
            _cameraNoise.SetActive(false);
        }

        private IEnumerator Fader(float targetAlpha, float duration, float waitingTime = 0, Action callback = null)
        {
            float progress;
            float passedTime = 0;
            float startAlpha = _fade.alpha;

            while(_fade.alpha != targetAlpha)
            {
                progress = passedTime / duration;
                passedTime += Time.deltaTime;

                _fade.alpha = Mathf.Lerp(startAlpha, targetAlpha, progress);

                yield return null;
            }

            yield return new WaitForSeconds(waitingTime);

            callback?.Invoke();
        }
    }
}