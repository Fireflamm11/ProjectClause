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
            Random rand = new();

            for (int i = 0; i < flankwidth; i++)
            {
                if (flanks[0].Frontline == null || flanks[1].Frontline == null)
                {
                    //TODO flanking
                    continue;
                }

                //TODO Asymetrical Flanks
                if (flanks[0].Frontline[i] != null)
                {
                    if (flanks[1].Frontline[i] != null)
                    {
                        _ = flanks[0].Frontline[i].DamageTaken((rand.NextDouble() + 0.5) * flanks[1].Frontline[i].LightDamage);
                    }
                }
                if (flanks[1].Frontline[i] != null)
                {
                    if (flanks[0].Frontline[i] != null)
                    {
                        _ = flanks[1].Frontline[i].DamageTaken((rand.NextDouble() + 0.5) * flanks[0].Frontline[i].LightDamage);
                    }
                }

                if ((flanks[0].Frontline[i] != null) && flanks[0].Frontline[i].Health <= 0)
                {
                    RoundDeaths++;
                    flanks[0].UnitKilled(flanks[0].Frontline[i], i);
                }
                if ((flanks[1].Frontline[i] != null) && flanks[1].Frontline[i].Health <= 0)
                {
                    RoundDeaths++;
                    flanks[1].UnitKilled(flanks[1].Frontline[i], i);
                }
            }

            CheckBettleStatus();
            return RoundDeaths;
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
