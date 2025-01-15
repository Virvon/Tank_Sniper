using Assets.Sources.Gameplay.Player.Aiming;
using Assets.Sources.Gameplay.Player.Weapons;
using Assets.Sources.Gameplay.Player.Wrappers;
using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI.Gameplay.GameplayWindows
{
    public class TankGameplayWindow : GameplayWindow
    {
        [SerializeField] private ReloadPanel _reloadProgressBar;

        private TankAiming _aiming;
        private PlayerTankWeapon _playerTankWeapon;

        private Coroutine _aimChanger;
        private bool _isReloaded;

        [Inject]
        private void Construct(TankAiming tankAiming, PlayerTankWeapon playerTankWeapon)
        {
            _aiming = tankAiming;
            _playerTankWeapon = playerTankWeapon;

            _isReloaded = false;

            _aiming.StateChanged += OnAimingStateChanged;
            _aiming.StateChangingFinished += OnAimingStageChangingFinished;
            _playerTankWeapon.Reloaded += OnReloaded;
        }

        protected override void OnDestroy()
        {
            _aiming.StateChanged -= OnAimingStateChanged;
            _aiming.StateChangingFinished -= OnAimingStageChangingFinished;
            _playerTankWeapon.Reloaded -= OnReloaded;
            base.OnDestroy();
        }

        private void OnAimingStateChanged(bool isAimed, float duration)
        {
            if (isAimed)
            {
                OverviewAimCanvasGroup.alpha = 0;
                SetAimButtonActive(false);
            }
            else
            {
                OverviewAimCanvasGroup.alpha = 1;
            }

            if (_aimChanger != null)
                StopCoroutine(_aimChanger);

            _aimChanger = StartCoroutine(AimChanger(isAimed, duration));
        }

        private void OnAimingStageChangingFinished(bool isAimed)
        {
            if (isAimed == false && _isReloaded == false)
                SetAimButtonActive(true);
        }

        private void OnReloaded(float duration)
        {
            SetAimButtonActive(false);
            _isReloaded = true;

            _reloadProgressBar.StartReload(duration, callback: () =>
            {
                _isReloaded = false;
                SetAimButtonActive(true);
            });
        }

        private IEnumerator AimChanger(bool isAimed, float duration)
        {
            float progress;
            float passedTime = 0;
            float targetAlpha = isAimed ? 1 : 0;
            float startAlpha = AimingCanvasGroup.alpha;

            while (AimingCanvasGroup.alpha != targetAlpha)
            {
                progress = passedTime / duration;
                passedTime += Time.deltaTime;

                AimingCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, progress);

                yield return null;
            }
        }
    }
}