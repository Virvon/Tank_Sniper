using System;

namespace Assets.Sources.Data
{
    [Serializable]
    public class CharacterSkinData
    {
        public string Id;
        public bool IsUnlocked;

        public CharacterSkinData(string id, bool isUnlocked)
        {
            Id = id;
            IsUnlocked = isUnlocked;
        }
    }
}
