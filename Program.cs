using System;
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
            Random R = new Random();

            // Registro de Ids
            EntityRegistry register = new EntityRegistry();
            // Como pegar os objetos por id: WeaponCreate newWeapon = register.GetEntityById<WeaponCreate>(weapon[1].Id);

            // ------------- Choose the game language
            LanguagesManager s = new LanguagesManager();/*
            s.LanguageChoose(
                Tools.Answer(s, "Choose your language: \n0 - English, USA (default) \n1 - Português, BR", s.LanguageOptions.Length));
            // --------------------------------------------------
            
            /*/

            // ------------- Make the classes options

            WeaponCreate[] weapon = new WeaponCreate[]
            {
                new WeaponCreate(register, s, "--", 0d, 0, 0, 0d, 0d),
                new WeaponCreate(register, s, s.GetSubtitle("Weapons", "stick"), 3.2d, 4, 1, 5d, 10d),
                new WeaponCreate(register, s, s.GetSubtitle("Weapons", "woodenSword"), 3.5d, 5, 2, 7d, 12d),
                new WeaponCreate(register, s, s.GetSubtitle("Weapons", "woodenBow"), 4.2d, 6, 3, 10d, 15d),
            };

            // Entidades
            MobCreate[] mob = new MobCreate[]
            {
                // idioma, nome, raça, dano, vida maxima, chace de critico, dano de critico, arma, nivel maximo, esquiva, chance de escapar
                new MobCreate(register, s, s.GetSubtitle("Mobs", "zombie"), s.GetSubtitle("Races", "zombies"), 2.2d, 7, 1.1d, 1.7d, weapon[0], 10, 0.8d, 1.2d),
                new MobCreate(register, s, s.GetSubtitle("Mobs", "skeleton"), s.GetSubtitle("Races", "skeletons"), 3.2d, 5, 2.2d, 2.3d, weapon[3], 12, 2.1d, 1.6d),
                new MobCreate(register, s, s.GetSubtitle("Mobs", "slime"), s.GetSubtitle("Races", "slimes"), 2.7d, 4, 3.5d, 2.7d, weapon[0], 15, 0.5d, 2d),
            };

            // Raças
            MobCreate[] race = new MobCreate[]
            {
                // idioma, nome, raça, dano, vida maxima, chace de critico, dano de critico, arma, nivel, nivel maximo, esquiva, chance de escapar
                new MobCreate(register, s, null, s.GetSubtitle("Races", "humans"), 2.4d, 10, 1.7d, 1.5d, weapon[0], 10, 1.7d, 1.5d),
                new MobCreate(register, s, null, s.GetSubtitle("Races", "dwarves"), 3d, 8, 1.3d, 1.6d, weapon[0], 7, 1.2d, 1.3d),
            };
            // --------------------------------------------------

            //

            // ------------- Make the race descriptions
            race[0].SetDescription($"\n[{race[0].Race.ToUpper()}]:\n" + s.GetSubtitle("Descriptions", "humans"));
            race[1].SetDescription($"\n[{race[1].Race.ToUpper()}]:\n" + s.GetSubtitle("Descriptions", "dwarves"));
            // --------------------------------------------------

            //
            
            #region Jogo
            //*
            if (s.Chose)
            {
                // criação do jogador
                MobCreate player = Menu.CreatePlayer(s, race);
                player.Player = true;
                Adventure adventure = new Adventure(register, s, player, mob, weapon);

                // Loop do jogo
                Menu.StartMenu(s);
                s.ShowSubtitle($"{s.GetSubtitle("Subtitles", "playMenu")}\n");
                int answer;
                do
                {
                    // Menu
                    answer = Tools.Answer(s,
                        s.GetSubtitle("Menu", "options"),
                        Enum.GetValues(typeof(StartActions)).Length);
                    switch ((StartActions)answer)
                    {
                        case StartActions.Exit:
                            s.ShowSubtitle(s.GetSubtitle("Subtitles", "leaving")); // Saindo do jogo
                            break;

                        case StartActions.Adventure:

                            int randomAction = R.Next(10);

                            // Weapon
                            if (randomAction <= 2) adventure.WeaponFound();

                            // Mob
                            else if (randomAction <= 7) adventure.MobFound();

                            // Merchant
                            else if (randomAction <= 8) adventure.MerchantFound();

                            // Treasury
                            else if (randomAction <= 10) adventure.TreasuryFound();

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
                            s.ShowSubtitle(player.Description.ToString() + "\n");
                            break;

                        default:
                            Tools.InvalidAction(s);
                            continue;
                    }
                    if (Menu.GameOver(s, player)) break;
                } while (answer > 0);
            }

            s.ShowSubtitle(s.GetSubtitle("Subtitles", "thanksForPlayed"));
            //*/
            #endregion
        }
    }
}
