using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectClause.Model
{
    public class Army
    {
        public string Name { get; private set; }
        public Hero ArmyLeader { get; private set; }
        public Flank Center { get; private set; }
        public Flank Left { get; private set; }
        public Flank Right { get; private set; }

        public Army(int number)
        {
            Name = "Army" + number;
            ArmyLeader = new Hero("Balto"+number);

            Center = new Flank("c" + number, ArmyLeader);
            Left = new Flank("l" + number);
            Right = new Flank("r" + number);
        }

    }
}
