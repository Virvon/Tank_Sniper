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
        public DecalType DecalType;

        public TankData(uint level, bool isUnlocked, DecalType decalType)
        {
            Level = level;
            IsUnlocked = isUnlocked;
            DecalType = decalType;

            SkinType = TankSkinType.Base;
        }
    }
}
