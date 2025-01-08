using Assets.Sources.Gameplay.Player.Aiming;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay
{
    public abstract class HandleableRotationObject : MonoBehaviour
    {
        [SerializeField] private float _sensivity = 0.5f;

        private IRotationAiming _rotationAiming;

        private Vector2 _lastHandlePosition;

        public Vector2 Rotation { get; protected set; }

        [Inject]
        protected void Construct(IRotationAiming rotationAiming)
        {
            _rotationAiming = rotationAiming;

            _rotationAiming.HandlePressed += OnHandlePressed;
            _rotationAiming.AimShifted += OnAimShifted;
        }

        protected virtual void OnDestroy()
        {
            _rotationAiming.HandlePressed -= OnHandlePressed;
            _rotationAiming.AimShifted -= OnAimShifted;
        }

        protected virtual void OnHandlePressed(Vector2 handlePosition) =>
            _lastHandlePosition = handlePosition;

        protected virtual void OnAimShifted(Vector2 handlePosition)
        {
            Vector2 delta = handlePosition - _lastHandlePosition;
            _lastHandlePosition = handlePosition;

            Rotation += new Vector2(-delta.y, delta.x) * _sensivity;
            Rotation = ClampRotation(Rotation);

            Rotate();
        }

        protected abstract Vector2 ClampRotation(Vector2 rotation);

        protected void Rotate() =>
            transform.rotation = Quaternion.Euler(Rotation.x, Rotation.y, 0);
    }
}