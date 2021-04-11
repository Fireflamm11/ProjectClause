using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectClause.Model.Battles
{
    public class BattleStatus
    {
        public List<char[]>[] army1;
        public List<char[]>[] army2;

        public BattleStatus(List<char[]>[] army1, List<char[]>[] army2)
        {
            this.army1 = army1;
            this.army2 = army2;
        }

        public BattleStatus(List<char[]>[][] armies)
        {
            army1 = armies[0];
            army2 = armies[1];
        }
    }
}
