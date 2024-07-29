using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.ClassManager;

namespace Game.Entities
{
    internal class Player : MobCreate
    {
        public Player(string name)
        {
            Name = name;
            MaxLvl = 20;
            MaxLife = 10;
            Life = MaxLife;
            Damage = 2.7d;
            Dodge = 1.2d;
            CriticChance = 1.5d;
            CriticDamage = 1.5d;
            Lvl = 1;
            NextLvlXp = 10;
            State = MobState.Exploring;
        }
    }
}
