using System;
using UnityEngine;

namespace Assets.Sources.Services.InputService
{
    public class InputService : IDisposable
    {
        private readonly InputActionSheme _inputActionSheme;

        public InputService()
        {
            _inputActionSheme = new();

            _inputActionSheme.Enable();

            _inputActionSheme.PlayerInput.Aiming.performed += ctx => Debug.Log(ctx);
        }

        public void Dispose()
        {
            Debug.Log("dispose input");
        }
    }
}
