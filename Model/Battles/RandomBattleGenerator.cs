using ProjectClause.Model.Battles;
using ProjectClause.Model.StaticData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectClause.Model
{
    public class RandomBattleGenerator
    {
        public static Battle Battle { get; private set; }

        public static void GenerateRandomBattle()
        {
            Console.WriteLine("Starting Generation");
            Console.WriteLine("Generating Dummy Classes");
            Army[] Armies = new Army[] { GenerateRandomArmy(), GenerateRandomArmy() };
            string Terrain = "none";
            List<string> Conditions = new();
            Console.WriteLine("Generating Dummy Battle");
            Battle = new(Armies, Terrain, Conditions);

            Console.WriteLine("Starting Battle");
            BattlePrint.PrintBattleHeader(Battle);
            BattlePrint.PrintBattleState(Battle);

            //while (!battle.IsOver)
            //{
            //    battle.BattleStep();
            //    BattlePrint.PrintBattleState(battle);
            //    System.Threading.Thread.Sleep(1000);
            //}
            //BattlePrint.PrintBattleState(battle);
            //Console.WriteLine("Battle Done");

        }

        public static BattleStatus NextRound()
        {
            Battle.BattleStep();
            BattlePrint.PrintBattleState(Battle);
            return GetCurrentStatus();
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
            foreach (Unit u in UnitLoader.UnitList)
            {
                unitList.Add(u, rand.Next(3, 7));
            }

            Flank flank = new(new Hero("Bodo"), unitList);

            return flank;
        }

        public static BattleStatus GetCurrentStatus()
        {
            List<char[]>[] army1 = { new List<char[]>(), new List<char[]>(), new List<char[]>() };
            List<char[]>[] army2 = { new List<char[]>(), new List<char[]>(), new List<char[]>() };

            for (int i = 0; i < 3; i++)
            {
                Flank flank1 = Battle.Armies[0].Flanks[i];
                Flank flank2 = Battle.Armies[1].Flanks[i];

                foreach (Unit[] line in flank1.Battlelines)
                {
                    char[] units = new char[line.Length];
                    int counter = 0;
                    foreach (Unit u in line)
                    {
                        if (u != null)
                        {
                            units[counter] = u.UnitSymbol;
                        }
                        else
                        {
                            units[counter] = '_';
                        }
                        counter++;
                    }
                    army1[i].Add(units);
                }

                foreach (Unit[] line in flank2.Battlelines)
                {
                    char[] units = new char[line.Length];
                    int counter = 0;
                    foreach (Unit u in line)
                    {
                        if (u != null)
                        {
                            units[counter] = u.UnitSymbol;
                        }
                        else
                        {
                            units[counter] = '_';
                        }
                        counter++;
                    }
                    army2[i].Add(units);
                }
            }

            return new BattleStatus(army1, army2);
        }

    }
}
