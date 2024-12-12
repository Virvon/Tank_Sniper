using Assets.Sources.Types;
using System;

namespace Assets.Sources.Data
{
    [Serializable]
    public class DecalData
    {
        public DecalType Type;
        public bool IsUnlocked;

        public DecalData(DecalType type, bool isUnlocked)
        {
            Type = type;
            IsUnlocked = isUnlocked;
        }
    }
}
