using Assets.Sources.Gameplay.Player;
using Cinemachine;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.Cameras
{
    public class AimingCamera : MonoBehaviour
    {
        private const int EnabledPriority = 1;
        private const int DisabledPriority = 0;

        [SerializeField] private float _sensivity = 0.5f;
        [SerializeField] private CinemachineVirtualCamera _overviewCamera;
        [SerializeField] private CinemachineVirtualCamera _aimingCamera;

        private GameplayCamera _camera;
        private Aiming _aiming;

        private Vector2 _rotation;
        private CinemachineVirtualCamera _currentCamera;
        private Vector2 _lastHandlePosition;

        [Inject]
        private void Construct(GameplayCamera camera, Aiming aiming)
        {
            _camera = camera;
            _aiming = aiming;

            _rotation = new Vector2(transform.rotation.w, transform.rotation.y);

            SetCameraActive(_overviewCamera, 0);

            _aiming.AimShifted += OnAimShifted;
            _aiming.StateChanged += OnAimingStateChanged;
            _aiming.HandlePressed += OnHandlePressed;
        }

        private void OnDestroy()
        {
            _aiming.AimShifted -= OnAimShifted;
            _aiming.StateChanged -= OnAimingStateChanged;
            _aiming.HandlePressed -= OnHandlePressed;
        }

        private void OnAimingStateChanged(bool isAimed, float duration)
        {
            CinemachineVirtualCamera targetCamera = isAimed ? _aimingCamera : _overviewCamera;

            SetCameraActive(targetCamera, duration);
        }

        private void OnHandlePressed(Vector2 handlePosition) =>
            _lastHandlePosition = handlePosition;

        private void OnAimShifted(Vector2 handlePosition)
        {
            Vector2 delta = handlePosition - _lastHandlePosition;
            _lastHandlePosition = handlePosition;

            _rotation += new Vector2(-delta.y, delta.x) * _sensivity;
            transform.rotation = Quaternion.Euler(_rotation.x, _rotation.y, 0);
        }

        private void SetCameraActive(CinemachineVirtualCamera targetCamera, float duration)
        {
            _camera.SetBlednDuration(duration);

            if (_currentCamera != null)
                _currentCamera.Priority = DisabledPriority;

            _currentCamera = targetCamera;
            _currentCamera.Priority = EnabledPriority;
        }

        public class Factory : PlaceholderFactory<string, Vector3, UniTask<AimingCamera>>
        {
        }
    }
}