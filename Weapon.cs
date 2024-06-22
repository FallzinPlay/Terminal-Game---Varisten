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
        public int Condition { get; set; }
        public int MaxCondition { get; private set; }

        public Weapon(string name, sbyte damage, int maxCondition)
        {
            Name = name;
            Damage = damage;
            MaxCondition = maxCondition;
        }

        public Weapon(string name, sbyte damage, int condition, int maxCondition) : this(name, damage, maxCondition)
        {
            Condition = condition;
        }

        public override string ToString()
        {
            return
                "[WEAPON]\n" +
                $"Name: {Name}\n" +
                $"Damage: {Damage}\n" +
                $"Condition: {Condition}";
        }
    }
}
