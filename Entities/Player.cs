using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.ClassManager;
using Game.Items;

namespace Game.Entities
{
    internal class Player : MobCreate
    {
        public Inventory Bag { get; private set; } = new Inventory();

        private LanguagesManager _language;

        public Player(LanguagesManager language, string name)
        {
            Name = name;
            MaxLvl = 20;
            MaxLife = 10;
            Life = MaxLife;
            Damage = 1.7d;
            Dodge = 1.2d;
            CriticChance = 1.5d;
            CriticDamage = 1.5d;
            Lvl = 1;
            NextLvlXp = 10;
            State = MobState.Exploring;
            EscapeChance = 5d;

            _language = language;
        }
        // !!!!!!!!!!!!!!!!!!! Fazer a verificação da arma !!!!!!!!!!!!!!!!!!!!
        public void BagVerify()
        {
            _language.ShowSubtitle(ToString());
            ItemCreate[] _bag = Bag.Bag;
            int _item;
            do
            {
                _item = Tools.Answer(_language,
                    Bag.CheckBag(_language) +  _language.GetSubtitle("Menu", "leave").Replace("#N", _bag.Length.ToString()),
                    _bag.Length + 1);

                if (_item == _bag.Length) break;
                if (_bag[_item] == null)
                {
                    _language.ShowSubtitle(_language.GetSubtitle("Player", "emptySlot"));
                    continue;
                }

                int _action = Tools.Answer(_language,
                _bag[_item].ShowInfo(_language) + "\n" + _language.GetSubtitle("Menu", "checkingBag"),
                3);
                switch (_action)
                {
                    case 0:
                        break;

                    case 1:
                        if (_bag[_item] is WeaponCreate)
                        {
                            WeaponEquip(_bag[_item] as WeaponCreate);
                            Bag.DropItem(_bag[_item]);
                        }
                        break;

                    case 2:
                        Bag.DropItem(_bag[_item]);
                        break;
                }
                break;
            } while (_item != _bag.Length);
        }

        public sealed override void GetDamage(double damage)
        {
            base.GetDamage(damage);
            if (Life < Life / 5)
                _language.ShowSubtitle(
                    _language.GetSubtitle("Player", "dying"));
        }

        public sealed override double SetDamage()
        {
            double _damage = base.SetDamage();
            return _damage;
        }

        public sealed override bool TryRunAway()
        {
            bool _escape = base.TryRunAway();
            if (_escape)
                _language.ShowSubtitle(
                    _language.GetSubtitle("Player", "escape") + "\n");
            else
                _language.ShowSubtitle(
                    _language.GetSubtitle("Player", "noEscape") + "\n");
            return _escape;
        }

        public sealed override bool WeaponEquip(WeaponCreate weapon)
        {
            if (Lvl >= weapon.NecessaryLvl)
            {
                base.WeaponEquip(weapon);
                return true;
            }
            return false;
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

        public sealed override void GetCoins(double coins)
        {
            base.GetCoins(coins);
            _language.ShowSubtitle(
                _language.GetSubtitle("Player", "getCoins").Replace("#Coins", Coins.ToString("F2", CultureInfo.InvariantCulture)) + "\n");
        }

        public sealed override void GetXp(double xp)
        {
            base.GetXp(xp);
            if (Xp >= NextLvlXp)
                LvlUp();
            _language.ShowSubtitle(
                _language.GetSubtitle("Player", "getXp").Replace("#Xp", Xp.ToString("F2", CultureInfo.InvariantCulture)));
        }

        public sealed override void LvlUp(int lvl = 1)
        {
            base.LvlUp(Lvl += lvl);
            Cure(MaxLife);
            Xp -= NextLvlXp;
            NextLvlXp = Lvl * 42;

            _language.ShowSubtitle(
                _language.GetSubtitle("Player", "lvlUp"));
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(
                $"[{Name}]\n" +
                $"Lvl: {Lvl}\n" +
                $"Xp: {Xp.ToString("F2", CultureInfo.InvariantCulture)}xp\n" +
                $"{_language.GetSubtitle("Status", "necessaryXp")}: {NextLvlXp}\n" +
                $"{_language.GetSubtitle("Status", "maxLife")}: {MaxLife}\n" +
                $"{_language.GetSubtitle("Status", "life")}: {Life.ToString("F2", CultureInfo.InvariantCulture)}\n" +
                $"{_language.GetSubtitle("Status", "damage")}: {Damage.ToString("F2", CultureInfo.InvariantCulture)}\n" +
                $"{_language.GetSubtitle("Status", "dodge")}: {Dodge.ToString("F2", CultureInfo.InvariantCulture)}\n" +
                $"{_language.GetSubtitle("Status", "criticChance")}: {CriticChance.ToString("F2", CultureInfo.InvariantCulture)}\n" +
                $"{_language.GetSubtitle("Status", "criticDamage")}: {CriticDamage.ToString("F2", CultureInfo.InvariantCulture)}\n" +
                $"Coins: {Coins.ToString("F2", CultureInfo.InvariantCulture)} coins");

            if (Weapon != null)
                sb.AppendLine($"{_language.GetSubtitle("Status", "weapon")}: {Weapon.Name}");

            return sb.ToString();
        }
    }
}
