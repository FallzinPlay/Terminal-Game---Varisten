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
        public bool Alive { get; private set; }
        public bool WeaponEquiped { get; private set; }
        public sbyte Coins { get; private set; }


        Random random = new Random();

        public Mob(string name, sbyte life)
        {
            this.Name = name;
            this.Life = life;
            Damage = 2;
            Alive = true;
            CriticChance = 5;
            CriticDamage = 2;
        }

        public Mob(string name, sbyte life, sbyte damage) : this(name, life)
        {
            Damage = damage;
            Alive = true;
            CriticChance = 5;
            CriticDamage = 2;
        }

        public Mob(string name, sbyte life, sbyte damage, sbyte maxLife, sbyte criticChance, sbyte criticDamage) : this(name, life, damage)
        {
            MaxLife = maxLife;
            Alive = true;
            CriticChance = criticChance;
            CriticDamage = criticDamage;
        }

        public sbyte TakeDamage()
        {
            sbyte damage = Damage;

            // Se estiver equipando uma arma, somar ataque
            if (WeaponEquiped)
            {
                damage = WeaponDamage;
            }

            // Chance de crítico
            if (random.Next(CriticChance) == 0)
            {
                damage *= CriticDamage;
                Console.WriteLine("[Critical]!!");
            }

            return damage;
        }

        public void GetDamage(sbyte damage)
        {
            this.Life -= damage;
        }

        public void WeaponEquip(string weaponName, sbyte weaponDamage)
        {
            WeaponEquiped = true;
            WeaponName = weaponName;
            WeaponDamage = weaponDamage;
        }

        public void WeaponUnequip()
        {
            WeaponEquiped = false;
            WeaponName = null;
            WeaponDamage = 0;
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
            this.Life += life;
        }

        public void GetCoin(sbyte coins)
        {
            this.Coins += coins;
        }

        public override string ToString()
        {
            return 
                $"[{Name}]\n" +
                $"Life: {Life}\n" +
                $"Damage: {Damage}\n" +
                $"Critic Chance: {CriticChance}\n" +
                $"Critic Damage: {CriticDamage}\n" +
                $"Weapon: {WeaponName}\n" +
                $"Weapon Damage: {WeaponDamage}\n" +
                $"Coins: {Coins}\n";
        }
    }
}
