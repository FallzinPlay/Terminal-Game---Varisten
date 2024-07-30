using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.ClassManager;

namespace Game.Items.Weapons
{
    internal class WoodenBow : WeaponCreate
    {
        public WoodenBow()
        {
            Name = "Wooden Bow";
            Damage = 2.1d;
            MaxCondition = 6;
            Condition = MaxCondition;
            NecessaryLvl = 3;
            MinPrice = 10d;
            MaxPrice = 15d;
        }
    }
}
