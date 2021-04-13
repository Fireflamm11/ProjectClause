using ProjectClause.Model.StaticData;
using System;
using System.Collections.Generic;

namespace ProjectClause.Model
{
    public class Flank
    {
        public int FlankIndex { get; private set; }
        public int FlankWidth { get; private set; }
        public int FlankDepth { get; private set; }
        public int OpponentIndex { get; private set; }
        public Hero FlankLeader { get; private set; }
        public List<FlankCondition> Conditions { get; private set; }
        public FlankStrategy CurrentFlankStrategy { get; private set; }
        public Dictionary<Unit, FlankUnitsStats> ReserveUnits { get; private set; }
        public Dictionary<Unit, FlankUnitsStats> EngagedUnits { get; private set; }
        public Dictionary<Unit, FlankUnitsStats> DisengagingUnits { get; private set; }
        public Dictionary<Unit, int> FleedUnits { get; private set; }

        public Flank(int index)
        {
            FlankIndex = index;
            FlankLeader = new Hero("Bodo");
            FlankWidth = 10;
            FlankDepth = 2;
            ReserveUnits = new Dictionary<Unit, FlankUnitsStats>();
            Conditions = new List<FlankCondition>();
        }

        public Flank(Hero leader, int index)
        {
            FlankIndex = index;
            FlankLeader = leader;
            FlankWidth = 10;
            FlankDepth = 2;
            ReserveUnits = new Dictionary<Unit, FlankUnitsStats>();
            Conditions = new List<FlankCondition>();
        }

        public Flank(Hero leader, int index, Dictionary<Unit, FlankUnitsStats> units, int width = 10, int depth = 2)
        {
            FlankIndex = index;
            FlankWidth = width;
            FlankDepth = depth;
            FlankLeader = leader;
            ReserveUnits = units;
            Conditions = new List<FlankCondition>();
        }

        public Flank(Hero leader, int index, Dictionary<Unit, FlankUnitsStats> units, List<FlankCondition> conditions, int width = 6, int depth = 2)
        {
            FlankIndex = index;
            FlankWidth = width;
            FlankDepth = depth;
            FlankLeader = leader;
            ReserveUnits = units;
            Conditions = conditions;
        }

        public void SetupFormation(Flank[] opponent, bool opponentEmpty = false)
        {
            EngagedUnits = new();
            DisengagingUnits = new();
            FleedUnits = new();
            foreach (Unit unit in ReserveUnits.Keys)
            {
                FleedUnits.Add(unit, 0);

                ReserveUnits[unit].Health = ReserveUnits[unit].Count * unit.Health;
                ReserveUnits[unit].Moral = ReserveUnits[unit].Count * unit.Moral;
            }

            UpdateFlankStrategy(opponent, opponentEmpty);
            OpponentIndex = FlankIndex;
        }

        public (double, double) DetermineAttack(Flank enemyFlank)
        {
            if (enemyFlank.IsEmpty()) return (0, 0);

            DisengageUnits(CurrentFlankStrategy.DisengageUnits);
            EngageUnits(CurrentFlankStrategy.EngageUnits);

            double lightdmg = 0;
            double heavydmg = 0;

            //Disengage
            foreach (KeyValuePair<Unit, FlankUnitsStats> unit in DisengagingUnits)
            {
                if (CurrentFlankStrategy.DisengageUnits.Contains(unit.Key.Type))
                {
                    lightdmg += unit.Key.LightDamage * unit.Key.Disengagement * CurrentFlankStrategy.DisengagementModificator * CurrentFlankStrategy.LightDamageModificator * unit.Value.Count;
                    heavydmg += unit.Key.HeavyDamage * unit.Key.Disengagement * CurrentFlankStrategy.DisengagementModificator * CurrentFlankStrategy.HeavyDamageModificator * unit.Value.Count;
                }
            }
            //Skirmishing
            foreach (KeyValuePair<Unit, FlankUnitsStats> unit in ReserveUnits)
            {
                if (CurrentFlankStrategy.SkirmishingUnits.Contains(unit.Key.Type))
                {
                    lightdmg += (unit.Key.LightDamage) * unit.Key.Skirmish * CurrentFlankStrategy.SkirmishModificator * CurrentFlankStrategy.LightDamageModificator * unit.Value.Count;
                    heavydmg += (unit.Key.HeavyDamage) * unit.Key.Skirmish * CurrentFlankStrategy.SkirmishModificator * CurrentFlankStrategy.HeavyDamageModificator * unit.Value.Count;
                }
            }
            //Engage
            foreach (KeyValuePair<Unit, FlankUnitsStats> unit in EngagedUnits)
            {
                if (CurrentFlankStrategy.EngageUnits.Contains(unit.Key.Type))
                {
                    lightdmg += unit.Key.LightDamage * unit.Key.Engagement * CurrentFlankStrategy.EngagementModificator * CurrentFlankStrategy.LightDamageModificator * unit.Value.Count;
                    heavydmg += unit.Key.HeavyDamage * unit.Key.Engagement * CurrentFlankStrategy.EngagementModificator * CurrentFlankStrategy.HeavyDamageModificator * unit.Value.Count;
                }
            }

            return (lightdmg, heavydmg);
        }

