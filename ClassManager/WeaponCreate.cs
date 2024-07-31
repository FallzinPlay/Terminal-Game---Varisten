using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Game.ClassManager;

namespace Game
{
    public abstract class WeaponCreate : ItemCreate
    {
        public double Damage { get; protected set; }
        public int NecessaryLvl { get; protected set; }
        public int Condition { get; set; }
        public int MaxCondition { get; protected set; }

        private static readonly Random R = new Random();

        public void Erode(bool randomErode = false)
        {
            if (randomErode) Condition = R.Next(1, MaxCondition);
            else Condition--;
        }

        public override string ShowInfo(LanguagesManager s)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(
                $"{s.GetSubtitle("Status", "damage")}: {Damage.ToString("F2", CultureInfo.InvariantCulture)}\n" +
                $"{s.GetSubtitle("Status", "condition")}: {Condition}\n" +
                $"{s.GetSubtitle("Status", "necessaryLvl")}: {NecessaryLvl}\n");
            return base.ShowInfo(s) + sb.ToString();
        }
    }
}
