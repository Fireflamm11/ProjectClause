namespace ProjectClause.Model.StaticData
{
    public class FlankUnitsStats
    {
        public int Count { get; set; }
        public double Health { get; set; }
        public double Moral { get; set; }

        public FlankUnitsStats(int count, int health = 0, int moral = 0)
        {
            Count = count;
            Health = health;
            Moral = moral;
        }
    }
}
