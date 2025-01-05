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
        public BiomeType CurrentLevelType;
        public uint CurrentLevelIndex;

        public PlayerProgress(TankData[] tanks, TankSkinData[] skins, DecalData[] decals, BiomeType startLevelType)
        {
            Tanks = tanks;
            TankSkins = skins;
            Decals = decals;
            CurrentLevelType = startLevelType;

            SelectedTankLevel = Tanks.Where(tank => tank.IsUnlocked).First().Level;
            CurrentLevelIndex = 0;
        }

        public event Action<uint> TankUnlocked;
        public event Action<uint> SelectedTankChanged;
        public event Action<TankSkinType> TankSkinUnlocked;
        public event Action<DecalType> DecalUnlocked;
        public event Action<DecalType> DecalChanged;

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

        public void UnlockTankSkin(TankSkinType type)
        {
            GetSkin(type).IsUnlocked = true;
            TankSkinUnlocked?.Invoke(type);

            SelectTankSkin(type);
        }

        public TankSkinData GetSkin(TankSkinType type) =>
            TankSkins.First(skin => skin.Type == type);

        public void SelectTankSkin(TankSkinType type)
        {
            GetTank(SelectedTankLevel).SkinType = type;
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
    }
}
