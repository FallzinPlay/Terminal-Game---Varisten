using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Globalization;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Game.ClassManager;
using Game.Items;
using Game.Items.Weapons;

namespace Game.Entities
{
    public class Player : MobCreate
    {
        public Inventory Bag { get; private set; } = new Inventory(5, new Stick() as ItemCreate);

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
            Coins = 5d;
            
            _language = language;
        }

        public void BagVerify()
        {
            int _action;
            int _actionInWeapon;
            int _actionInItem;
            ItemCreate _item;
            string _invalidOption = _language.GetSubtitle("Player", "invalidOption");
            do
            {
                _action = Tools.Answer(_language,
                    ToString() + "\n" + _language.GetSubtitle("Menu", "inventory"),
                    3);
                switch(_action)
                {
                    case 0:
                        break;

                    case 1:
                        #region Weapon
                        if (Weapon != null)
                        {
                            _actionInWeapon = Tools.Answer(_language,
                                Weapon.ShowInfo(_language) + _language.GetSubtitle("Menu", "weaponCheck"),
                                3);
                            switch (_actionInWeapon)
                            {
                                case 0:
                                    break;

                                case 1:
                                    Bag.ColectItem(Weapon);
                                    WeaponUnequip();
                                    break;

                                case 2:
                                    WeaponUnequip();
                                    break;

                                default:
                                    _language.ShowSubtitle(_invalidOption);
                                    continue;
                            }
                        }
                        else
                            _language.ShowSubtitle(_language.GetSubtitle("Player", "noWeapon"));
                        #endregion
                        continue;

                    case 2:
                        _item = Bag.GetItemInBag(_language);
                        _actionInItem = Tools.Answer(_language,
                            _item.ShowInfo(_language) + "\n" + _language.GetSubtitle("Menu", "checkingItems"),
                            3);
                        switch (_actionInItem)
                        {
                            case 0:
                                break;

                            case 1:
                                if (_item is WeaponCreate)
                                    WeaponEquip(_item as WeaponCreate);
                                Bag.DropItem(_item);
                                break;

                            case 2:
                                Bag.DropItem(_item);
                                break;

                            default:
                                _language.ShowSubtitle(_invalidOption);
                                continue;
                        }
                        break;

                    default:
                        _language.ShowSubtitle(_invalidOption);
                        continue;
                }
                break;
            } while (true);
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
                _language.ShowSubtitle(
                    _language.GetSubtitle("System", "weaponEquip") +
                    "\n");
                return true;
            }
            return false;
        }

        public sealed override void WeaponUnequip()
        {
            base.WeaponUnequip();
            _language.ShowSubtitle(
                    _language.GetSubtitle("System", "weaponUnequip") +
                    "\n");
        }

        public sealed override void Cure(double life)
        {
            if (Life + life >= MaxLife)
                throw new GameException(_language.GetSubtitle("Player", "fullLife"));

            base.Cure(life);
        }

        public sealed override void GetCoins(double coins)
        {
            base.GetCoins(coins);
            _language.ShowSubtitle($"(+${coins} coins)");
        }

        public sealed override void GetXp(double xp)
        {
            base.GetXp(xp);
            if (Xp >= NextLvlXp)
                LvlUp();
            _language.ShowSubtitle($"(+{xp}Xp)");
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
