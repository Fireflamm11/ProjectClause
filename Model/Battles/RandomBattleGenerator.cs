using ProjectClause.Model.Battles;
using ProjectClause.Model.StaticData;
using System;
using System.Collections.Generic;

namespace ProjectClause.Model
{
    public class RandomBattleGenerator
    {
        public static List<Battle> Battles { get; private set; } = new();

        public static void GenerateRandomBattle()
        {
            Console.WriteLine("Starting Generation");
            Console.WriteLine("Generating Dummy Classes");
            AddNewBattle();

            Console.WriteLine("Starting Battle");
        }

        public static void AddNewBattle()
        {
            Army[] Armies = new Army[] { GenerateRandomArmy(), GenerateRandomArmy() };
            string Terrain = "none";
            List<string> Conditions = new();
            Console.WriteLine("Generating Dummy Battle");
            Battles.Add(new(Armies, Terrain, Conditions));
        }

        public static List<BattleStatus> NextRound()
        {
            foreach (Battle battle in Battles)
                battle.BattleStep();
            return GetCurrentStatus();
        }

        public static Army GenerateRandomArmy()
        {
            Army army = new(GenerateRandomFlank(0), GenerateRandomFlank(1), GenerateRandomFlank(2), " ");

            return army;
        }

        public static Flank GenerateRandomFlank(int index)
        {
            Dictionary<Unit, FlankUnitsStats> unitList = new();
            Random rand = new();
            foreach (Unit u in JsonLoader.UnitList)
            {
                unitList.Add(u, new(rand.Next(20, 30)));
            }

            Flank flank = new(new Hero("Bodo"), index, unitList);

            return flank;
        }

        public static List<BattleStatus> GetCurrentStatus()
        {
            List<BattleStatus> status = new();
            foreach (Battle battle in Battles)
                status.Add(new BattleStatus(battle));
            return status;
        }

    }
}
