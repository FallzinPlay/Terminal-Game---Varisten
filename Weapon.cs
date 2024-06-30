using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    internal class Weapon
    {
        public string Name { get; set; }
        public sbyte Damage { get; private set; }
        public sbyte NecessaryLvl { get; private set; }
        public int Condition { get; set; }
        public int MaxCondition { get; private set; }

        public Weapon(string name, sbyte damage, int maxCondition, sbyte necessaryLvl)
        {
            Name = name;
            Damage = damage;
            MaxCondition = maxCondition;
            this.NecessaryLvl = necessaryLvl;
        }

        public Weapon(string name, sbyte damage, int condition, int maxCondition, sbyte necessaryLvl) : this(name, damage, maxCondition, necessaryLvl)
        {
            this.Condition = condition;
        }

        public override string ToString()
        {
            return
                "[WEAPON]\n" +
                $"Name: {Name}\n" +
                $"Damage: {Damage}\n" +
                $"Condition: {Condition}\n" +
                $"Necessary Lvl: {NecessaryLvl}";
        }
    }
}
