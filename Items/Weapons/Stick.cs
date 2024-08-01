using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Game.ClassManager;
using static System.Net.Mime.MediaTypeNames;

namespace Game.Items.Weapons
{
    internal class Stick : WeaponCreate
    {
        public Stick()
        {
            Name = "Stick";
            Damage = 0.5d;
            MaxCondition = 4;
            Condition = MaxCondition;
            NecessaryLvl = 1;
            Price = 3.50d;
        }
    }
}
