using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectClause.Model
{
    public class Hero
    {
        public string Name { get; private set; }
        public List<string> Abilities { get; private set; }

        public Hero(string name)
        {
            Name = name;
        }
    }
}
