using Assets.Sources.Types;
using System;
using System.Linq;
using UnityEngine;

namespace Assets.Sources.Data
{
    [Serializable]
    public class PlayerProgress
    {
        public TankData[] Tanks;
        public uint SelectedTankLevel;
        public TankSkinData[] TankSkins;
        public DecalData[] Decals;
        public BiomeType CurrentBiomeType;
        public uint CurrentLevelIndex;
        public CharacterSkinData[] CharacterSkins;
        public PlayerCharacterType SelectedPlayerCharacter;
        public Wallet Wallet;
        public TankBuyingData TankBuyingData;
        public uint CompletedLevelsCount;
        public DeskData DeskData;
        public bool IsSoundOn;

        public PlayerProgress(
            TankData[] tanks,
            TankSkinData[] skins,
            DecalData[] decals,
            BiomeType startLevelType,
            CharacterSkinData[] characterSkins,
            uint startTankBuyingCost)
        {
            Tanks = tanks;
            TankSkins = skins;
            Decals = decals;
            CurrentBiomeType = startLevelType;
            CharacterSkins = characterSkins;

            SelectedTankLevel = Tanks.Where(tank => tank.IsUnlocked).First().Level;
            CurrentLevelIndex = 0;
            SelectedPlayerCharacter = CharacterSkins.First(skin => skin.IsUnlocked).Type;
            Wallet = new();
            TankBuyingData = new(startTankBuyingCost);
            CompletedLevelsCount = 0;
            DeskData = new();
            IsSoundOn = true;
        }

        public event Action<uint> TankUnlocked;
        public event Action<uint> SelectedTankChanged;
        public event Action<string> TankSkinUnlocked;
        public event Action<DecalType> DecalUnlocked;
        public event Action<DecalType> DecalChanged;
        public event Action<PlayerCharacterType> CharacterSkinUnlocked;
        public event Action<PlayerCharacterType> CharacterSkinChanged;

        public void TryUnlockTank(uint level)
        {
            TankData tank = Tanks.First(tank => tank.Level == level);

            if (tank.IsUnlocked == false)
            {
                tank.IsUnlocked = true;
                TankUnlocked?.Invoke(level);
                TrySelectTank(level);
            }
        }

        public void TrySelectTank(uint level)
        {
            if(GetTank(level).IsUnlocked)
            {
                SelectedTankLevel = level;
                SelectedTankChanged?.Invoke(SelectedTankLevel);
            }           
        }

        public TankData GetTank(uint level)
        {
            if (Tanks.Any(tank => tank.Level == level) == false)
            {
                Debug.LogError("Tank of this level not found");
                return null;
            }
            else
            {
                return Tanks.First(tank => tank.Level == level);
            }
        }

        public TankData GetSelectedTank() =>
            GetTank(SelectedTankLevel);

        public void UnlockTankSkin(string id)
        {
            GetSkin(id).IsUnlocked = true;
            TankSkinUnlocked?.Invoke(id);

            SelectTankSkin(id);
        }

        public TankSkinData GetSkin(string id) =>
            TankSkins.First(skin => skin.Id == id);

        public void SelectTankSkin(string id)
        {
            GetTank(SelectedTankLevel).SkinId = id;
            SelectedTankChanged?.Invoke(SelectedTankLevel);
        }

        public DecalData GetDecal(DecalType type) =>
            Decals.First(decal => decal.Type == type);

        public void UnlockDecal(DecalType type)
        {
            GetDecal(type).IsUnlocked = true;
            DecalUnlocked?.Invoke(type);

            SelectDecal(type);
        }

        public void SelectDecal(DecalType type)
        {
            GetTank(SelectedTankLevel).DecalType = type;
            DecalChanged?.Invoke(type);
        }

        public CharacterSkinData GetCharacterSkin(PlayerCharacterType type) =>
            CharacterSkins.First(skin => skin.Type == type);

        public void UnlockCharacterSkin(PlayerCharacterType type)
        {
            GetCharacterSkin(type).IsUnlocked = true;

            CharacterSkinUnlocked?.Invoke(type);
            SelectCharacterSkin(type);
        }

        public void SelectCharacterSkin(PlayerCharacterType type)
        {
            SelectedPlayerCharacter = type;
            CharacterSkinChanged?.Invoke(type);
        }
    }
}
