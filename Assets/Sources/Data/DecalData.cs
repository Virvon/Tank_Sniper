using Assets.Sources.Types;
using System;

namespace Assets.Sources.Data
{
    [Serializable]
    public class DecalData
    {
        public string Id;
        public bool IsUnlocked;

        public DecalData(string id, bool isUnlocked)
        {
            Id = id;
            IsUnlocked = isUnlocked;
        }
    }
}
