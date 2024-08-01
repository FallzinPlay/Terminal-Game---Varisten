using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.ClassManager;

namespace Game.Items.Weapons
{
    internal class WoodenSword : WeaponCreate
    {
        public WoodenSword()
        {
            Name = "Wooden Sword";
            Damage = 1.7d;
            MaxCondition = 5;
            Condition = MaxCondition;
            NecessaryLvl = 2;
            Price = 5.35d;
        }
    }
}
