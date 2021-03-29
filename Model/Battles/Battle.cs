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

            for (int x = 0; x < lines - 1; x++)
                for (int i = 0; i < flankwidth; i++)
                {
                    if (flanks[0].Battlelines[x] == null || flanks[1].Battlelines[x] == null)
                    {
                        //TODO flanking of whole Flanks
                        continue;
                    }

                    //Each field must be checked, wheather it is empty(no unit on it) before dealing dmg
                    Unit defender1 = new(), defender2 = new();
                    if (flanks[0].Battlelines[x][i] != null)
                    {
                        defender1 = GetDefenderIndex(flanks[1], 0, i, flanks[0].Battlelines[x][i].Flanking);
                    }
                    if (flanks[1].Battlelines[x][i] != null)
                    {
                        defender2 = GetDefenderIndex(flanks[0], 0, i, flanks[1].Battlelines[x][i].Flanking);
                    }

                    if (defender1 != null && flanks[0].Battlelines[x][i] != null)
                    {
                        DealDmg(flanks[0].Battlelines[x][i], defender1);
                    }
                    if (defender2 != null && flanks[1].Battlelines[x][i] != null)
                    {
                        DealDmg(flanks[1].Battlelines[x][i], defender2);
                    }

                    //Similar to above we must check for null in case of a dead unity before trying to kill it
                    if ((flanks[0].Battlelines[x][i] != null) && flanks[0].Battlelines[x][i].Health <= 0)
                    {
                        RoundDeaths++;
                        flanks[0].UnitKilled(flanks[0].Battlelines[x][i], i);
                    }
                    if ((flanks[1].Battlelines[x][i] != null) && flanks[1].Battlelines[x][i].Health <= 0)
                    {
                        RoundDeaths++;
                        flanks[1].UnitKilled(flanks[1].Battlelines[x][i], i);
                    }
                }

            CheckBettleStatus();
            return RoundDeaths;
        }

        private Unit GetDefenderIndex(Flank defendingFlank, int battleLineIndexAttacker, int unitIndexAttacker, int flankingAttaker)
        {
            if (defendingFlank.Battlelines[battleLineIndexAttacker][unitIndexAttacker] != null)
            {
                return defendingFlank.Battlelines[battleLineIndexAttacker][unitIndexAttacker];
            }
            else
            {
                //Flanking
                //Decide on a random base, whether to attack left or right
                Random rand = new();
                int LeftOrRight = rand.Next(0, 2);
                int flankingCounter = 0;

                if (LeftOrRight == 0)
                {
                    //Attack left == index-1
                    for (int i = unitIndexAttacker - 1; i >= 0; i--)
                    {
                        if (defendingFlank.Battlelines[battleLineIndexAttacker][i] != null)
                        {
                            return defendingFlank.Battlelines[battleLineIndexAttacker][i];
                        }
                        flankingCounter++;
                        if (flankingCounter >= flankingAttaker) { break; }
                    }
                }
                else
                {
                    //Attack right == index+1
                    for (int i = unitIndexAttacker + 1; i < defendingFlank.Battlelines[battleLineIndexAttacker].Length; i++)
                    {
                        if (defendingFlank.Battlelines[battleLineIndexAttacker][unitIndexAttacker] != null)
                        {
                            return defendingFlank.Battlelines[battleLineIndexAttacker][i];
                        }
                        flankingCounter++;
                        if (flankingCounter >= flankingAttaker) { break; }
                    }
                }
            }
            //throw new Exception("Couldn't find unit to flank " + unitIndexAttacker);
            return null;
        }

        private void DealDmg(Unit attacker, Unit defender)
        {
            Random rand = new();
            _ = defender.DamageTaken((rand.NextDouble() + 0.5) * attacker.LightDamage);
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
