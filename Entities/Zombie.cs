using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.ClassManager;

namespace Game.Entities
{
    internal class Zombie : MobCreate
    {
        public Zombie()
        {
            Name = "Zombie";
            Damage = 2.2d;
            MaxLife = 7;
            CriticChance = 1.1d;
            CriticDamage = 1.7d;
            MaxLvl = 10;
            Dodge = 0.8d;
        }
    }
}
