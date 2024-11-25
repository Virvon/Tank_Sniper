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

            _inputActionSheme.PlayerInput.Aiming.performed += ctx => Debug.Log(ctx.ReadValue<Vector2>());
        }

        public void Dispose()
        {
            Debug.Log("dispose input");
        }
    }
}
