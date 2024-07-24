﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Net;
using System.Diagnostics;

namespace Game
{
    internal class Program
    {
        static void Main(string[] args)
        {

            // ------------- Choose the game language
            LanguagesManager s = new LanguagesManager();
            s.LanguageChoose(
                Tools.Answer(s, "Choose your language: \n0 - English, USA (default) \n1 - Português, BR", s.LanguageOptions.Length));
            // --------------------------------------------------
            
            //

            // ------------- Make the classes options

            WeaponCreate[] weapon = new WeaponCreate[]
            {
                new WeaponCreate(s, "--", 0d, 0, 0, 0d, 0d),
                new WeaponCreate(s, s.GetSubtitle("Weapons", "stick"), 3.2d, 4, 1, 5d, 10d),
                new WeaponCreate(s, s.GetSubtitle("Weapons", "woodenSword"), 3.5d, 5, 2, 7d, 12d),
                new WeaponCreate(s, s.GetSubtitle("Weapons", "woodenBow"), 4.2d, 6, 3, 10d, 15d),
            };

            // Entidades
            MobCreate[] mob = new MobCreate[]
            {
                // idioma, nome, raça, vida, dano, vida maxima, chace de critico, dano de critico, arma, nivel, nivel maximo, esquiva, chance de escapar
                new MobCreate(s, s.GetSubtitle("Mobs", "zombie"), s.GetSubtitle("Races", "zombies"), 0, 2.2d, 7, 1.1d, 1.7d, weapon[0], weapon[0].NecessaryLvl, 10, 0.8d, 1.2d),
                new MobCreate(s, s.GetSubtitle("Mobs", "skeleton"), s.GetSubtitle("Races", "skeletons"), 0, 3.2d, 5, 2.2d, 2.3d, weapon[3], weapon[3].NecessaryLvl, 12, 2.1d, 1.6d),
                new MobCreate(s, s.GetSubtitle("Mobs", "slime"), s.GetSubtitle("Races", "slimes"), 0, 2.7d, 4, 3.5d, 2.7d, weapon[0], weapon[0].NecessaryLvl, 15, 0.5d, 2d),
            };

            // Raças
            MobCreate[] race = new MobCreate[]
            {
                // idioma, nome, raça, vida, dano, vida maxima, chace de critico, dano de critico, arma, nivel, nivel maximo, esquiva, chance de escapar
                new MobCreate(s, null, s.GetSubtitle("Races", "humans"), 10d, 2.4d, 10, 1.7d, 1.5d, weapon[0], 1, 10, 1.7d, 1.7d),
                new MobCreate(s, null, s.GetSubtitle("Races", "dwarves"), 8d, 3d, 8, 1.3d, 1.6d, weapon[0], 1, 7, 1.2d, 1.3d),
            };
            // --------------------------------------------------

            //

            // ------------- Make the race descriptions
            race[0].SetDescription($"\n[{race[0].Race.ToUpper()}]:\n" + s.GetSubtitle("Descriptions", "humans"));
            race[1].SetDescription($"\n[{race[1].Race.ToUpper()}]:\n" + s.GetSubtitle("Descriptions", "dwarves"));
            // --------------------------------------------------

            //

            #region Jogo
            int answer;
            if (s.Chose)
            {
                // criação do jogador
                MobCreate player = Menu.CreatePlayer(s, race);
                player.Player = true;
                Adventure adventure = new Adventure(s, player, mob, weapon);

                // Loop do jogo
                Menu.StartMenu(s);
                bool playing = true;
                while (playing)
                {
                    // Acaba o jogo se o jogador morrer
                    if (player.Life <= 0)
                    {
                        Menu.GameOver(s);
                        playing = false;
                    }

                    // Menu
                    answer = Tools.Answer(s, s.ShowSubtitle(s.GetSubtitle("Menu", "options")), Enum.GetValues(typeof(StartActions)).Length);

                    // Sai para o menu principal
                    if (answer == 0)
                    {
                        s.ShowSubtitle(s.GetSubtitle("Subtitles", "leaving")); // Saindo do jogo
                        playing = false;
                    }
                    switch ((StartActions)answer)
                    {
                        case StartActions.Exit:
                            break;

                        case StartActions.Adventure:

                            answer = Tools.Random().Next(10);

                            // Weapon
                            if (answer <= 3) adventure.WeaponFound();

                            // Mob
                            else if (answer <= 7) adventure.MobFound();

                            // Merchant
                            else if (answer <= 9) adventure.MerchantFound();

                            // Treasury
                            else if (answer <= 10) adventure.TreasuryFound();

                            else
                            {
                                // Mostra ação invalida
                                Tools.InvalidAction(s);
                            }
                            break;

                        case StartActions.PlayerStatus:
                            s.ShowSubtitle(player.ToString());
                            break;

                        case StartActions.PlayerBag:
                            // Mostra a arma do player
                            if (player.WeaponEquiped != false)
                            {
                                s.ShowSubtitle(player.Weapon.ToString());
                            }
                            else
                            {
                                // Diz que não tem arma
                                s.ShowSubtitle(s.GetSubtitle("Subtitles", "haveNoWeapon"));
                            }
                            break;

                        case StartActions.RaceDescription:
                            s.ShowSubtitle(player.Description.ToString());
                            break;

                        default:
                            Tools.InvalidAction(s);
                            break;
                    }
                }
            }

            s.ShowSubtitle(s.GetSubtitle("Subtitles", "thanksForPlayed"));
            #endregion
        }
    }
}
