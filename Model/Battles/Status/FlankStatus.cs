using ProjectClause.Model.StaticData;
using System.Collections.Generic;

namespace ProjectClause.Model.Battles.Status
{
    public class FlankStatus
    {
        public string FlankLeader { get; set; }
        public Dictionary<string, int> Units { get; set; }

        public FlankStatus(Flank flank)
        {
            FlankLeader = flank.FlankLeader.Name;
            Units = new Dictionary<string, int>();
            foreach (KeyValuePair<Unit, FlankUnitsStats> unit in flank.ReserveUnits)
            {
                Units.Add(unit.Key.Name, unit.Value.Count);
            }
        }
    }
}
