using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Game
{
    internal class Mob
    {
        public string Name { get; set; }
        public string Weapon { get; private set; }
        public sbyte Life { get; private set; }
        public sbyte MaxLife { get; private set; }
        public sbyte Damage { get; private set; }
        public sbyte Dodge {get; private set; }
        public sbyte CriticChance { get; private set; }
        public sbyte CriticDamage { get; private set; }
        public sbyte Coins { get; set; }
        public sbyte Lvl { get; private set; }
        public double Xp { get; private set; }
        public sbyte NextLvlXp {  get; private set; }
        public bool Alive { get; private set; }
        public bool WeaponEquiped { get; private set; }


        Random random = new Random();

        public Mob(string name, sbyte maxLife)
        {
            this.Name = name;
            this.MaxLife = maxLife;
            this.Life = maxLife;
            this.Damage = 2;
            this.Alive = true;
            this.CriticChance = 5;
            this.CriticDamage = 2;
            this.Lvl = 1;
            this.NextLvlXp = 10;
            this.Dodge = 3;
        }

        public Mob(string name, sbyte life, sbyte damage, sbyte maxLife, sbyte criticChance, sbyte criticDamage, sbyte dodge) : this(name, maxLife)
        {
            this.MaxLife = maxLife;
            this.Life = life;
            this.Life = maxLife;
            this.Damage = damage;
            this.Alive = true;
            this.CriticChance = criticChance;
            this.CriticDamage = criticDamage;
            this.Lvl = 1;
            this.Dodge = dodge;
        }

        public Mob(string name, sbyte life, sbyte damage, sbyte maxLife, sbyte criticChance, sbyte criticDamage, string weapon, sbyte lvl, sbyte dodge) : this(name, life, damage, maxLife, criticChance, criticDamage, dodge)
        {
            this.Alive = true;
            this.Weapon = weapon;
            this.WeaponEquiped = true;
            this.Lvl = lvl;
        }

        public void GetDamage(sbyte damage)
        {
            this.Life -= damage;
        }

        public bool WeaponEquip(string weapon, sbyte necessaryLvl)
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
                Console.WriteLine("You don't have enough Lvl!\n");
                return false;
            }
        }

        public void WeaponUnequip()
        {
            this.WeaponEquiped = false;
            this.Weapon = null;
        }

        public bool Buy(sbyte price)
        {
            if (price <= this.Coins)
            {
                this.Coins -= price;
                return true;
            }
            return false;
        }

        public void Cure(sbyte life)
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

        public void GetXp(double xp)
        {
            this.Xp += xp;
            Console.WriteLine($"You received {xp}xp\n");
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
                    $"Damage: {Damage} + 1\n" +
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
                $"Xp: {Xp}\n" +
                $"Xp to Next Lvl: {NextLvlXp}\n" +
                $"Life: {Life}\n" +
                $"Max Life: {MaxLife}\n" +
                $"Damage: {Damage}\n" +
                $"Critic Chance: {CriticChance}\n" +
                $"Critic Damage: {CriticDamage}\n" +
                $"Weapon: {Weapon}\n" +
                $"Coins: {Coins}\n";
        }
    }
}
