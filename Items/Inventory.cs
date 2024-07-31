using Game.ClassManager;
using Game.Items.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Items
{
    internal class Inventory
    {
        public ItemCreate[] Bag { get; private set; } = new ItemCreate[5];

        public Inventory()
        {
            ColectItem(new Stick());
        }

        public bool ColectItem(ItemCreate item)
        {
            for (int i = 0; i <  Bag.Length; i++)
            {
                if (Bag[i] == null)
                {
                    Bag[i] = item;
                    return true;
                }
            }
            return false;
        }

        public bool DropItem(ItemCreate item)
        {
            for (int i = 0; i < Bag.Length; i++)
            {
                if (Bag[i].Id == item.Id)
                {
                    Bag[i] = null;
                    return true;
                }
            }
            throw new GameException("Item not found.");
        }

        public ItemCreate GetItemIntoBag(ItemCreate item)
        {
            for (int i = 0; i < Bag.Length; i++)
            {
                if (Bag[i].Id == item.Id)
                    return Bag[i];
            }
            throw new GameException("Item not found.");
        }

        public void BagUpgrade()
        {
            ItemCreate[] _oldBag = Bag;
            ItemCreate[] _newBag = new ItemCreate[_oldBag.Length + 5];
            for (int i = 0; i <= _oldBag.Length; i++)
            {
                if (_oldBag[i] != null)
                    _newBag.Append(_oldBag[i]);
            }
        }

        public string CheckBag(LanguagesManager s)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"[{s.GetSubtitle("Status", "myBag")}]");
            for (int i = 0; i < Bag.Length; i++)
            {
                if (Bag[i] != null)
                    sb.AppendLine($"[{i}]: {Bag[i].Name}");
                else
                    sb.AppendLine("--");
            }
            return sb.ToString();
        }
    }
}
