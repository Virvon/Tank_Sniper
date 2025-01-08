using Assets.Sources.Gameplay.Player.Aiming;
using Cinemachine;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.Cameras
{
    public class AimingCamera : RotationCamera
    {
        private const int EnabledPriority = 1;
        private const int DisabledPriority = 0;

        [SerializeField] private CinemachineVirtualCamera _overviewCamera;
        [SerializeField] private CinemachineVirtualCamera _aimingCamera;

        private GameplayCamera _camera;
        private TankAiming _aiming;

        private CinemachineVirtualCamera _currentCamera;

        [Inject]
        private void Construct(GameplayCamera camera, TankAiming aiming)
        {
            _camera = camera;
            _aiming = aiming;
            
            SetCameraActive(_overviewCamera, 0);

            _aiming.StateChanged += OnAimingStateChanged;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _aiming.StateChanged -= OnAimingStateChanged;
        }

        private void OnAimingStateChanged(bool isAimed, float duration)
        {
            CinemachineVirtualCamera targetCamera = isAimed ? _aimingCamera : _overviewCamera;

            SetCameraActive(targetCamera, duration);
        }

        private void SetCameraActive(CinemachineVirtualCamera targetCamera, float duration)
        {
            _camera.SetBlednDuration(duration);

            if (_currentCamera != null)
                _currentCamera.Priority = DisabledPriority;

            _currentCamera = targetCamera;
            _currentCamera.Priority = EnabledPriority;
        }       
    }
}