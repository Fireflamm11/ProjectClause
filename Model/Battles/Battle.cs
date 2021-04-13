using ProjectClause.Model.StaticData;
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
            Armies[0].Left.SetupFormation(Armies[1].Flanks);
            Armies[0].Center.SetupFormation(Armies[1].Flanks);
            Armies[0].Right.SetupFormation(Armies[1].Flanks);

            Armies[1].Left.SetupFormation(Armies[0].Flanks);
            Armies[1].Center.SetupFormation(Armies[0].Flanks);
            Armies[1].Right.SetupFormation(Armies[0].Flanks);
        }

        public void BattleStep()
        {
            int RoundDeaths = 0;
            RoundDeaths += ClashFlanks(new Flank[] { Armies[0].Left, Armies[1].Left });
            RoundDeaths += ClashFlanks(new Flank[] { Armies[0].Center, Armies[1].Center });
            RoundDeaths += ClashFlanks(new Flank[] { Armies[0].Right, Armies[1].Right });

            RoundEnd(RoundDeaths);
        }

        private int ClashFlanks(Flank[] flanks)
        {
            int RoundDeaths = 0;

            (double, double) attackFlank1 = (0, 0);
            (double, double) attackFlank2 = (0, 0);
            if (!flanks[0].IsEmpty())
            {
                flanks[0].UpdateFlankStrategy(Armies[1].Flanks, Armies[1].Flanks[flanks[0].OpponentIndex].IsEmpty());
                attackFlank1 = flanks[0].DetermineAttack(Armies[1].Flanks[flanks[0].OpponentIndex]);
            }

            if (!flanks[1].IsEmpty())
            {
                flanks[1].UpdateFlankStrategy(Armies[0].Flanks, Armies[0].Flanks[flanks[1].OpponentIndex].IsEmpty());
                attackFlank2 = flanks[1].DetermineAttack(Armies[0].Flanks[flanks[1].OpponentIndex]);
            }

            if (attackFlank1 != (0, 0))
            {
                Armies[1].Flanks[flanks[0].OpponentIndex].RecieveAttack(attackFlank1);
            }

            if (attackFlank2 != (0, 0))
            {
                Armies[0].Flanks[flanks[1].OpponentIndex].RecieveAttack(attackFlank2);
            }

            if (CheckBettleStatus() != -1) IsOver = false;
            return RoundDeaths;
        }

        private int CheckBettleStatus()
        {
            if (Armies[0].IsBeaten())
            {
                return 0;
            }
            else if (Armies[1].IsBeaten())
            {
                return 1;
            }
            return -1;
        }

        public void RoundEnd(int deaths)
        {
            Console.WriteLine("deaths this round:" + deaths);
            Armies[0].RoundEnd();
            Armies[1].RoundEnd();
        }
    }
}
