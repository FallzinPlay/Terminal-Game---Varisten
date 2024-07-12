using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using static System.Net.Mime.MediaTypeNames;

namespace Game.Classes
{
    internal class Mob
    {
        public string Name { get; set; }
        public string Race { get; private set; }
        public string Description { get; set; }
        public int Lvl { get; set; }
        public int MaxLvl { get; private set; }
        public int MaxLife { get; private set; }
        public double Life { get; private set; }
        public double Damage { get; private set; }
        public double Dodge { get; private set; }
        public double CriticChance { get; private set; }
        public double CriticDamage { get; private set; }
        public double EscapeChance { get; private set; }
        public double Coins { get; set; }
        public double Xp { get; private set; }
        public double NextLvlXp { get; private set; }
        public bool Alive { get; private set; }
        public bool WeaponEquiped { get; private set; }
        public bool Fighting { get; set; }
        public Weapon Weapons { get; set; }

        private LanguagesManager Language;

        Random random = new Random();

        public Mob(LanguagesManager language, string name, string race, double life, double damage, int maxLife, double criticChance, double criticDamage, Weapon weapon, int lvl, int maxLvl, double dodge, double escapeChance)
        {
            this.Name = name;
            this.Race = race;
            this.MaxLife = maxLife;
            this.Life = life;
            this.MaxLife = maxLife;
            this.Damage = damage;
            this.Alive = true;
            this.CriticChance = criticChance;
            this.CriticDamage = criticDamage;
            this.Dodge = dodge;
            this.Weapons = weapon;
            this.Lvl = lvl;
            this.MaxLvl = maxLvl;
            this.NextLvlXp = 10;
            this.EscapeChance = escapeChance;

            if (weapon.Name != "--") this.WeaponEquiped = true;
            this.Language = language;
        }



        public void GetDamage(double damage)
        {
            this.Life -= damage;
        }

        public bool WeaponEquip(Weapon weapon, int necessaryLvl)
        {
            if (Lvl >= necessaryLvl)
            {
                this.WeaponEquiped = true;
                this.Weapons = weapon;

                Console.WriteLine(this.Language.GetSubtitle("Subtitles", "weaponEquipped"));
                return true;
            }
            else
            {
                Console.WriteLine(this.Language.GetSubtitle("Subtitles", "insufficientLvl"));
                return false;
            }
        }

        public void WeaponUnequip()
        {
            this.WeaponEquiped = false;
            this.Weapons = null;
        }

        public bool Buy(double price)
        {
            this.Coins = Math.Round(this.Coins, 2);
            price = Math.Round(price, 2);
            if (price <= this.Coins)
            {
                this.Coins -= price;
                return true;
            }
            return false;
        }

        public void Cure(double life)
        {
            if (this.Life + life <= this.MaxLife)
            {
                this.Life += life;
            }
            else
            {
                this.Life = MaxLife;
            }
        }

        public void GetCoins(double coins)
        {
            this.Coins += coins;
            Console.WriteLine($"{this.Language.GetSubtitle("Subtitles", "coinsReceived")} {coins.ToString("F2", CultureInfo.InvariantCulture)} {this.Language.GetSubtitle("MobClass", "coins")}\n");
        }

        public void GetXp(double xp)
        {
            this.Xp += xp;
            Console.WriteLine($"{this.Language.GetSubtitle("Subtitles", "xpReceived")} {xp.ToString("F2", CultureInfo.InvariantCulture)}xp\n");
            LvlUp();
        }

        public bool LvlUp()
        {
            if (this.Xp >= this.NextLvlXp)
            {
                this.Lvl += 1;
                this.Xp -= this.NextLvlXp;
                this.NextLvlXp = this.Lvl * 50 / 2;

                this.MaxLife += this.Lvl -1;
                this.Life = this.MaxLife;
                this.Damage += 0.02 * this.Lvl;

                Console.WriteLine(
                    $"{this.Language.GetSubtitle("Subtitles", "lvlUp")} [{Lvl}]\n" +
                    $"{this.Language.GetSubtitle("MobClass", "maxLife")}: {MaxLife}\n" +
                    $"{this.Language.GetSubtitle("MobClass", "damage")}: {Damage.ToString("F2", CultureInfo.InvariantCulture)}\n" +
                    $"{this.Language.GetSubtitle("MobClass", "life")}: [Full]\n");

                return true;
            }

            return false;
        }

        public void ForceLvlUp(int lvl)
        {
            this.Lvl = lvl;
            this.NextLvlXp = this.Lvl * 50 / 2;
            this.Xp = random.Next(this.Lvl, (int)this.NextLvlXp) * random.NextDouble();

            this.MaxLife += this.Lvl -1;
            this.Damage += 0.02 * this.Lvl;
        }

        public override string ToString()
        {
            return
                $"[{Name}]\n" +
                $"{this.Language.GetSubtitle("MobClass", "race")}: {Race}\n" +
                $"Lvl: {Lvl}\n" +
                $"Xp: {Xp.ToString("F2", CultureInfo.InvariantCulture)}\n" +
                $"{this.Language.GetSubtitle("MobClass", "necessaryXp")}: {NextLvlXp}\n" +
                $"{this.Language.GetSubtitle("MobClass", "life")}: {Life.ToString("F2", CultureInfo.InvariantCulture)}\n" +
                $"{this.Language.GetSubtitle("MobClass", "maxLife")}: {MaxLife}\n" +
                $"{this.Language.GetSubtitle("MobClass", "dodge")}: {Dodge.ToString("F2", CultureInfo.InvariantCulture)}\n" +
                $"{this.Language.GetSubtitle("MobClass", "damage")}: {Damage.ToString("F2", CultureInfo.InvariantCulture)}\n" +
                $"{this.Language.GetSubtitle("MobClass", "criticChance")}: {CriticChance.ToString("F2", CultureInfo.InvariantCulture)}\n" +
                $"{this.Language.GetSubtitle("MobClass", "criticDamage")}: {CriticDamage.ToString("F2", CultureInfo.InvariantCulture)}\n" +
                $"{this.Language.GetSubtitle("MobClass", "weapon")}: {Weapons.Name}\n" +
                $"{this.Language.GetSubtitle("MobClass", "coins")}: {Coins.ToString("F2", CultureInfo.InvariantCulture)}\n";
        }
    }
}
