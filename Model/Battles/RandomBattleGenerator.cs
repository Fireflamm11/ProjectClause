using ProjectClause.Model.StaticData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectClause.Model
{
    public class RandomBattleGenerator
    {
        public static void GenerateRandomBattle()
        {
            Console.WriteLine("Starting Generation");
            Console.WriteLine("Generating Dummy Classes");
            Army[] Armies = new Army[] { GenerateRandomArmy(), GenerateRandomArmy() };
            string Terrain = "none";
            List<string> Conditions = new();
            Console.WriteLine("Generating Dummy Battle");
            Battle battle = new(Armies, Terrain, Conditions);

            Console.WriteLine("Starting Battle");
            BattlePrint.PrintBattleHeader(battle);
            BattlePrint.PrintBattleState(battle);

            while (!battle.IsOver)
            {
                battle.BattleStep();
                BattlePrint.PrintBattleState(battle);
                System.Threading.Thread.Sleep(1000);
            }
            BattlePrint.PrintBattleState(battle);
            Console.WriteLine("Battle Done");

        }

        public static Army GenerateRandomArmy()
        {
            Army army = new(GenerateRandomFlank(), GenerateRandomFlank(), GenerateRandomFlank(), " ");

            return army;
        }

        public static Flank GenerateRandomFlank()
        {
            Dictionary<Unit, int> unitList = new();
            Random rand = new();
            foreach(Unit u in UnitLoader.UnitList)
            {
                unitList.Add(u, rand.Next(3, 7));
            }

            Flank flank = new(new Hero("Bodo"), unitList);

            return flank;
        }
    }
}
