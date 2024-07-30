using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.ClassManager;

namespace Game.Entities
{
    internal class Player : MobCreate
    {
        private LanguagesManager _language;

        public Player(LanguagesManager language, string name)
        {
            Name = name;
            MaxLvl = 20;
            MaxLife = 10;
            Life = MaxLife;
            Damage = 2.7d;
            Dodge = 1.2d;
            CriticChance = 1.5d;
            CriticDamage = 1.5d;
            Lvl = 1;
            NextLvlXp = 10;
            State = MobState.Exploring;

            _language = language;
        }

        public override double Atack(MobCreate enemy)
        {
            double _damage = base.Atack(enemy);
            if (Weapon.Condition <= 0)
            {
                WeaponUnequip();
                _language.ShowSubtitle(
                    _language.GetSubtitle("Weapon", "broke"));
            }
            return _damage;
        }

        public sealed override void Cure(double life)
        {
            if (Life + life >= MaxLife)
                throw new GameException(_language.GetSubtitle("Player", "fullLife"));

            base.Cure(life);
        }

        public bool Buy(double price)
        {
            if (price > Coins)
            {
                _language.ShowSubtitle(_language.GetSubtitle("Merchant", "noCoins"));
                return false;
            }

            Coins = Math.Round(Coins, 2);
            price = Math.Round(price, 2);
            Coins -= price;
            return true;
        }

        public sealed override void GetXp(double xp)
        {
            base.GetXp(xp);
            if (Xp >= NextLvlXp)
            {
                Xp -= NextLvlXp;
                LvlUp();
            }
        }

        public sealed override void LvlUp(int lvl = 1)
        {
            base.LvlUp(lvl);
            Cure(MaxLife);
            NextLvlXp = Lvl * 42;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(
                $"[{Name}]\n" +
                $"Lvl: {Lvl}\n" +
                $"Xp: {Xp.ToString("F2", CultureInfo.InvariantCulture)}\n" +
                $"{_language.GetSubtitle("MobClass", "necessaryXp")}: {NextLvlXp}\n" +
                $"{_language.GetSubtitle("MobClass", "maxLife")}: {MaxLife}\n" +
                $"{_language.GetSubtitle("MobClass", "life")}: {Life.ToString("F2", CultureInfo.InvariantCulture)}\n" +
                $"{_language.GetSubtitle("MobClass", "damage")}: {Damage.ToString("F2", CultureInfo.InvariantCulture)}\n" +
                $"{_language.GetSubtitle("MobClass", "dodge")}: {Dodge.ToString("F2", CultureInfo.InvariantCulture)}\n" +
                $"{_language.GetSubtitle("MobClass", "criticChance")}: {CriticChance.ToString("F2", CultureInfo.InvariantCulture)}\n" +
                $"{_language.GetSubtitle("MobClass", "criticDamage")}: {CriticDamage.ToString("F2", CultureInfo.InvariantCulture)}\n" +
                $"{_language.GetSubtitle("MobClass", "weapon")}: {Weapon.Name}\n" +
                $"{_language.GetSubtitle("MobClass", "coins")}: {Coins.ToString("F2", CultureInfo.InvariantCulture)}");

            return sb.ToString();
        }
    }
}
