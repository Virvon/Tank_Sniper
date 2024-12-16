using System;
using UnityEngine;

namespace Assets.Sources.Services.InputService
{
    public interface IInputService
    {
        event Action<Vector2> HandlePressed;
        event Action<Vector2> HandleMoved;
        event Action HandleMoveCompleted;

        event Action AimingButtonPressed;
        event Action UndoAimingButtonPressed;
    }
}