        public void RecieveAttack((double, double) dmg)
        {
            double sumDmg = dmg.Item1 + dmg.Item2;
            foreach (Unit unitClass in DisengagingUnits.Keys)
            {
                double dmgFaktor = (2 - unitClass.Disengagement) * (2 - CurrentFlankStrategy.DisengagementModificator) * CurrentFlankStrategy.DamageDistribution[0];
                ReceiveAttackUnit(sumDmg, dmgFaktor, unitClass, DisengagingUnits);
            }
            foreach (KeyValuePair<Unit, FlankUnitsStats> unit in ReserveUnits)
            {
                if (CurrentFlankStrategy.SkirmishingUnits.Contains(unit.Key.Type))
                {
                    double dmgFaktor = (2 - unit.Key.Skirmish) * (2 - CurrentFlankStrategy.SkirmishModificator) * CurrentFlankStrategy.DamageDistribution[2];
                    ReceiveAttackUnit(sumDmg, dmgFaktor, unit.Key, ReserveUnits);
                }
            }
            foreach (Unit unitClass in EngagedUnits.Keys)
            {
                double dmgFaktor = (2 - unitClass.Engagement) * (2 - CurrentFlankStrategy.EngagementModificator) * CurrentFlankStrategy.DamageDistribution[2];
                ReceiveAttackUnit(sumDmg, dmgFaktor, unitClass, EngagedUnits);
            }
        }

        private void ReceiveAttackUnit(double sumDmg, double dmgFaktor, Unit unitClass, Dictionary<Unit, FlankUnitsStats> UnitDict)
        {
            double dmgHalf = sumDmg / 2;
            double armDmg = dmgHalf - unitClass.Armor;
            if (armDmg < 0) armDmg = 0;
            double totalDmg = armDmg * dmgFaktor;
            double moralDmg = dmgHalf * dmgFaktor;

            UnitDict[unitClass].Health -= totalDmg;
            if (UnitDict[unitClass].Health < 0) UnitDict[unitClass].Health = 0;
            int remainingUnits = (int)(UnitDict[unitClass].Health / unitClass.Health);
            int lossedUnits = (int)(UnitDict[unitClass].Count - remainingUnits);

            UnitDict[unitClass].Moral -= moralDmg;
            if (UnitDict[unitClass].Moral < 0) UnitDict[unitClass].Moral = 0;
            remainingUnits = (int)(UnitDict[unitClass].Moral / unitClass.Moral);
            int fleeingUnits = (int)(UnitDict[unitClass].Count - remainingUnits);

            UnitDict[unitClass].Moral -= lossedUnits * (unitClass.Moral + 1) + fleeingUnits;
            if (UnitDict[unitClass].Moral < 0) UnitDict[unitClass].Moral = 0;
            UnitDict[unitClass].Health -= fleeingUnits * (unitClass.Health);
            if (UnitDict[unitClass].Health < 0) UnitDict[unitClass].Health = 0;
            UnitDict[unitClass].Count -= lossedUnits + fleeingUnits;
            FleedUnits[unitClass] += fleeingUnits;
            if (UnitDict[unitClass].Count < 0) UnitDict[unitClass].Count = 0;
        }

        private void DisengageUnits(List<UnitType> units)
        {
            foreach (KeyValuePair<Unit, FlankUnitsStats> unit in EngagedUnits)
            {
                if (units.Contains(unit.Key.Type))
                {
                    EngagedUnits.Remove(unit.Key);
                    DisengagingUnits.Add(unit.Key, unit.Value);
                }
            }
        }

        private void EngageUnits(List<UnitType> units)
        {
            foreach (KeyValuePair<Unit, FlankUnitsStats> unit in ReserveUnits)
            {
                if (units.Contains(unit.Key.Type))
                {
                    ReserveUnits.Remove(unit.Key);
                    EngagedUnits.Add(unit.Key, unit.Value);
                }
            }
        }

        public void UpdateFlankStrategy(Flank[] enemyFlanks, bool opponentEmpty)
        {
            if (opponentEmpty)
            {
                OpponentIndex = FlankLeader.FlankingTarget(enemyFlanks, FlankIndex);
            }
            bool isFlanking = false;
            if (OpponentIndex != FlankIndex) isFlanking = true;

            CurrentFlankStrategy = FlankLeader.DecideStrategy(enemyFlanks[OpponentIndex], isFlanking);
        }

        public void RoundEnd()
        {
            foreach (KeyValuePair<Unit, FlankUnitsStats> unit in DisengagingUnits)
            {
                DisengagingUnits.Remove(unit.Key);
                ReserveUnits.Add(unit.Key, unit.Value);
            }
        }

        public void UnitKilled(Unit killedUnit)
        {
            killedUnit.Die();
        }

        public bool IsEmpty()
        {
            foreach (KeyValuePair<Unit, FlankUnitsStats> unit in ReserveUnits)
            {
                if (unit.Value.Count > 0)
                {
                    return false;
                }
            }
            foreach (KeyValuePair<Unit, FlankUnitsStats> unit in EngagedUnits)
            {
                if (unit.Value.Count > 0)
                {
                    return false;
                }
            }
            foreach (KeyValuePair<Unit, FlankUnitsStats> unit in DisengagingUnits)
            {
                if (unit.Value.Count > 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
