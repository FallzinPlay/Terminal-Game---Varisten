using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.ClassManager;

namespace Game
{
    internal class Menu
    {
        public static bool StartMenu(LanguagesManager s)
        {
            int n = Tools.Answer(s, s.GetSubtitle("Menu", "start"));
            if (n == 1) return true;
            return false;
        }

        public static bool GameOver(LanguagesManager s, MobCreate player)
        {
            if (player.State == MobState.Death)
            {
                s.ShowSubtitle(s.GetSubtitle("Subtitles", "gameOver"));
                return true;
            }
            return false;
        }
    }
}
