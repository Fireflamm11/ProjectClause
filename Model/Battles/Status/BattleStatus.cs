namespace ProjectClause.Model.Battles
{
    public class BattleStatus
    {
        public ArmyStatus[] armyStati;

        public BattleStatus(Battle battle)
        {
            armyStati = new ArmyStatus[] { new ArmyStatus(battle.Armies[0]), new ArmyStatus(battle.Armies[1]) };
        }
    }
}
