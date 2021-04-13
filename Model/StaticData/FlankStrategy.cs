using System.Collections.Generic;

namespace ProjectClause.Model.StaticData
{
    public class FlankStrategy
    {
        public string Name { get; set; }
        public Dictionary<UnitType, double> TypeModificator { get; set; }
        public List<UnitType> DisengageUnits { get; set; }
        public List<UnitType> EngageUnits { get; set; }
        public List<UnitType> SkirmishingUnits { get; set; }
        public double[] DamageDistribution { get; set; }
        public int FlankingModificator { get; set; }

        public double LightDamageModificator { get; set; }
        public double HeavyDamageModificator { get; set; }

        public double SkirmishModificator { get; set; }
        public double EngagementModificator { get; set; }
        public double DisengagementModificator { get; set; }
    }
}
