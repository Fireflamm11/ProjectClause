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

        public Army(string armyName)
        {
            Name = armyName;
            ArmyLeader = new Hero(armyName + "Balto");

            Center = new Flank(ArmyLeader, 1);
            Left = new Flank(0);
            Right = new Flank(2);
            Flanks = new Flank[] { Left, Center, Right };
        }

        public Army(int number)
        {
            Name = "Army" + number;
            ArmyLeader = new Hero("Balto" + number);

            Center = new Flank(ArmyLeader, 1);
            Left = new Flank(0);
            Right = new Flank(2);
            Flanks = new Flank[] { Left, Center, Right };
        }

        public Army(Flank left, Flank center, Flank right, string name = "NONE")
        {
            Name = name;
            ArmyLeader = new Hero(name);

            Center = center;
            Left = left;
            Right = right;
            Flanks = new Flank[] { Left, Center, Right };
        }

        public bool IsBeaten()
        {
            if (Left.IsEmpty() && Center.IsEmpty() && Right.IsEmpty())
            {
                return true;
            }
            return false;
        }

        public void RoundEnd()
        {
            Left.RoundEnd();
            Center.RoundEnd();
            Right.RoundEnd();
        }
    }
}
