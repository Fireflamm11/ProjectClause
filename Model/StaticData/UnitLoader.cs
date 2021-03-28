using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProjectClause.Model.StaticData
{
    public class UnitLoader
    {
        public static List<Unit> UnitList { get; private set; } = new List<Unit>();

        public static void LoadUnits()
        {
            using StreamReader reader = File.OpenText(Constants.UnitListPath);
            JsonSerializer serializer = new();
            UnitList = (List<Unit>)serializer.Deserialize(reader, typeof(List<Unit>));
        }

    }


}

