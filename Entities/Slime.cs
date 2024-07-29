using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.ClassManager;

namespace Game.Entities
{
    internal class Slime : MobCreate
    {
        public Slime()
        {
            Name = "Slime";
            Damage = 2.7d;
            MaxLife = 4;
            CriticChance = 3.5d;
            CriticDamage = 2.7d;
            MaxLvl = 15;
            Dodge = 0.5d;
        }
    }
}
