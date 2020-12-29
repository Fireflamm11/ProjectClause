using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectClause.Model
{
    public class Unit
    {
        public string Name { get; private set; }
        public UnitType Type { get; private set; }
        public char UnitSymbol { get; private set; }
        public int Size { get; private set; }

        public double LightDamage { get; private set; }
        public double HeavyDamage { get; private set; }
        public double Armor { get; private set; }
        public double Disengagement { get; private set; }
        public double Health { get; private set; }
        public double Moral { get; private set; }
        public double Skirmish { get; private set; }
        public double Engagement { get; private set; }
        public double RangedCombat { get; private set; }

        public List<string> Traits { get; private set; }

        public Unit(string name, UnitType type, List<string> traits, char symbol = UnitSymbols.Infantry, int size = 1, int lightDamage = 1, int heavyDamage = 1, int armor = 1, int disengagement = 1, int health = 1, int moral = 1, int skirmish = 1, int engagement = 1, int rangedCombat = 1)
        {
            Name = name;
            Type = type;
            UnitSymbol = symbol;
            Size = size;
            Traits = traits;

            LightDamage = lightDamage;
            HeavyDamage = heavyDamage;
            Armor = armor;
            Disengagement = disengagement;
            Health = health;
            Moral = moral;
            Skirmish = skirmish;
            Engagement = engagement;
            RangedCombat = rangedCombat;
        }

        public Unit(Unit unit)
        {
            Name = unit.Name;
            Type = unit.Type;
            UnitSymbol = unit.UnitSymbol;
            Size = unit.Size;
            Traits = new List<string>(unit.Traits);

            LightDamage = unit.LightDamage;
            HeavyDamage = unit.HeavyDamage;
            Armor = unit.Armor;
            Disengagement = unit.Disengagement;
            Health = unit.Health;
            Moral = unit.Moral;
            Skirmish = unit.Skirmish;
            Engagement = unit.Engagement;
            RangedCombat = unit.RangedCombat;
        }

        public double DamageTaken(double damage)
        {
            Health -= damage;
            return Health;
        }
    }
}
