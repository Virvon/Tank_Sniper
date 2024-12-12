using Assets.Sources.Types;
using System;

namespace Assets.Sources.Data
{
    [Serializable]
    public class TankSkinData
    {
        public TankSkinType Type;
        public bool IsUnlocked;

        public TankSkinData(TankSkinType type)
        {
            Type = type;

            IsUnlocked = false;
        }
    }
}
