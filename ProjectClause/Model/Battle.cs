using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectClause.Model
{
    public class Battle
    {
        public bool IsOver { get; private set; } = false;

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
            ClashFlanks(new Flank[] { Armies[0].Left, Armies[1].Left });
            ClashFlanks(new Flank[] { Armies[0].Center, Armies[1].Center });
            ClashFlanks(new Flank[] { Armies[0].Right, Armies[1].Right });
        }

        private void ClashFlanks(Flank[] flanks)
        {
            Random rand = new Random();
            for (int i = 0; i < 6; i++)
            {
                if(!flanks[0].Formation[i].Any() || !flanks[1].Formation[i].Any())
                {
                    continue;
                }

                flanks[0].Formation[i][0].DamageTaken((rand.NextDouble() + 0.5) * flanks[1].Formation[i][0].LightDamage);
                flanks[1].Formation[i][0].DamageTaken((rand.NextDouble() + 0.5) * flanks[0].Formation[i][0].LightDamage);

                if (flanks[0].Formation[i][0].Health <= 0)
                {
                    flanks[0].UnitKilled(flanks[0].Formation[i][0]);
                }
                if (flanks[1].Formation[i][0].Health <= 0)
                {
                    flanks[1].UnitKilled(flanks[1].Formation[i][0]);
                }
            }
            CheckBettleStatus();
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

    }
}
