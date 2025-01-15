using Assets.Sources.Types;
using System;

namespace Assets.Sources.Data
{
    [Serializable]
    public class TankSkinData
    {
        public string Id;
        public bool IsUnlocked;

        public TankSkinData(string id)
        {
            Id = id;

            IsUnlocked = false;
        }
    }
}
