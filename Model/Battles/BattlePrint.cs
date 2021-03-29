using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectClause.Model
{
    public static class BattlePrint
    {
        public static int Round { private set; get; } = 0;
        public static void PrintBattleHeader(Battle battle)
        {
            Console.WriteLine("*******************************************");
            Console.WriteLine("                  " + battle.Armies[0].Name);
            Console.WriteLine(battle.Armies[0].Left.FlankLeader.Name + "||" + battle.Armies[0].Center.FlankLeader.Name + "||" + battle.Armies[0].Right.FlankLeader.Name);
            Console.WriteLine("*******************************************");
            Console.WriteLine(battle.Armies[1].Left.FlankLeader.Name + "||" + battle.Armies[1].Center.FlankLeader.Name + "||" + battle.Armies[1].Right.FlankLeader.Name);
            Console.WriteLine("                  " + battle.Armies[1].Name);
            Console.WriteLine("*******************************************");
            Console.WriteLine("\n");
        }

        public static void PrintBattleState(Battle battle)
        {
            Console.WriteLine("----------------------");
            Console.WriteLine("Round: " + Round);
            Console.WriteLine("----------------------");

            //Console.WriteLine(" ********************");
            //Print top army
            Console.WriteLine(battle.Armies[0].ArmyLeader.Name);
            PrintLine(battle.Armies[0], 1);
            PrintLine(battle.Armies[0], 0);

            Console.WriteLine("----------------------");
            //Print bottom army
            PrintLine(battle.Armies[1], 0);
            PrintLine(battle.Armies[1], 1);
            Console.WriteLine(battle.Armies[1].ArmyLeader.Name);

            //Console.WriteLine(" ********************");
            Console.WriteLine("\n");

            Round++;
        }

        private static void PrintLine(Army army, int lineIndex)
        {
            string line = " ";

            foreach (Flank flank in army.Flanks)
            {
                line += PrintLineFlank(flank.Battlelines[lineIndex]);
            }
            line = line.Remove(line.Length - 1);
            Console.WriteLine(line);
        }

        private static string PrintLineFlank(Unit[] units)
        {
            string line = "";
            foreach (Unit u in units)
            {
                if (u == null)
                {
                    line += " ";
                    continue;
                }

                try
                {
                    line += u.UnitSymbol;
                }
                catch (NullReferenceException)
                {
                    //There is no unit on this space, so no symbol to display
                    line += " ";
                }
                //catch (ArgumentOutOfRangeException)
                //{
                //    line += UnitSymbols.Dead;
                //}
            }
            line += "|";
            return line;
        }
    }
}
