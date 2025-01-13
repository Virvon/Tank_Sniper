using System;

namespace Assets.Sources.Gameplay.Player.Weapons
{
    public interface IShootable
    {
        public event Action BulletsCountChanged;

        public uint BulletsCount { get; }
    }
}
