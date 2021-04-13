using ProjectClause.Model.StaticData;
using System.Collections.Generic;

namespace ProjectClause.Model.Battles
{
    public class ArmyStatus
    {
        public string Leader { get; private set; }
        public string[] FlankLeaders { get; private set; }
        public Dictionary<string, int>[] Flanks { get; private set; }

        public ArmyStatus(Army army)
        {
            Leader = army.ArmyLeader.Name;
            FlankLeaders = new string[] { army.Left.FlankLeader.Name, army.Center.FlankLeader.Name, army.Right.FlankLeader.Name };
            Flanks = new Dictionary<string, int>[] { new Dictionary<string, int>(), new Dictionary<string, int>(), new Dictionary<string, int>() };

            for (int i = 0; i < Flanks.Length; i++)
            {
                foreach (Unit unit in army.Flanks[i].FleedUnits.Keys)
                {
                    Flanks[i].Add(unit.Name, 0);
                }
                foreach (KeyValuePair<Unit, FlankUnitsStats> unit in army.Flanks[i].ReserveUnits)
                {
                    Flanks[i][unit.Key.Name] += unit.Value.Count;
                }
                foreach (KeyValuePair<Unit, FlankUnitsStats> unit in army.Flanks[i].EngagedUnits)
                {
                    Flanks[i][unit.Key.Name] += unit.Value.Count;
                }
            }
        }

    }
}
