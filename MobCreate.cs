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
using System.Security.Cryptography;

namespace Game
{
    public class MobCreate : Identifier
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
        public bool Player {  get; set; }
        public bool WeaponEquiped { get; private set; }
        public MobState State {  get; set; }
        public WeaponCreate Weapon { get; private set; }

        private readonly LanguagesManager Language;

        public MobCreate(EntityRegistry register, LanguagesManager language, string name, string race, double damage, int maxLife, double criticChance, double criticDamage, WeaponCreate weapon, int maxLvl, double dodge, double escapeChance)
        {
            this.Name = name;
            this.Race = race;
            this.MaxLife = maxLife;
            this.Life = this.MaxLife;
            this.MaxLife = maxLife;
            this.Damage = damage;
            this.CriticChance = criticChance;
            this.CriticDamage = criticDamage;
            this.Dodge = dodge;
            this.Weapon = weapon;
            this.Lvl = 1;
            this.MaxLvl = maxLvl;
            this.NextLvlXp = 10;
            this.EscapeChance = escapeChance;
            this.State = MobState.Exploring;
            this.Language = language;

            WeaponEquip(weapon);

            register.AddEntity(this);
        }

        #region Combat

        // Esquiva
        public bool GetDodge()
        {
            if (Tools.RandomChance(this.Dodge))
            {
                // Legenda
                this.Language.ShowSubtitle(
                    $"[{this.Name} " +
                    $"{this.Language.GetSubtitle("Combat", "dodged")}]!!");

                return true;
            }
            return false;
        }

        // Recebe dano
        public double GetDamage(double damage)
        {
            this.Life -= damage;
            return damage;
        }

        // Ataque
        public double SetDamage(MobCreate enemy)
        {
            this.Language.ShowSubtitle($"[{this.Name}]");

            double damage = 0;
            if (!enemy.GetDodge())
            {
                damage = this.Damage;
                // Se estiver equipando uma arma, somar ataque
                if (this.WeaponEquiped)
                {
                    damage = this.Weapon.Damage + this.Damage / 5;
                    this.Weapon.Erode();
                }

                // Chance de crítico
                if (Tools.RandomChance(this.CriticChance))
                {
                    damage *= this.CriticDamage;

                    // Legenda
                    this.Language.ShowSubtitle(
                        $"[{this.Language.GetSubtitle("Combat", "critical")}]!!");
                }
                enemy.GetDamage(damage);
            }

            // Mostra quanto dano ô jogador causou ao inimigo
            this.Language.ShowSubtitle(
                this.Language.GetSubtitle(
                    "Combat", "damageTo").Replace("#1", damage.ToString("F2", CultureInfo.InvariantCulture)).Replace("#2", enemy.Name) +
                    "\n");

            return damage;
        }

        #endregion

        #region Weapon
        public bool WeaponEquip(WeaponCreate weapon)
        {
            bool equipped = true;
            if (weapon.Name == "--") equipped = false;
            if (this.Player)
            {
                if (Lvl >= weapon.NecessaryLvl)
                {
                    // Legenda
                    this.Language.ShowSubtitle(
                        this.Language.GetSubtitle("Subtitles", "weaponEquipped"));
                }
                else
                {
                    // Legenda
                    this.Language.ShowSubtitle(this.Language.GetSubtitle("Subtitles", "insufficientLvl"));
                    return false;
                }
            }

            this.WeaponEquiped = equipped;
            this.Weapon = weapon;
            this.Weapon.User = this;
            return true;
        }

        public void WeaponUnequip()
        {
            this.WeaponEquiped = false;
            this.Weapon = null;
            this.Weapon.User = null;
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

        public bool Cure(double life)
        {
            if (this.Life + life <= this.MaxLife)
            {
                this.Life += life;
            }
            else
            {
                this.Life = this.MaxLife;
            }
            return true;
        }

        public bool GetCoins(double coins)
        {
            this.Coins += coins;
            if (this.Player)
            {
                // Legenda
                this.Language.ShowSubtitle(
                    $"{this.Language.GetSubtitle("Subtitles", "coinsReceived")} " +
                    $"{coins.ToString("F2", CultureInfo.InvariantCulture)} " +
                    $"{this.Language.GetSubtitle("MobClass", "coins")}");
            }

            return true;
        }

        #region Lvl
        public bool GetXp(double xp)
        {
            this.Xp += xp;
            LvlUp();

            if (this.Player)
            {
                //Legenda
                this.Language.ShowSubtitle(
                    $"{this.Language.GetSubtitle("Subtitles", "xpReceived")} " +
                    $"{xp.ToString("F2", CultureInfo.InvariantCulture)}xp");
            }

            return true;
        }

        public void LvlUp(int lvl = 0)
        {
            if (!this.Player) if (lvl > 0) this.Lvl = lvl;
            if (this.Xp >= this.NextLvlXp)
            {
                this.Lvl += 1;
                lvl = this.Lvl - 1;
                this.Xp -= this.NextLvlXp;
            }

            this.NextLvlXp = this.Lvl * 50 / 2;
            this.MaxLife += lvl;
            this.Damage += 0.02 * lvl;

            if (this.Player)
            {
                Cure(this.MaxLife);

                // Legenda
                this.Language.ShowSubtitle(
                    $"{this.Language.GetSubtitle("Subtitles", "lvlUp")} [{this.Lvl}]\n" +
                    $"{this.Language.GetSubtitle("MobClass", "maxLife")}: {this.MaxLife}\n" +
                    $"{this.Language.GetSubtitle("MobClass", "damage")}: {this.Damage.ToString("F2", CultureInfo.InvariantCulture)}\n" +
                    $"{this.Language.GetSubtitle("MobClass", "life")}: {this.Life}\n");
            }
        }
        #endregion

        public string SetDescription(string description)
        {
            this.Description = description;
            return this.Description;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(
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
                $"{this.Language.GetSubtitle("MobClass", "coins")}: {this.Coins.ToString("F2", CultureInfo.InvariantCulture)}");

            return sb.ToString();
        }
    }
}
