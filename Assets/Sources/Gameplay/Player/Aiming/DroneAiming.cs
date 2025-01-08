using Assets.Sources.Services.InputService;
using System;
using UnityEngine;

namespace Assets.Sources.Gameplay.Player.Aiming
{
    public class DroneAiming : IDisposable, IRotationAiming, IShootedAiming
    {
        private readonly IInputService _inputService;

        public event Action Shooted;
        public event Action<Vector2> AimShifted;
        public event Action<Vector2> HandlePressed;

        protected DroneAiming(IInputService inputService)
        {
            _inputService = inputService;

            _inputService.HandlePressed += OnHandlePressed;
            _inputService.HandleMoved += OnHandleMoved;
            _inputService.AimingButtonPressed += OnAimingButtonPressed;
        }

        public void Dispose()
        {
            _inputService.HandlePressed -= OnHandlePressed;
            _inputService.HandleMoved -= OnHandleMoved;
            _inputService.AimingButtonPressed -= OnAimingButtonPressed;
        }

        private void OnAimingButtonPressed() =>
            Shooted?.Invoke();

        private void OnHandleMoved(Vector2 handlePosition) =>
            AimShifted?.Invoke(handlePosition);

        private void OnHandlePressed(Vector2 handlePosition) =>
            HandlePressed?.Invoke(handlePosition);
    }
}