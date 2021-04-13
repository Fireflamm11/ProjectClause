using ProjectClause.Model.StaticData;
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

        public FlankStrategy DecideStrategy(Flank enemeFlank, bool isFlanking)
        {
            return JsonLoader.FlankStrategyList[0];
        }

        public int FlankingTarget(Flank[] enemeFlanks, int index)
        {
            if (index == 1)
            {
                int[] possibleIndex = { 0, 2 };
                Random rand = new Random();
                int idx = rand.Next(2);

                if (!enemeFlanks[possibleIndex[idx]].IsEmpty())
                {
                    return possibleIndex[idx];
                }
                else
                {
                    return possibleIndex[((idx + 1) % 2)];
                }
            }
            else
            {
                return 1;
            }
        }
    }
}
