using System;

namespace Assets.Sources.Data
{
    [Serializable]
    public class PlayerProgress
    {
        public TankData[] Tanks;

        public PlayerProgress(TankData[] tanks)
        {
            Tanks = tanks;
        }
    }
}
