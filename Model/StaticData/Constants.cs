using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectClause.Model.StaticData
{
    public class Constants
    {
        //Paths
        public static readonly string UnitListPath = Directory.GetCurrentDirectory() +@"\\Model\\StaticData\\UnitData.json";
        public static readonly string FlankStrategyListPath = Directory.GetCurrentDirectory() +@"\\Model\\StaticData\\FlankStrategyData.json";
    }
}
