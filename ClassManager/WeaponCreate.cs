using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Game
{
    public abstract class WeaponCreate : Identifier
    {
        public string Name { get; set; }
        public double Damage { get; protected set; }
        public int NecessaryLvl { get; protected set; }
        public int Condition { get; set; }
        public int MaxCondition { get; protected set; }
        public double MinPrice { get; protected set; }
        public double MaxPrice { get; protected set; }

        private static readonly Random R = new Random();

        public void Erode(bool randomErode = false)
        {
            if (randomErode) this.Condition = R.Next(1, this.MaxCondition);
            else this.Condition--;
        }

        public virtual string ShowInfo(LanguagesManager s)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(
                $"[{this.Name}]\n" +
                $"{s.GetSubtitle("WeaponClass", "damage")}  : {this.Damage.ToString("F2", CultureInfo.InvariantCulture)}\n" +
                $"{s.GetSubtitle("WeaponClass", "condition")}: {this.Condition}\n" +
                $"{s.GetSubtitle("WeaponClass", "necessaryLvl")}: {this.NecessaryLvl}\n");
            return sb.ToString();
        }
    }
}
