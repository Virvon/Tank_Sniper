using System;
using UnityEngine;

namespace Assets.Sources.Gameplay.Player.Aiming
{
    public interface IRotationAiming
    {
        public event Action<Vector2> AimShifted;
        public event Action<Vector2> HandlePressed;
    }
}