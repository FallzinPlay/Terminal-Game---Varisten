using Game.ClassManager;
using Game.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Entities
{
    public class Merchant : MobCreate
    {
        public Inventory Shop { get; private set; }

        public Merchant(EntityRegistry register)
        {
            Name = "Merchant";
            Damage = 2.3d;
            MaxLife = 12;
            CriticChance = 1.7d;
            CriticDamage = 1.9d;
            MaxLvl = 15;
            Dodge = 1.0d;
            DropChance = 2.1d;
            EscapeChance = 2.0d;

            int _capacity = 5;
            ItemCreate[] _item = new ItemCreate[_capacity];
            for (int i = 0; i < _capacity; i++)
                _item[i] = Tools.RandomWeapon(register);
            Shop = new Inventory(_capacity, _item);
        }

        public void ShowItem(LanguagesManager s, int index, Player player)
        {
            int _actionInBag;
            List<ItemCreate> _bag = Shop.Bag;
            do
            {
                _actionInBag = Tools.Answer(s,
                _bag[index].ShowInfo(s) + "\n" + s.GetSubtitle("Menu", "buyingItem"),
                3);
                switch (_actionInBag)
                {
                    case 0:
                        break;

                    case 1:
                        if (player.Buy(_bag[index].Price))
                            Sell(_bag[index].Price);
                        break;

                    default:
                        s.ShowSubtitle(s.GetSubtitle("Error", "invalidOption"));
                        continue;
                }
                break;
            } while (_actionInBag > 0);
        }
    }
}
