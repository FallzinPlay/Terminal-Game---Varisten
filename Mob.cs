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

namespace Game
{
    internal class Mob
    {
        public string Name { get; set; }
        public string Weapon { get; private set; }
        public double Life { get; private set; }
        public int MaxLife { get; private set; }
        public double Damage { get; private set; }
        public double Dodge { get; private set; }
        public double CriticChance { get; private set; }
        public double CriticDamage { get; private set; }
        public double Coins { get; set; }
        public int Lvl { get; private set; }
        public double Xp { get; private set; }
        public double NextLvlXp { get; private set; }
        public bool Alive { get; private set; }
        public bool WeaponEquiped { get; private set; }


        Random random = new Random();

        public Mob(string name, int maxLife)
        {
            this.Name = name;
            this.MaxLife = maxLife;
            this.Life = maxLife;
            this.Damage = 2.5d;
            this.Alive = true;
            this.CriticChance = 1.2d;
            this.CriticDamage = 1.5d;
            this.Lvl = 1;
            this.NextLvlXp = 10;
            this.Dodge = 1.3d;
        }

        public Mob(string name, double life, double damage, int maxLife, double criticChance, double criticDamage, string weapon, int lvl, double dodge) : this(name, maxLife)
        {
            this.MaxLife = maxLife;
            this.Life = life;
            this.MaxLife = maxLife;
            this.Damage = damage;
            this.Alive = true;
            this.CriticChance = criticChance;
            this.CriticDamage = criticDamage;
            this.Dodge = dodge;
            this.Weapon = weapon;
            this.Lvl = lvl;
            this.NextLvlXp = 10;

            if (weapon != null) this.WeaponEquiped = true;
        }

        public void GetDamage(double damage)
        {
            this.Life -= damage;
        }

        public bool WeaponEquip(string weapon, int necessaryLvl)
        {
            if (Lvl >= necessaryLvl)
            {
                this.WeaponEquiped = true;
                this.Weapon = weapon;

                Console.WriteLine("Weapon equiped!\n");
                return true;
            }
            else
            {
                Console.WriteLine("I don't have enough Lvl!\n");
                return false;
            }
        }

        public void WeaponUnequip()
        {
            this.WeaponEquiped = false;
            this.Weapon = null;
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
            Console.WriteLine($"Coins received: {coins.ToString("F2", CultureInfo.InvariantCulture)} coins\n");
        }

        public void GetXp(double xp)
        {
            this.Xp += xp;
            Console.WriteLine($"Xp received: {xp.ToString("F2", CultureInfo.InvariantCulture)}xp\n");
            LvlUp();
        }

        public bool LvlUp()
        {
            if (this.Xp >= this.NextLvlXp)
            {
                this.Lvl += 1;
                this.Xp -= this.NextLvlXp;
                this.NextLvlXp *= this.Lvl;

                Console.WriteLine(
                    $"Lvl up! [{Lvl}]\n" +
                    $"Max Life: {MaxLife} + 2\n" +
                    $"Damage: {Damage.ToString("F2", CultureInfo.InvariantCulture)} + 1\n" +
                    $"Life: [Full]\n");

                this.MaxLife += 2;
                this.Damage += 1;
                this.Life = this.MaxLife;
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return
                $"[{Name}]\n" +
                $"Lvl: {Lvl}\n" +
                $"Xp: {Xp.ToString("F2", CultureInfo.InvariantCulture)}\n" +
                $"Xp to Next Lvl: {NextLvlXp}\n" +
                $"Life: {Life.ToString("F2", CultureInfo.InvariantCulture)}\n" +
                $"Max Life: {MaxLife}\n" +
                $"Dodge: {Dodge.ToString("F2", CultureInfo.InvariantCulture)}\n" +
                $"Damage: {Damage.ToString("F2", CultureInfo.InvariantCulture)}\n" +
                $"Critic Chance: {CriticChance.ToString("F2", CultureInfo.InvariantCulture)}\n" +
                $"Critic Damage: {CriticDamage.ToString("F2", CultureInfo.InvariantCulture)}\n" +
                $"Weapon: {Weapon}\n" +
                $"Coins: {Coins.ToString("F2", CultureInfo.InvariantCulture)}\n";
        }
    }
}
