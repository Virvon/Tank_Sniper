using Assets.Sources.Services.InputService;
using System;
using UnityEngine;

namespace Assets.Sources.MainMenu
{
    public class TankShootingHandler : IDisposable
    {
        private readonly IInputService _inputService;

        private bool _isActive;

        public event Action<Vector2> HandlePressed;

        public TankShootingHandler(IInputService inputService)
        {
            _inputService = inputService;

            _isActive = true;

            _inputService.HandlePressed += OnHandlePressed;
        }

        public void Dispose() =>
            _inputService.HandlePressed -= OnHandlePressed;

        public void SetActive(bool isActive) =>
            _isActive = isActive;

        private void OnHandlePressed(Vector2 handlePosition)
        {
            if (_isActive)
                HandlePressed?.Invoke(handlePosition);
        }
    }
}
