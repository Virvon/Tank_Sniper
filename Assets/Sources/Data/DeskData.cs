using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Sources.Data
{
    [Serializable]
    public class DeskData
    {
        public List<DeskCellData> Cells;

        public DeskData()
        {
            Cells = new();
        }

        public uint GetCellInfo(string id)
        {
            DeskCellData data = Cells.FirstOrDefault(cell => cell.Id == id);

            if(data == null)
            {
                data = new DeskCellData(id);
                Cells.Add(data);
            }

            return data.TankLevel;
        }

        public void UpdateCellInfo(string id, uint tankLevel)
        {
            DeskCellData data = Cells.FirstOrDefault(cell => cell.Id == id);
            data.TankLevel = tankLevel;
        }
    }
}
