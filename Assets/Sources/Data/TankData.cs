using Assets.Sources.Types;
using System;

namespace Assets.Sources.Data
{
    [Serializable]
    public class TankData
    {
        public uint Level;
        public bool IsUnlocked;
        public TankSkinType SkinType;

        public TankData(uint level, bool isUnlocked)
        {
            Level = level;
            IsUnlocked = isUnlocked;

            SkinType = TankSkinType.Base;
        }
    }
}
