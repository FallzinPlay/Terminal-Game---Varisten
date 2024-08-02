using Game.ClassManager;
using Game.Entities;
using Game.Items.Weapons;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Game.Items
{
    public class Inventory
    {
        public List<ItemCreate> Bag { get; private set; } = new List<ItemCreate>();
        public int MaxCapacity {  get; private set; }

        public Inventory(int maxCapacity, params ItemCreate[] item)
        {
            MaxCapacity = maxCapacity;
            foreach (ItemCreate i in item)
                ColectItem(i);
        }

        public bool ColectItem(ItemCreate item)
        {
            if (Bag.Count <= MaxCapacity)
            {
                Bag.Add(item);
                return true;
            }
            return false;
        }

        public void DropItem(ItemCreate item)
        {
            Bag.Remove(item);
        }

        public void BagUpgrade()
        {
            MaxCapacity += 3;
        }

        public string CheckBag(LanguagesManager s)
        {
            StringBuilder sb = new StringBuilder();
            int _count = 0;
            foreach (ItemCreate item in Bag)
            {
                sb.AppendLine($"[{_count+1}]: {item.Name}");
                _count++;
            } 
            for (int i = _count; i < MaxCapacity; i++)
                    sb.AppendLine($"[{i+1}]: --");
            return sb.ToString();
        }

        public ItemCreate GetItemInBag(LanguagesManager s)
        {
            int _index;
            ItemCreate _item = null;
            do
            {
                _index = Tools.Answer(s,
                   $"[{s.GetSubtitle("Status", "items")}]\n" + s.GetSubtitle("Menu", "leave") + "\n" + CheckBag(s),
                   MaxCapacity + 1);

                if (_index > Bag.Count)
                {
                    s.ShowSubtitle(s.GetSubtitle("System", "emptySlot") + "\n");
                    continue;
                }
                if (_index > 0)
                {
                    _index--;
                    _item = Bag[_index];
                }
            } while (_item == null);
            return _item;
        }
    }
}
