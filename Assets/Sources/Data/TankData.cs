using System;

namespace Assets.Sources.Data
{
    [Serializable]
    public class TankData
    {
        public uint Level;
        public bool IsUnlocked;

        public TankData(uint level, bool isUnlocked)
        {
            Level = level;
            IsUnlocked = isUnlocked;
        }
    }
}
