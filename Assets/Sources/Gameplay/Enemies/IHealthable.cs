using System;

namespace Assets.Sources.Gameplay.Enemies
{
    public interface IHealthable
    {
        public event Action<uint, uint> Damaged;
        public uint MaxHealth { get; }
    }
}
