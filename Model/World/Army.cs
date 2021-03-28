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
        public Flank[] Flanks { get; private set; }

        public Army(string name)
        {
            Name = name;
            ArmyLeader = new Hero(name + "Balto");

            Center = new Flank(ArmyLeader);
            Left = new Flank();
            Right = new Flank();
            Flanks = new Flank[] { Left, Center, Right };
        }

        public Army(int number)
        {
            Name = "Army" + number;
            ArmyLeader = new Hero("Balto" + number);

            Center = new Flank(ArmyLeader);
            Left = new Flank();
            Right = new Flank();
            Flanks = new Flank[] { Left, Center, Right };
        }

        public Army(Flank left, Flank center, Flank right, string name = "NONE")
        {
            Name = name;
            ArmyLeader = new Hero("Balto" + name);

            Center = center;
            Left = left;
            Right = right;
            Flanks = new Flank[] { Left, Center, Right };
        }

        public void RoundEnd()
        {
            Left.RoundEnd();
            Center.RoundEnd();
            Right.RoundEnd();
        }
    }
}
