using System;
using UnityEngine;

namespace Assets.Sources.Gameplay.Destructions
{
    public interface IDestructablePart
    {
        event Action<Vector3, uint> Destructed;
        Transform Transform { get; }
    }
}