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

namespace Game.ClassManager
{
    public abstract class MobCreate : Identifier
    {
        public string Name { get; set; }
        public string Description { get; protected set; }
        public int Lvl { get; protected set; }
        public int MaxLvl { get; protected set; }
        public int MaxLife { get; protected set; }
        public double Life { get; protected set; }
        public double Damage { get; protected set; }
        public double Dodge { get; protected set; }
        public double CriticChance { get; protected set; }
        public double CriticDamage { get; protected set; }
        public double EscapeChance { get; protected set; }
        public double Coins { get; protected set; }
        public double Xp { get; protected set; }
        public double NextLvlXp { get; protected set; }
        public MobState State {  get; set; }
        public WeaponCreate Weapon { get; protected set; }

        #region Combat

        // Esquiva
        public bool GetDodge()
        {
            if (Tools.RandomChance(Dodge))
                return true;
            return false;
        }

        // Recebe dano
        public void GetDamage(double damage)
        {
            Life -= damage;
            if (Life <= 0)
                State = MobState.Death;
        }

        // Ataque
        public virtual double Atack(MobCreate enemy)
        {
            double damage = 0;
            if (!enemy.GetDodge())
            {
                damage = Damage;
                // Se estiver equipando uma arma, somar ataque
                if (Weapon != null)
                {
                    damage = Weapon.Damage + Damage / 5;
                    Weapon.Erode();
                    if (Weapon.Condition <= 0)
                    {
                        WeaponUnequip();
                    }
                }

                // Chance de crítico
                if (Tools.RandomChance(CriticChance))
                {
                    damage *= CriticDamage;
                }
                enemy.GetDamage(damage);
            }
            return damage;
        }

        #endregion

        #region Weapon
        public void WeaponEquip(WeaponCreate weapon)
        {
            Weapon = weapon;
        }

        public void WeaponUnequip()
        {
            Weapon = null;
        }
        #endregion

        public virtual void Cure(double life)
        {
            Life += life;
        }

        public void GetCoins(double coins)
        {
            Coins += coins;
        }

        #region Lvl
        public virtual void GetXp(double xp)
        {
            Xp += xp;
        }

        public virtual void LvlUp(int lvl)
        {
            Lvl += lvl;
            MaxLife += lvl;
            Damage += 0.02 * lvl;
        }
        #endregion

        public virtual string ShowInfo(LanguagesManager s)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(
                $"[{Name}]\n" +
                $"Lvl: {Lvl}\n" +
                $"Xp: {Xp.ToString("F2", CultureInfo.InvariantCulture)}\n" +
                $"{s.GetSubtitle("MobClass", "life")}: {Life.ToString("F2", CultureInfo.InvariantCulture)}\n" +
                $"{s.GetSubtitle("MobClass", "damage")}: {Damage.ToString("F2", CultureInfo.InvariantCulture)}\n" +
                $"{s.GetSubtitle("MobClass", "weapon")}: {Weapon.Name}\n" +
                $"{s.GetSubtitle("MobClass", "coins")}: {Coins.ToString("F2", CultureInfo.InvariantCulture)}");

            return sb.ToString();
        }
    }
}
