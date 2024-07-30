using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.ClassManager;
using Game.Items.Weapons;

namespace Game.Entities
{
    internal class Skeleton : MobCreate
    {
        public Skeleton()
        {
            Name = "Skeleton";
            Damage = 0.9d;
            MaxLife = 5;
            CriticChance = 2.2d;
            CriticDamage = 2.3d;
            MaxLvl = 12;
            Dodge = 2.1d;

            WeaponEquip(new WoodenBow());
        }
    }
}
