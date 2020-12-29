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
            Army[] Armies = new Army[] { new Army(0), new Army(1) };
            string Terrain = "none";
            List<string> Conditions = new List<string>();
            Console.WriteLine("Generating Dummy Battle");
            Battle battle = new Battle(Armies, Terrain, Conditions);

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
    }
}
