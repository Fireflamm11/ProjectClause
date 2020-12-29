using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectClause.Model
{
    public static class BattlePrint
    {
        public static void PrintBattleHeader(Battle battle)
        {
            Console.WriteLine("*******************************************");
            Console.WriteLine("                  " + battle.Armies[0].Name);
            Console.WriteLine(battle.Armies[0].Left.FlankLeader.Name + "||" + battle.Armies[0].Center.FlankLeader.Name + "||" + battle.Armies[0].Right.FlankLeader.Name);
            Console.WriteLine("*******************************************");
            Console.WriteLine(battle.Armies[1].Left.FlankLeader.Name + "||" + battle.Armies[1].Center.FlankLeader.Name + "||" + battle.Armies[1].Right.FlankLeader.Name);
            Console.WriteLine("                  " + battle.Armies[1].Name);
            Console.WriteLine("*******************************************");
        }

        public static void PrintBattleState(Battle battle)
        {
            int maxLengthTop = 0;
            foreach (List<Unit> list in battle.Armies[0].Left.Formation)
            {
                if (list.Count > maxLengthTop)
                {
                    maxLengthTop = list.Count;
                }
            }
            foreach (List<Unit> list in battle.Armies[0].Center.Formation)
            {
                if (list.Count > maxLengthTop)
                {
                    maxLengthTop = list.Count;
                }
            }
            foreach (List<Unit> list in battle.Armies[0].Right.Formation)
            {
                if (list.Count > maxLengthTop)
                {
                    maxLengthTop = list.Count;
                }
            }

            int maxLengthBottom = 0;
            foreach (List<Unit> list in battle.Armies[1].Left.Formation)
            {
                if (list.Count > maxLengthBottom)
                {
                    maxLengthBottom = list.Count;
                }
            }
            foreach (List<Unit> list in battle.Armies[1].Center.Formation)
            {
                if (list.Count > maxLengthBottom)
                {
                    maxLengthBottom = list.Count;
                }
            }
            foreach (List<Unit> list in battle.Armies[1].Right.Formation)
            {
                if (list.Count > maxLengthBottom)
                {
                    maxLengthBottom = list.Count;
                }
            }

            maxLengthTop--;
            maxLengthBottom--;

            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++");
            //Print top army
            for(int i = maxLengthTop; i > 0; i--)
            {
                PrintLine(i, battle.Armies[0]);
            }

            Console.WriteLine("-------------------------------------------");
            //Print bottom army
            for (int i = 0; i < maxLengthBottom; i++)
            {
                PrintLine(i, battle.Armies[1]);
            }

            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++");
        }

        private static void PrintLine(int index, Army army)
        {
            string line = "";

            foreach (List<Unit> list in army.Left.Formation)
            {
                try
                {
                    line += list[index].UnitSymbol;
                }
                catch (ArgumentOutOfRangeException)
                {
                    line += UnitSymbols.Dead;
                }
            }
            line += '|';
            foreach (List<Unit> list in army.Center.Formation)
            {
                try
                {
                    line += list[index].UnitSymbol;
                }
                catch (ArgumentOutOfRangeException)
                {
                    line += UnitSymbols.Dead;
                }
            }
            line += '|';
            foreach (List<Unit> list in army.Right.Formation)
            {
                try
                {
                    line += list[index].UnitSymbol;
                }
                catch (ArgumentOutOfRangeException)
                {
                    line += UnitSymbols.Dead;
                }
            }
            Console.WriteLine(line);
        }

    }
}
