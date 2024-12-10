using System;
using UnityEngine;

namespace Assets.Sources.Services.InputService
{
    public class InputService : IDisposable, IInputService
    {
        private readonly InputActionSheme _inputActionSheme;

        public InputService()
        {
            _inputActionSheme = new();

            _inputActionSheme.Enable();

            _inputActionSheme.GameplayInput.Aiming.performed += ctx => SightShifted?.Invoke(ctx.ReadValue<Vector2>());
            _inputActionSheme.GameplayInput.Shooting.performed += ctx => Shooted?.Invoke();

            _inputActionSheme.MainMenuInput.HandleMove.started += ctx => HandlePressed?.Invoke(ctx.ReadValue<Vector2>());
            _inputActionSheme.MainMenuInput.HandleMove.performed += ctx => HandleMoved?.Invoke(ctx.ReadValue<Vector2>());
            _inputActionSheme.MainMenuInput.HandleMove.canceled += ctx => HandleMoveCompleted?.Invoke();
        }

        public event Action<Vector2> SightShifted;
        public event Action Shooted;

        public event Action<Vector2> HandlePressed;
        public event Action<Vector2> HandleMoved;
        public event Action HandleMoveCompleted;

        public void Dispose()
        {
            
        }
    }
}
