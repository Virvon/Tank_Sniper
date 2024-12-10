using System;
using UnityEngine;

namespace Assets.Sources.Services.InputService
{
    public interface IInputService
    {
        public event Action<Vector2> SightShifted;
        event Action Shooted;
        event Action<Vector2> HandlePressed;
        event Action<Vector2> HandleMoved;
        event Action HandleMoveCompleted;
    }
}