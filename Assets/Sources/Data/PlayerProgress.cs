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

        public PlayerProgress(TankData[] tanks, TankSkinData[] skins)
        {
            Tanks = tanks;
            TankSkins = skins;

            SelectedTankLevel = Tanks.Where(tank => tank.IsUnlocked).First().Level;
        }

        public event Action<uint> TankUnlocked;
        public event Action<uint> SelectedTankChanged;
        public event Action<TankSkinType> TankSkinUnlocked;

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

        public void UnlockTankSkin(TankSkinType type)
        {
            TankSkins.First(skin => skin.Type == type).IsUnlocked = true;
            TankSkinUnlocked?.Invoke(type);
        }

        public TankSkinData GetSkin(TankSkinType type) =>
            TankSkins.First(skin => skin.Type == type);

        public void SelectTankSkin(TankSkinType type)
        {
            GetTank(SelectedTankLevel).SkinType = type;
            SelectedTankChanged?.Invoke(SelectedTankLevel);
        }
    }
}
