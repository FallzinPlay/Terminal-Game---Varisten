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
            Damage = 1.1d;
            MaxLife = 7;
            CriticChance = 1.1d;
            CriticDamage = 1.7d;
            MaxLvl = 10;
            Dodge = 0.8d;
            DropChance = 4.1d;
            EscapeChance = 1.2d;
        }
    }
}
