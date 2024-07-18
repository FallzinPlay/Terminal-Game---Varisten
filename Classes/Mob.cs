﻿using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using static System.Net.Mime.MediaTypeNames;
using System.Security.Cryptography;

namespace Game.Classes
{
    internal class Mob
    {
        public string Name { get; set; }
        public string Race { get; private set; }
        public string Description { get; private set; }
        public int Lvl { get; private set; }
        public int MaxLvl { get; private set; }
        public int MaxLife { get; private set; }
        public double Life { get; private set; }
        public double Damage { get; private set; }
        public double Dodge { get; private set; }
        public double CriticChance { get; private set; }
        public double CriticDamage { get; private set; }
        public double EscapeChance { get; private set; }
        public double Coins { get; private set; }
        public double Xp { get; private set; }
        public double NextLvlXp { get; private set; }
        public bool Alive { get; private set; }
        public bool WeaponEquiped { get; private set; }
        public bool Fighting { get; set; }
        public Weapon Weapon { get; private set; }

        readonly LanguagesManager Language;
        readonly Random random = new Random();

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
            this.Weapon = weapon;
            this.Lvl = lvl;
            this.MaxLvl = maxLvl;
            this.NextLvlXp = 10;
            this.EscapeChance = escapeChance;

            if (weapon.Name != "--") this.WeaponEquiped = true;
            this.Language = language;
        }


        #region Combat

        // Esquiva
        public bool GetDodge(double randomization)
        {
            bool dodged = randomization <= this.Dodge / 3;
            if (dodged)
            {
                // Legenda
                Console.WriteLine(
                    $"[{this.Name}" +
                    $"{this.Language.GetSubtitle("Combat", "dodged")}]!!");

                return true;
            }
            return false;
        }

        // Recebe dano
        public double GetDamage(double damage)
        {
            this.Life -= Damage;
            return damage;
        }

        // Ataque
        public double SetDamage(double randomization, Mob enemy, params Weapon[] weapons)
        {
            if (!GetDodge(randomization))
            {
                double damage = this.Damage;

                // Se estiver equipando uma arma, somar ataque
                bool invalidWeapon = false;
                foreach (Weapon w in weapons) if (this.Weapon == w) invalidWeapon = true; // Verifica se está usando uma arma valida
                if (this.WeaponEquiped && !invalidWeapon)
                {
                    damage = this.Weapon.Damage + this.Damage / 5;
                    this.Weapon.Erode();
                }

                // Chance de crítico
                bool criticChance = randomization <= this.CriticChance / 3;
                if (criticChance)
                {
                    damage *= this.CriticDamage;

                    // Legenda
                    Console.WriteLine(
                        $"[{this.Language.GetSubtitle("Combat", "critical")}]!!\n");
                }

                // O mob atacado recebe o dano
                enemy.GetDamage(damage);
                return damage;
            }

            return 0;
        }

        #endregion

        #region Weapon
        public bool WeaponEquip(Weapon weapon, int necessaryLvl)
        {
            if (Lvl >= necessaryLvl)
            {
                this.WeaponEquiped = true;
                this.Weapon = weapon;

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
            this.Weapon = null;
        }
        #endregion

        #region Trade
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
        #endregion

        public void Cure(double life)
        {
            if (this.Life + life <= this.MaxLife)
            {
                this.Life += life;
            }
            else
            {
                this.Life = this.MaxLife;
            }
        }

        public void GetCoins(double coins)
        {
            this.Coins += coins;

            // Legenda
            this.Language.ShowSubtitle(
                $"{this.Language.GetSubtitle("Subtitles", "coinsReceived")}" +
                $"{coins.ToString("F2", CultureInfo.InvariantCulture)}" +
                $"{this.Language.GetSubtitle("MobClass", "coins")}\n");
        }

        #region Lvl
        public void GetXp(double xp)
        {
            this.Xp += xp;

            //Legenda
            this.Language.ShowSubtitle(
                $"{this.Language.GetSubtitle("Subtitles", "xpReceived")}" +
                $"{xp.ToString("F2", CultureInfo.InvariantCulture)}xp\n");

            LvlUp();
        }

        public bool LvlUp(int lvl = 0)
        {
            // Eleva o mob para o nível desejado
            if (lvl > 0)
            {
                this.Lvl = lvl;
                this.NextLvlXp = this.Lvl * 50 / 2;
                this.Xp = random.Next(this.Lvl, (int)this.NextLvlXp) * random.NextDouble();

                this.MaxLife += this.Lvl - 1;
                this.Damage += 0.02 * this.Lvl;
            }
            else // Evolui conforme o Xp aumenta
            {
                if (this.Xp >= this.NextLvlXp)
                {
                    this.Lvl += 1;
                    this.Xp -= this.NextLvlXp;
                    this.NextLvlXp = this.Lvl * 50 / 2;

                    this.MaxLife += this.Lvl - 1;
                    this.Life = this.MaxLife;
                    this.Damage += 0.02 * this.Lvl;

                    // Legenda
                    this.Language.ShowSubtitle(
                        $"{this.Language.GetSubtitle("Subtitles", "lvlUp")} [{Lvl}]\n" +
                        $"{this.Language.GetSubtitle("MobClass", "maxLife")}: {MaxLife}\n" +
                        $"{this.Language.GetSubtitle("MobClass", "damage")}: {this.Damage.ToString("F2", CultureInfo.InvariantCulture)}\n" +
                        $"{this.Language.GetSubtitle("MobClass", "life")}: [Full]\n");

                    return true;
                }
            }

            return false;
        }
        #endregion

        public string SetDescription(string description)
        {
            this.Description = description;
            return this.Description;
        }

        public override string ToString()
        {
            return
                $"[{this.Name}]\n" +
                $"{this.Language.GetSubtitle("MobClass", "race")}: {this.Race}\n" +
                $"Lvl: {this.Lvl}\n" +
                $"Xp: {this.Xp.ToString("F2", CultureInfo.InvariantCulture)}\n" +
                $"{this.Language.GetSubtitle("MobClass", "necessaryXp")}: {this.NextLvlXp}\n" +
                $"{this.Language.GetSubtitle("MobClass", "life")}: {this.Life.ToString("F2", CultureInfo.InvariantCulture)}\n" +
                $"{this.Language.GetSubtitle("MobClass", "maxLife")}: {this.MaxLife}\n" +
                $"{this.Language.GetSubtitle("MobClass", "dodge")}: {this.Dodge.ToString("F2", CultureInfo.InvariantCulture)}\n" +
                $"{this.Language.GetSubtitle("MobClass", "damage")}: {this.Damage.ToString("F2", CultureInfo.InvariantCulture)}\n" +
                $"{this.Language.GetSubtitle("MobClass", "criticChance")}: {this.CriticChance.ToString("F2", CultureInfo.InvariantCulture)}\n" +
                $"{this.Language.GetSubtitle("MobClass", "criticDamage")}: {this.CriticDamage.ToString("F2", CultureInfo.InvariantCulture)}\n" +
                $"{this.Language.GetSubtitle("MobClass", "weapon")}: {this.Weapon.Name}\n" +
                $"{this.Language.GetSubtitle("MobClass", "coins")}: {this.Coins.ToString("F2", CultureInfo.InvariantCulture)}\n";
        }
    }
}
