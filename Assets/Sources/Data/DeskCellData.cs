using System;

namespace Assets.Sources.Data
{
    [Serializable]
    public class DeskCellData
    {
        public string Id;
        public uint TankLevel;

        public DeskCellData(string id)
        {
            Id = id;

            TankLevel = 0;
        }
    }
}
