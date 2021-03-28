using ProjectClause.Model.StaticData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectClause.Model
{
    public class Starter
    {
        public static void Start()
        {
            Console.WriteLine("Generating new random battle");
            UnitLoader.LoadUnits();

            RandomBattleGenerator.GenerateRandomBattle();
        }
    }
}
