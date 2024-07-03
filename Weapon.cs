using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Game
{
    internal class Weapon
    {
        public string Name { get; set; }
        public double Damage { get; private set; }
        public int NecessaryLvl { get; private set; }
        public int Condition { get; set; }
        public int MaxCondition { get; private set; }
        public double MinPrice { get; private set; }
        public double MaxPrice { get; private set; }

        public Weapon(string name, double damage, int maxCondition, int necessaryLvl, double minPrice, double maxPrice)
        {
            this.Name = name;
            this.Damage = damage;
            this.MaxCondition = maxCondition;
            this.Condition = MaxCondition;
            this.NecessaryLvl = necessaryLvl;
            this.MinPrice = minPrice;
            this.MaxPrice = maxPrice;
        }

        public override string ToString()
        {
            return
                "[WEAPON]\n" +
                $"Name: {Name}\n" +
                $"Damage: {Damage.ToString("F2", CultureInfo.InvariantCulture)}\n" +
                $"Condition: {Condition}\n" +
                $"Necessary Lvl: {NecessaryLvl}\n";
        }
    }
}
