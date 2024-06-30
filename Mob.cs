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
        public string WeaponName { get; private set; }
        public sbyte Life { get; private set; }
        public sbyte MaxLife { get; private set; }
        public sbyte Damage { get; private set; }
        public sbyte CriticChance { get; private set; }
        public sbyte CriticDamage { get; private set; }
        public sbyte WeaponDamage { get; private set; }
        public sbyte WeaponContition { get; set; }
        public sbyte Coins { get; set; }
        public sbyte Lvl { get; private set; }
        public sbyte Xp { get; private set; }
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
        }

        public Mob(string name, sbyte life, sbyte damage) : this(name, life)
        {
            this.Damage = damage;
            this.Alive = true;
            this.CriticChance = 5;
            this.CriticDamage = 2;
        }

        public Mob(string name, sbyte life, sbyte damage, sbyte maxLife, sbyte criticChance, sbyte criticDamage) : this(name, life, damage)
        {
            this.MaxLife = maxLife;
            this.Alive = true;
            this.CriticChance = criticChance;
            this.CriticDamage = criticDamage;
            this.Lvl = 1;
        }

        public Mob(string name, sbyte life, sbyte damage, sbyte maxLife, sbyte criticChance, sbyte criticDamage, string weaponName, sbyte weaponDamage, sbyte weaponCondition, sbyte lvl) : this(name, life, damage, maxLife, criticChance, criticDamage)
        {
            this.WeaponName = weaponName;
            this.WeaponDamage = weaponDamage;
            this.WeaponContition = weaponCondition;
            this.Lvl = lvl;
        }

        public sbyte TakeDamage()
        {
            sbyte damage = this.Damage;

            // Se estiver equipando uma arma, somar ataque
            if (WeaponEquiped)
            {
                damage = WeaponDamage;
                this.WeaponContition--;
            }

            // Chance de crítico
            if (random.Next(CriticChance) == 0)
            {
                damage *= CriticDamage;
                Console.WriteLine("[Critical]!!\n");
            }

            return damage;
        }

        public void GetDamage(sbyte damage)
        {
            this.Life -= damage;
        }

        public bool WeaponEquip(string weaponName, sbyte weaponDamage, sbyte weaponCondition, sbyte necessaryLvl)
        {
            if (Lvl >= necessaryLvl)
            {
                this.WeaponEquiped = true;
                this.WeaponName = weaponName;
                this.WeaponDamage = weaponDamage;
                this.WeaponContition = weaponCondition;

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
            WeaponEquiped = false;
            WeaponName = null;
            WeaponDamage = 0;
            WeaponContition = 0;
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

        public void GetXp(sbyte xp)
        {
            this.Xp += xp;
            Console.WriteLine($"You received {Xp}xp\n");
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
                $"Weapon: {WeaponName}\n" +
                $"Weapon Damage: {WeaponDamage}\n" +
                $"Weapon Condition: {WeaponContition}\n" +
                $"Coins: {Coins}\n";
        }
    }
}
