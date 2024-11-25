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

            _inputActionSheme.PlayerInput.Aiming.performed += ctx => SightShifted?.Invoke(ctx.ReadValue<Vector2>());
            _inputActionSheme.PlayerInput.Shooting.performed += ctx => Shooted?.Invoke();
        }

        public event Action<Vector2> SightShifted;
        public event Action Shooted;

        public void Dispose()
        {
            Debug.Log("dispose input");
        }
    }
}
