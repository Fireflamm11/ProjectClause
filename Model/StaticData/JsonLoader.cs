using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProjectClause.Model.StaticData
{
    public class JsonLoader
    {
        public static bool IsLoaded { get; private set; } = false;
        public static List<Unit> UnitList { get; private set; } = new List<Unit>();
        public static List<FlankStrategy> FlankStrategyList { get; private set; } = new List<FlankStrategy>();

        public static Dictionary<string, Unit> UnitDict { get; private set; } = new Dictionary<string, Unit>();
        public static Dictionary<string, FlankStrategy> FlankStrategyDict { get; private set; } = new Dictionary<string, FlankStrategy>();

        public static void LoadJson()
        {
            if (IsLoaded) return;
            LoadUnits();
            LoadFlankStrategy();
            IsLoaded = true;
        }

        private static void LoadUnits()
        {
            using StreamReader reader = File.OpenText(Constants.UnitListPath);
            JsonSerializer serializer = new();
            UnitList = (List<Unit>)serializer.Deserialize(reader, typeof(List<Unit>));
            foreach (Unit unit in UnitList)
            {
                UnitDict.Add(unit.Name, unit);
            }
        }

        private static void LoadFlankStrategy()
        {
            using StreamReader reader = File.OpenText(Constants.FlankStrategyListPath);
            JsonSerializer serializer = new();
            FlankStrategyList = (List<FlankStrategy>)serializer.Deserialize(reader, typeof(List<FlankStrategy>));
            foreach (FlankStrategy strat in FlankStrategyList)
            {
                FlankStrategyDict.Add(strat.Name, strat);
            }
        }

    }


}

