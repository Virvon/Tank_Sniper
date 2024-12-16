using Assets.Sources.Services.CoroutineRunner;
using Assets.Sources.Services.InputService;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs;
using Assets.Sources.Utils;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Sources.Gameplay
{
    public class Aiming : IDisposable
    {
        private const int AimedProgress = 1;
        private const int UnaimedProgress = 0;

        private readonly IInputService _inputService;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly AimingConfig _aimingConfig;

        private Coroutine _timer;
        private float _aimingProgress;
        private bool _canAim;

        public event Action Shooted;
        public event Action<Vector2> AimShifted;
        public event Action<bool, float> StateChanged;
        public event Action<Vector2> HandlePressed;
        public event Action<bool> StateChangingFinished;

        public Aiming(IInputService inputService, ICoroutineRunner coroutineRunner, IStaticDataService staticDataService)
        {
            _inputService = inputService;
            _coroutineRunner = coroutineRunner;
            _aimingConfig = staticDataService.AimingConfig;

            _aimingProgress = 0;
            _canAim = true;

            _inputService.HandlePressed += OnHandlePressed;
            _inputService.HandleMoved += OnHandleMoved;
            _inputService.HandleMoveCompleted += OnHandleMoveCompleted;
            _inputService.AimingButtonPressed += OnAimingButtonPressed;
            _inputService.UndoAimingButtonPressed += StopAiming;
        }

        public void Dispose()
        {
            _inputService.HandlePressed -= OnHandlePressed;
            _inputService.HandleMoved -= OnHandleMoved;
            _inputService.HandleMoveCompleted -= OnHandleMoveCompleted;
            _inputService.AimingButtonPressed -= OnAimingButtonPressed;
            _inputService.UndoAimingButtonPressed -= StopAiming;
        }

        private void OnAimingButtonPressed()
        {
            if (_aimingProgress == UnaimedProgress)
                OnAiming();
        }

        private void OnHandlePressed(Vector2 handlePosition)
        {
            HandlePressed?.Invoke(handlePosition);

            if (_aimingProgress > UnaimedProgress)
                OnAiming();
        }

        private void OnHandleMoved(Vector2 handlePosition) =>
            AimShifted?.Invoke(handlePosition);

        private void OnHandleMoveCompleted()
        {
            if (_aimingProgress == AimedProgress)
            {
                TryStopTimer();
                Shooted?.Invoke();

                _timer = _coroutineRunner.StartCoroutine(Timer(_aimingConfig.ShootingAimDuration, callback: () =>
                {
                    StopAiming();
                }));
            }
            else if(_aimingProgress != UnaimedProgress)
            {
                StopAiming();
            }
        }

        private void StopAiming()
        {
            if (_aimingProgress == UnaimedProgress)
                return;

            TryStopTimer();

            _canAim = false;
            _timer = _coroutineRunner.StartCoroutine(Aimer(UnaimedProgress, callback: () => _canAim = true));
        }

        private void OnAiming()
        {
            if(_canAim)
            {
                TryStopTimer();

                _timer = _coroutineRunner.StartCoroutine(Aimer(AimedProgress));
            }
        }

        private void TryStopTimer()
        {
            if (_timer != null)
                _coroutineRunner.StopCoroutine(_timer);
        }

        private IEnumerator Timer(float duration, Action callback)
        {
            yield return new WaitForSeconds(duration);

            callback?.Invoke();
        }

        private IEnumerator Aimer(float targetProgress, Action callback = null)
        {
            if (_canAim == false && targetProgress > _aimingProgress)
                yield break;

            float progress;
            float startAimingProgress = _aimingProgress;
            float passedTime = Extensions.Remap(startAimingProgress, UnaimedProgress, AimedProgress, 0, _aimingConfig.AimingDuration);
            bool isAimed = targetProgress >= _aimingProgress;

            if (targetProgress < _aimingProgress)
                passedTime = _aimingConfig.AimingDuration - passedTime;

            StateChanged?.Invoke(isAimed, _aimingConfig.AimingDuration - passedTime);

            while(_aimingProgress != targetProgress)
            {
                progress = passedTime / _aimingConfig.AimingDuration;
                passedTime += Time.deltaTime;

                _aimingProgress = Mathf.Lerp(startAimingProgress, targetProgress, progress);

                yield return null;
            }

            callback?.Invoke();
            StateChangingFinished?.Invoke(isAimed);
        }
    }
}