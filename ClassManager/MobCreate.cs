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
using Game.Entities;

namespace Game.ClassManager
{
    public abstract class MobCreate : Identifier
    {
        public string Name { get; set; }
        public string Description { get; protected set; }
        public int Lvl { get; protected set; } = 1;
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
        public double DropChance {  get; protected set; }
        public MobState State { get; set; } = MobState.Exploring;
        public WeaponCreate Weapon { get; protected set; }


        // Chance de critico
        public double CriticCheck(double damage)
        {
            if (Tools.RandomChance(CriticChance))
                return damage *= CriticDamage;
            return damage;
        }

        // Esquiva
        public bool DodgeCheck()
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
            else
                State = MobState.Fighting;
        }

        // Ataque
        public double SetDamage()
        {
            return Tools.RandomDouble(Damage, Damage / 2);
        }

        // Gera uma chance de tentar fugir
        public virtual bool TryRunAway()
        {
            if (Tools.RandomChance(EscapeChance))
            {
                State = MobState.Exploring;
                return true;
            }
            return false;
        }

        // Equipa a arma
        public virtual bool WeaponEquip(WeaponCreate weapon)
        {
            Weapon = weapon;
            Damage += weapon.Damage;
            return true;
        }

        // Desequipa a arma
        public virtual void WeaponUnequip()
        {
            Damage -= Weapon.Damage;
            Weapon = null;
        }

        // Cura
        public virtual void Cure(double life)
        {
            Life += life;
        }

        // Coleta moedas
        public virtual void GetCoins(double coins)
        {
            Coins += coins;
        }

        // Coleta xp
        public virtual void GetXp(double xp)
        {
            Xp += xp;
        }

        // Upa de nivel
        public virtual void LvlUp(int lvl)
        {
            Lvl = lvl;
            MaxLife += lvl;
            Damage += 0.02 * lvl;
        }

        // Compra
        public bool Buy(double price)
        {
            if (price > Coins)
                return false;

            Coins = Math.Round(Coins, 2);
            price = Math.Round(price, 2);
            Coins -= price;
            return true;
        }

        // Mostra o status
        public virtual string ShowInfo(LanguagesManager s)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(
                $"[{Name}]\n" +
                $"Lvl: {Lvl}\n" +
                $"Xp: {Xp.ToString("F2", CultureInfo.InvariantCulture)}\n" +
                $"{s.GetSubtitle("Status", "life")}: {Life.ToString("F2", CultureInfo.InvariantCulture)}\n" +
                $"{s.GetSubtitle("Status", "damage")}: {Damage.ToString("F2", CultureInfo.InvariantCulture)}\n" +
                $"Coins: {Coins.ToString("F2", CultureInfo.InvariantCulture)}");

            if (Weapon != null) sb.AppendLine($"{s.GetSubtitle("Status", "weapon")}: {Weapon.Name}");

            return sb.ToString();
        }
    }
}
