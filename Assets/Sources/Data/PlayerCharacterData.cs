using System;

namespace Assets.Sources.Data
{
    [Serializable]
    public class PlayerCharacterData
    {
        public string Id;
        public bool IsUnlocked;

        public PlayerCharacterData(string id, bool isUnlocked)
        {
            Id = id;
            IsUnlocked = isUnlocked;
        }
    }
}
