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
        public string SelectedPlayerCharacterId;
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
            SelectedPlayerCharacterId = CharacterSkins.First(skin => skin.IsUnlocked).Id;
            Wallet = new();
            TankBuyingData = new(startTankBuyingCost);
            CompletedLevelsCount = 0;
            DeskData = new();
            IsSoundOn = true;
        }

        public event Action<uint> TankUnlocked;
        public event Action<uint> SelectedTankChanged;
        public event Action<string> TankSkinUnlocked;
        public event Action<string> DecalUnlocked;
        public event Action<string> DecalChanged;
        public event Action<string> CharacterSkinUnlocked;
        public event Action<string> CharacterSkinChanged;

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

        public DecalData GetDecal(string id) =>
            Decals.First(decal => decal.Id == id);

        public void UnlockDecal(string id)
        {
            GetDecal(id).IsUnlocked = true;
            DecalUnlocked?.Invoke(id);

            SelectDecal(id);
        }

        public void SelectDecal(string id)
        {
            GetTank(SelectedTankLevel).DecalId = id;
            DecalChanged?.Invoke(id);
        }

        public CharacterSkinData GetCharacterSkin(string id) =>
            CharacterSkins.First(skin => skin.Id == id);

        public void UnlockCharacterSkin(string id)
        {
            GetCharacterSkin(id).IsUnlocked = true;

            CharacterSkinUnlocked?.Invoke(id);
            SelectCharacterSkin(id);
        }

        public void SelectCharacterSkin(string id)
        {
            SelectedPlayerCharacterId = id;
            CharacterSkinChanged?.Invoke(id);
        }
    }
}
