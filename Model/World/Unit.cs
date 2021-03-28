using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectClause.Model
{
    public class Unit
    {
        //General Info about the unit
        public string Name { get; set; }
        public UnitType Type { get; set; }
        public char UnitSymbol { get; set; }
        public int Size { get; set; }
        //Stats regarding durability
        public double Health { get; set; }
        public double Moral { get; set; }
        public double Armor { get; set; }
        //Offensive Stats
        public double LightDamage { get; set; }
        public double HeavyDamage { get; set; }
        public double RangedCombat { get; set; }
        //Multiplicative stats used in different situations, set in percent
        public double Disengagement { get; set; }
        public double Skirmish { get; set; }
        public double Engagement { get; set; }

        public List<string> Traits { get; set; }

        public Unit()
        {

        }

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

        public Unit Die()
        {
            return this;
        }
    }
}
