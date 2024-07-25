using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Game
{
    internal class WeaponCreate
    {
        public string Name { get; set; }
        public double Damage { get; private set; }
        public int NecessaryLvl { get; private set; }
        public int Condition { get; set; }
        public int MaxCondition { get; private set; }
        public double MinPrice { get; private set; }
        public double MaxPrice { get; private set; }

        private readonly LanguagesManager Language;
        private readonly Random R = new Random();

        public WeaponCreate(LanguagesManager language, string name, double damage, int maxCondition, int necessaryLvl, double minPrice, double maxPrice)
        {
            this.Name = name;
            this.Damage = damage;
            this.MaxCondition = maxCondition;
            this.Condition = MaxCondition;
            this.NecessaryLvl = necessaryLvl;
            this.MinPrice = minPrice;
            this.MaxPrice = maxPrice;

            this.Language = language;
        }

        public void Erode(bool randomErode = false)
        {
            if (randomErode) this.Condition = R.Next(1, this.MaxCondition);
            else this.Condition--;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(
                $"[{this.Name}]\n" +
                $"{this.Language.GetSubtitle("WeaponClass", "damage")}  : {this.Damage.ToString("F2", CultureInfo.InvariantCulture)}\n" +
                $"{this.Language.GetSubtitle("WeaponClass", "condition")}: {this.Condition}\n" +
                $"{this.Language.GetSubtitle("WeaponClass", "necessaryLvl")}: {this.NecessaryLvl}\n");

            return sb.ToString();
        }
    }
}
