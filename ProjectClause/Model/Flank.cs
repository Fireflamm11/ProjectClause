using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectClause.Model
{
    public class Flank
    {

        public int Width { get; private set; }
        public Hero FlankLeader { get; private set; }
        public List<string> Conditions { get; private set; }
        public Dictionary<Unit, int> Units { get; private set; }
        public List<Unit>[] Formation { get; private set; }

        public Flank(string number)
        {
            FlankLeader = new Hero("Bodo" + number);
            Width = 6;
            Units = new Dictionary<Unit, int>();
            Conditions = new List<string>();

            Unit testUnit = new Unit("Dummy", UnitType.LightInfantry, new List<string>());

            Units.Add(testUnit, 36);
        }

        public Flank(string number, Hero leader)
        {
            FlankLeader = leader;
            Width = 6;
            Units = new Dictionary<Unit, int>();
            Conditions = new List<string>();

            Unit testUnit = new Unit("Dummy", UnitType.LightInfantry, new List<string>());

            Units.Add(testUnit, 36);
        }

        public Flank(Hero leader, Dictionary<Unit, int> units, int width = 6)
        {
            Width = width;
            FlankLeader = leader;
            Units = units;
            Conditions = new List<string>();
        }

        public Flank(Hero leader, Dictionary<Unit, int> units, List<string> conditions, int width = 6)
        {
            Width = width;
            FlankLeader = leader;
            Units = units;
            Conditions = conditions;
        }

        public void SetupFormation()
        {
            Formation = new List<Unit>[Width];

            for(int i = 0; i<Formation.Length;i++)
            {
                Formation[i] = new List<Unit>();
            }

            int UnitCounter = 0;
            foreach (int amount in Units.Values)
            {
                UnitCounter += amount;
            }

            int AddedUnits = 0;
            for (int i = 0; i < UnitCounter; i += AddedUnits)
            {
                AddedUnits = 0;
                for (int j = 0; j < Formation.Length; j++)
                {
                    //TODO Handle Units size 2+
                    Formation[j].Add(new Unit(Units.Keys.ToList()[0]));
                    AddedUnits++;
                }
            }

        }

        public void UnitKilled(Unit killedUnit, int formationColumn = -1)
        {
            //TODO Remove killed unit from Flank troop pool

            if (formationColumn != -1)
            {
                //Units[killedUnit];
                Formation[formationColumn].Remove(killedUnit);
            }
            else
            {
                foreach (List<Unit> column in Formation)
                {
                    if (column.Contains(killedUnit))
                    {
                        column.Remove(killedUnit);
                    }
                }
            }
        }

        public bool IsEmpty()
        {
            bool empty = true;

            foreach(List<Unit> list in Formation)
            {
                empty = empty && !list.Any();
            }

            return empty;
        }
    }
}
