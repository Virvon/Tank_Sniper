using Assets.Sources.Types;
using System;

namespace Assets.Sources.Data
{
    [Serializable]
    public class CharacterSkinData
    {
        public PlayerCharacterType Type;
        public bool IsUnlocked;

        public CharacterSkinData(PlayerCharacterType type, bool isUnlocked)
        {
            Type = type;
            IsUnlocked = isUnlocked;
        }
    }
}
