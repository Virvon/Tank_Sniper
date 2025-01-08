using Assets.Sources.Gameplay.Player.Aiming;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI.Gameplay.GameplayWindows
{
    public class TankGameplayWindow : GameplayWindow
    {
        private TankAiming _aiming;

        private Coroutine _aimChanger;

        [Inject]
        private void Construct(TankAiming tankAiming)
        {
            _aiming = tankAiming;

            _aiming.StateChanged += OnAimingStateChanged;
            _aiming.StateChangingFinished += OnAimingStageChangingFinished;
        }

        protected override void OnDestroy()
        {
            _aiming.StateChanged -= OnAimingStateChanged;
            _aiming.StateChangingFinished -= OnAimingStageChangingFinished;
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
            if (isAimed == false)
                SetAimButtonActive(true);
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