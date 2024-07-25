using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static MobCreate CreatePlayer(LanguagesManager s, MobCreate[] race)
        {
            string playerName;
            s.ShowSubtitle(s.GetSubtitle("Subtitles", "setName")); // Pequena apresentação
            Console.Write(">> ");
            playerName = Console.ReadLine(); // Pega o nome do jogador
            s.ShowSubtitle(s.GetSubtitle("Subtitles", "greatChose"));
            MobCreate player = RaceChoose(s, race, playerName); // Cria o jogador
            s.ShowSubtitle($"{s.GetSubtitle("Subtitles", "wellcome")}\n");
            return player;
        }

        public static void GameOver(LanguagesManager s)
        {
            s.ShowSubtitle(s.GetSubtitle("Menu", "gameOver"));
        }

        public static MobCreate RaceChoose(LanguagesManager s, MobCreate[] race, string name)
        {

            int choose;
            MobCreate raceChose = null;
            while (raceChose == null)
            {
                s.ShowSubtitle(s.GetSubtitle("Subtitles", "raceChoose"));
                s.ShowSubtitle($"\n[{s.GetSubtitle("Titles", "races")}]");
                int max = race.Length;
                string options = "";
                for (int i = 0; i < max; i++)
                {
                    options += $"{i}: {race[i].Race}\n";
                }
                choose = Tools.Answer(s, options, max);

                race[choose].Name = name;
                s.ShowSubtitle($"{race[choose]}\n{race[choose].Description}\n");

                switch (Tools.Answer(s, s.GetSubtitle("Menu", "returnConfirm")))
                {
                    case 0:
                        continue;

                    case 1:
                        raceChose = race[choose];
                        break;

                    default:
                        s.ShowSubtitle(s.GetSubtitle("Error", "invalidAction"));
                        continue;
                }
            }
            return raceChose;
        }
    }
}
