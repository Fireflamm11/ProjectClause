using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectClause.Model
{
    public class Battle
    {
        public bool IsOver { get; private set; } = false;
        public int[] Battlewidth { get; private set; } = { 6, 6, 6 };

        public Army[] Armies { get; private set; }
        public string Terrain { get; private set; }
        public List<string> Conditions { get; private set; }

        public List<string> BattleLog { get; private set; }

        public Battle(Army[] armies, string terrain, List<string> conditions)
        {
            Console.WriteLine("Setting Battle Dummy Values");

            Armies = armies;
            Terrain = terrain;
            Conditions = conditions;

            Console.WriteLine("Setting Formation");
            SetupFormations();

            Battlewidth[0] = armies[0].Left.FlankWidth;
            Battlewidth[1] = armies[0].Center.FlankWidth;
            Battlewidth[2] = armies[0].Right.FlankWidth;
            if (Battlewidth[0] < armies[1].Left.FlankWidth)
            {
                Battlewidth[0] = armies[1].Left.FlankWidth;
            }
            if (Battlewidth[0] < armies[1].Center.FlankWidth)
            {
                Battlewidth[0] = armies[1].Center.FlankWidth;
            }
            if (Battlewidth[0] < armies[1].Right.FlankWidth)
            {
                Battlewidth[0] = armies[1].Right.FlankWidth;
            }

            Console.WriteLine("Battle Generation Done");
        }

        public void SetupFormations()
        {
            foreach (Army armie in Armies)
            {
                armie.Left.SetupFormation();
                armie.Center.SetupFormation();
                armie.Right.SetupFormation();
            }
            Console.WriteLine("Formation Setup Done");
        }

        public void BattleStep()
        {
            int RoundDeaths = 0;
            RoundDeaths += ClashFlanks(new Flank[] { Armies[0].Left, Armies[1].Left }, Battlewidth[0]);
            RoundDeaths += ClashFlanks(new Flank[] { Armies[0].Center, Armies[1].Center }, Battlewidth[1]);
            RoundDeaths += ClashFlanks(new Flank[] { Armies[0].Right, Armies[1].Right }, Battlewidth[2]);

            RoundEnd(RoundDeaths);
        }

        private int ClashFlanks(Flank[] flanks, int flankwidth)
        {
            int RoundDeaths = 0;
            int lines = flanks[0].Battlelines.Count;
            if (lines < flanks[1].Battlelines.Count) { lines = flanks[1].Battlelines.Count; }


            RoundDeaths += ResolveMelee(flanks, flankwidth);
            for (int line = 1; line < lines; line++)
            {
                if (flanks[0].Battlelines[line] == null && flanks[1].Battlelines[line] == null)
                {
                    //Both Flank lines are empty, so they can be ignored
                    continue;
                }

                if (flanks[0].Battlelines[line] == null || flanks[1].Battlelines[line] == null)
                {
                    //TODO flanking of whole Flanks
                    continue;
                }

                RoundDeaths += ResolveRanged(flanks, line, flankwidth);
            }

            CheckBettleStatus();
            return RoundDeaths;
        }

        private int ResolveMelee(Flank[] flanks, int flankwidth)
        {
            if (flanks[0].Battlelines[0] == null && flanks[1].Battlelines[0] == null)
            {
                //Both Flank lines are empty, so they can be ignored
                return 0;
            }

            if (flanks[0].Battlelines[0] == null || flanks[1].Battlelines[0] == null)
            {
                //TODO flanking of whole Flanks
                return 0;
            }

            int RoundDeaths = 0;
            for (int unitLineIndex = 0; unitLineIndex < flankwidth; unitLineIndex++)
            {
                //Each field must be checked, wheather it is empty(no unit on it) before dealing dmg
                Unit defenderArmy1 = new(), defenderArmy2 = new();
                if (flanks[0].Battlelines[0][unitLineIndex] != null)
                {
                    defenderArmy1 = GetDefenderHorizontal(flanks[1], 0, unitLineIndex, flanks[0].Battlelines[0][unitLineIndex].Flanking);
                }
                if (flanks[1].Battlelines[0][unitLineIndex] != null)
                {
                    defenderArmy2 = GetDefenderHorizontal(flanks[0], 0, unitLineIndex, flanks[1].Battlelines[0][unitLineIndex].Flanking);
                }

                if (defenderArmy1 != null && flanks[0].Battlelines[0][unitLineIndex] != null)
                {
                    if (DealDmg(flanks[0].Battlelines[0][unitLineIndex], defenderArmy1))
                    {
                        RoundDeaths++;
                        flanks[0].UnitKilled(flanks[0].Battlelines[0][unitLineIndex], unitLineIndex);
                    }
                }
                if (defenderArmy2 != null && flanks[1].Battlelines[0][unitLineIndex] != null)
                {
                    if (DealDmg(flanks[1].Battlelines[0][unitLineIndex], defenderArmy2))
                    {
                        RoundDeaths++;
                        flanks[1].UnitKilled(flanks[1].Battlelines[0][unitLineIndex], unitLineIndex);
                    }
                }
            }
            return RoundDeaths;
        }

        private int ResolveRanged(Flank[] flanks, int line, int flankwidth)
        {
            int RoundDeaths = 0;
            for (int unitLineIndex = 0; unitLineIndex < flankwidth; unitLineIndex++)
            {
                //Each field must be checked, wheather it is empty(no unit on it) before dealing dmg
                Unit defenderUnitArmy1 = new(), defenderUnitArmy2 = new();
                if (flanks[0].Battlelines[line][unitLineIndex] != null)
                {
                    defenderUnitArmy1 = GetDefenderIndexVerticalAndHorizontal(flanks[1], line, unitLineIndex, flanks[0].Battlelines[line][unitLineIndex].Flanking);
                }
                if (flanks[1].Battlelines[line][unitLineIndex] != null)
                {
                    defenderUnitArmy2 = GetDefenderIndexVerticalAndHorizontal(flanks[0], line, unitLineIndex, flanks[1].Battlelines[line][unitLineIndex].Flanking);
                }

                if (defenderUnitArmy1 != null && flanks[0].Battlelines[line][unitLineIndex] != null)
                {
                    if (DealDmg(flanks[0].Battlelines[line][unitLineIndex], defenderUnitArmy1, false))
                    {
                        RoundDeaths++;
                        flanks[0].UnitKilled(flanks[line].Battlelines[line][unitLineIndex], unitLineIndex);
                    }
                }
                if (defenderUnitArmy2 != null && flanks[1].Battlelines[line][unitLineIndex] != null)
                {
                    if (DealDmg(flanks[1].Battlelines[line][unitLineIndex], defenderUnitArmy2, false))
                    {
                        RoundDeaths++;
                        flanks[1].UnitKilled(flanks[1].Battlelines[line][unitLineIndex], unitLineIndex);
                    }
                }
            }
            return RoundDeaths;
        }

        private Unit GetDefenderHorizontal(Flank defendingFlank, int battleLineIndexAttacker, int unitIndexAttacker, int flankingAttaker)
        {
            if (defendingFlank.Battlelines[battleLineIndexAttacker][unitIndexAttacker] != null)
            {
                return defendingFlank.Battlelines[battleLineIndexAttacker][unitIndexAttacker];
            }
            else
            {
                int flankingCounter = 0;

                //Alternate between left and right, starting with right side enemies
                for (int i = 1; i <= flankingAttaker; i++)
                {
                    //Check if there is an enemy to the right
                    if (defendingFlank.Battlelines[battleLineIndexAttacker][unitIndexAttacker + 1] != null)
                    {
                        return defendingFlank.Battlelines[battleLineIndexAttacker][unitIndexAttacker + 1];
                    }
                    //Check if there is an enemy to the left
                    if (defendingFlank.Battlelines[battleLineIndexAttacker][unitIndexAttacker - 1] != null)
                    {
                        return defendingFlank.Battlelines[battleLineIndexAttacker][unitIndexAttacker - 1];
                    }

                    if (flankingCounter >= flankingAttaker) { break; }
                    flankingCounter++;
                }
            }
            //throw new Exception("Couldn't find unit to flank " + unitIndexAttacker);
            return null;
        }

        private Unit GetDefenderIndexVerticalAndHorizontal(Flank defendingFlank, int lineIndexAttacker, int unitIndexAttacker, int flankingAttaker)
        {
            for (int lineIndex = lineIndexAttacker; lineIndex >= 0; lineIndex--)
            {
                if (defendingFlank.Battlelines[lineIndex][unitIndexAttacker] != null)
                {
                    return defendingFlank.Battlelines[lineIndex][unitIndexAttacker];
                }
                else
                {
                    Unit horizontalDefender = GetDefenderHorizontal(defendingFlank, lineIndex, unitIndexAttacker, flankingAttaker);
                    if (horizontalDefender != null)
                    {
                        return horizontalDefender;
                    }
                }
            }
            return null;
        }

        private bool DealDmg(Unit attacker, Unit defender, bool melee = true)
        {
            double dmg = 0;
            if (melee)
            {
                dmg += attacker.LightMelee-defender.Armor+attacker.HeavyMelee;
            }
            else
            {
                dmg += attacker.LightRanged - defender.Armor + attacker.HeavyRanged;
            }
            double health = defender.DamageTaken(dmg);
            if (health <= 0) return false;
            return true;
        }

        private bool CheckBettleStatus()
        {
            foreach (Army army in Armies)
            {
                if (army.Left.IsEmpty() && army.Center.IsEmpty() && army.Right.IsEmpty())
                {
                    IsOver = true;
                    return IsOver;
                }
            }
            return IsOver;
        }

        public void RoundEnd(int deaths)
        {
            Console.WriteLine("deaths this round:" + deaths);
            Armies[0].RoundEnd();
            Armies[1].RoundEnd();
        }
    }
}
