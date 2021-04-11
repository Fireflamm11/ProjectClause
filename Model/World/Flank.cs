using ProjectClause.Model.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectClause.Model
{
    public class Flank
    {

        public int FlankWidth { get; private set; }
        public Hero FlankLeader { get; private set; }
        public List<string> Conditions { get; private set; }
        public Dictionary<Unit, int> ReserveUnits { get; private set; }
        //public Unit[] Frontline { get; private set; }
        //public Unit[] Backline { get; private set; }
        public List<Unit[]> Battlelines { get; private set; } = new List<Unit[]>();

        public Flank()
        {
            FlankLeader = new Hero("Bodo");
            FlankWidth = 6;
            ReserveUnits = new Dictionary<Unit, int>();
            Conditions = new List<string>();
        }

        public Flank(Hero leader)
        {
            FlankLeader = leader;
            FlankWidth = 6;
            ReserveUnits = new Dictionary<Unit, int>();
            Conditions = new List<string>();
        }

        public Flank(Hero leader, Dictionary<Unit, int> units, int width = 6)
        {
            FlankWidth = width;
            FlankLeader = leader;
            ReserveUnits = units;
            Conditions = new List<string>();
        }

        public Flank(Hero leader, Dictionary<Unit, int> units, List<string> conditions, int width = 6)
        {
            FlankWidth = width;
            FlankLeader = leader;
            ReserveUnits = units;
            Conditions = conditions;
        }

        public void SetupFormation()
        {
            Battlelines.Add(new Unit[FlankWidth]); //Frontline
            Battlelines.Add(new Unit[FlankWidth]); //Backline

            //TODO actual setup
            for (int i = 0; i < Battlelines[0].Length; i++)
            {
                Battlelines[0][i] = new Unit(ReserveUnits.Keys.First());
                ReserveUnits[ReserveUnits.Keys.First()]--;
            }

            for (int i = 0; i < Battlelines[1].Length; i++)
            {
                Battlelines[1][i] = new Unit(ReserveUnits.Keys.ToList()[2]);
                ReserveUnits[ReserveUnits.Keys.ToList()[2]]--;
            }

        }

        public void RefillBattlelines(Unit[] battleline)
        {
            for (int i = 0; i < battleline.Length; i++)
            {
                if (battleline[i] == null)
                {
                    try
                    {
                        battleline[i] = GetUnitFromReseve();
                    }
                    catch (EmptyReserveException)
                    {
                        throw;
                    }
                }
            }
        }

        public void MoveUpEmptyRanks()
        {
            for (int line = Battlelines.Count - 1; line > 0; line--)
                for (int unitIndex = 0; unitIndex < FlankWidth; unitIndex++)
                {
                    if (Battlelines[line - 1][unitIndex] == null)
                    {
                        Battlelines[line - 1][unitIndex] = Battlelines[line][unitIndex];
                        Battlelines[line][unitIndex] = null;
                    }
                }
        }

        private Unit GetUnitFromReseve()
        {
            foreach (Unit u in ReserveUnits.Keys)
            {
                if (ReserveUnits[u] > 0)
                {
                    ReserveUnits[u]--;
                    return new Unit(u);
                }
            }

            throw new EmptyReserveException();
        }

        public void RoundEnd()
        {
            try
            {
                RefillBattlelines(Battlelines[0]);
            }
            catch (EmptyReserveException)
            {

            }

            MoveUpEmptyRanks();
        }

        public void UnitKilled(Unit killedUnit, int formationColumn = -1)
        {
            //TODO Remove killed unit from Flank troop pool
            int index = -1;
            if (formationColumn != -1)
            {
                index = formationColumn;
            }
            else
            {
                for (int x = 0; x < Battlelines[0].Length; x++)
                {
                    if (Battlelines[0][x] == killedUnit)
                    {
                        index = x;
                    }
                }
            }
            KillUnit(killedUnit, index);
        }

        private void KillUnit(Unit unit, int index)
        {
            Battlelines[0][index] = null;
            unit.Die();
        }

        public bool IsEmpty()
        {
            foreach (Unit[] line in Battlelines)
            {
                if (line != null) { return false; }
            }

            foreach (int x in ReserveUnits.Values)
            {
                if (x != 0) { return false; }
            }

            return true;
        }
    }
}
