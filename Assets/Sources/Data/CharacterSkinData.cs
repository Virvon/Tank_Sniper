using Assets.Sources.Types;

namespace Assets.Sources.Data
{
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
