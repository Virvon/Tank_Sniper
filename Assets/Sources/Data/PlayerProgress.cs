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

        public PlayerProgress(TankData[] tanks)
        {
            Tanks = tanks;

            SelectedTankLevel = Tanks.Where(tank => tank.IsUnlocked).First().Level;
        }

        public event Action<uint> TankUnlocked;
        public event Action<uint> SelectedTankChanged;

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
            if (Tanks.Any(tank => tank.Level == level) == false)
            {
                Debug.LogError("Tank of this level not found");
                return;
            }
            else if(Tanks.First(tank => tank.Level == level).IsUnlocked == false)
            {
                return;
            }

            SelectedTankLevel = level;
            SelectedTankChanged?.Invoke(SelectedTankLevel);
        }
    }
}
