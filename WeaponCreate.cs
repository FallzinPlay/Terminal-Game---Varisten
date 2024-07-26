using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Game
{
    public class WeaponCreate : Identifier
    {
        public string Name { get; set; }
        public double Damage { get; private set; }
        public int NecessaryLvl { get; private set; }
        public int Condition { get; set; }
        public int MaxCondition { get; private set; }
        public double MinPrice { get; private set; }
        public double MaxPrice { get; private set; }
        public MobCreate User { get; set; }

        private readonly LanguagesManager Language;
        private static readonly Random R = new Random();

        public WeaponCreate(EntityRegistry register, LanguagesManager language, string name, double damage, int maxCondition, int necessaryLvl, double minPrice, double maxPrice)
        {
            this.Name = name;
            this.Damage = damage;
            this.MaxCondition = maxCondition;
            this.Condition = MaxCondition;
            this.NecessaryLvl = necessaryLvl;
            this.MinPrice = minPrice;
            this.MaxPrice = maxPrice;
            this.Language = language;

            register.AddEntity(this);
        }

        public void Erode(bool randomErode = false)
        {
            if (randomErode) this.Condition = R.Next(1, this.MaxCondition);
            else this.Condition--;
            if (this.User != null) Break();
        }

        public void Break()
        {
            if (this.Condition <= 0)
            {
                this.User.WeaponUnequip();
                if (User.Player)
                {
                    // Mostra que a arma quebrou
                    this.Language.ShowSubtitle(this.Language.GetSubtitle("Subtitles", "weaponBroke"));
                }
            }
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
