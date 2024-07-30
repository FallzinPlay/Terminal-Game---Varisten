﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Net;
using System.Diagnostics;
using Game.ClassManager;
using Game.Entities;
using Game.Items.Weapons;

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

            //*/

            try
            {
                #region Jogo
                //*
                // criação do jogador
                s.ShowSubtitle(s.GetSubtitle("System", "presentation")); // Pequena apresentação
                Console.Write(">> ");
                string playerName = Console.ReadLine(); // Pega o nome do jogador
                s.ShowSubtitle(s.GetSubtitle("System", "jokeWithName"));
                Player player = new Player(s, playerName);

                // Loop do jogo
                s.ShowSubtitle($"{s.GetSubtitle("System", "wellcome")}\n");
                
                int answer;
                while (Menu.StartMenu(s))
                {
                    do
                    {
                        // Menu
                        answer = Tools.Answer(s,
                            s.GetSubtitle("Menu", "adventure"),
                            Enum.GetValues(typeof(StartActions)).Length);
                        switch ((StartActions)answer)
                        {
                            case StartActions.Exit:
                                s.ShowSubtitle(s.GetSubtitle("System", "leaving").Replace("#PlayerName", player.Name)); // Saindo do jogo
                                break;

                            case StartActions.Adventure:
                                byte totalActions = 10;
                                double randomAction = 4;// Tools.RandomDouble(totalActions);

                                // Mob
                                if (randomAction < totalActions * 0.5d) MobFound(s, R, player);

                                // Merchant
                                else if (randomAction < totalActions * 0.7d) MerchantFound(s, player);

                                // Treasury
                                else if (randomAction < totalActions * 1d) TreasuryFound(s, player);

                                break;

                            case StartActions.PlayerBag:
                                // Mostra a arma do player

                                break;

                            default:
                                continue;
                        }
                    } while (answer > 0 || Menu.GameOver(s, player));
                }

                s.ShowSubtitle(s.GetSubtitle("Me", "thanks"));
                //*/
                #endregion
            }
            catch (Exception ex)
            {
                s.ShowSubtitle("Error: " + ex);
            }
        }
        // !!!!!!!!!!!!!!!! Verificar porquê não está saindo do loop !!!!!!!!!!!!!!
        public static void MobFound(LanguagesManager s, Random r, MobCreate player)
        {
            int answer;
            MobCreate mobFound = Tools.RandomMob(player);
            s.ShowSubtitle(
                s.GetSubtitle("Player", mobFound.Name.ToLower() + "Found") + "\n");
            player.State = MobState.Fighting;
            do
            {
                answer = Tools.Answer(s, s.GetSubtitle("Menu", "mobFound").Replace("#MobName", mobFound.Name), 4);
                switch (answer)
                {
                    case 0:
                        player.TryRunAway();
                        break;

                    case 1:
                        double _damage = player.Attack(mobFound);
                        string _critic = "";
                        if (player.Weapon == null && _damage > player.Damage)
                            _critic = s.GetSubtitle("Combat", "critic");

                        s.ShowSubtitle(
                            s.GetSubtitle("Player", "attack" + mobFound.Name + r.Next(3)));
                        s.ShowSubtitle(
                            s.GetSubtitle("Combat", "getDamage").Replace("#Name", mobFound.Name).Replace("#Damage", _damage.ToString("F2")) +
                            _critic + "\n");
                        break;

                    case 2:
                        s.ShowSubtitle(mobFound.ShowInfo(s));
                        continue;

                    case 3:
                        s.ShowSubtitle(player.ToString());
                        continue;

                    default:
                        InvalidOption(s);
                        continue;
                }

                // Morte do inimigo
                if (mobFound.State == MobState.Death)
                {
                    s.ShowSubtitle(
                        $"{s.GetSubtitle(mobFound.Name, "death")}\n");

                    player.GetXp(mobFound.Xp);
                    player.GetCoins(mobFound.Coins);

                    /* Chance de drop
                    if (mobFound.Weapon != null)
                    {
                        double dropChance = 3d;
                        if (Tools.RandomChance(dropChance))
                        {
                            // Mostra o drop do mob
                            answer = Tools.Answer(s,
                                $"{s.GetSubtitle("Subtitles", "mobDrop").Replace("#1", mobFound.Name).Replace("#2", mobFound.Weapon.Name)}" +
                                $"{s.GetSubtitle("Menu", "noYes")}");

                            switch (answer)
                            {
                                case 0:
                                    // Ignora a arma
                                    s.ShowSubtitle(s.GetSubtitle("Subtitles", "weaponIgnore"));
                                    break;

                                case 1:
                                    // Equipa a arma do inimigo
                                    player.WeaponEquip(mobFound.Weapon);
                                    break;

                                default:
                                    // Mostra o erro de ação invalida
                                    continue;
                            }
                        }
                    }
                    */
                    player.State = MobState.Exploring;
                    Console.WriteLine("Acabou");
                }
                else
                {
                    #region Turno do mob
                    // Chance do inimigo querer fugir
                    if (mobFound.Life < mobFound.MaxLife / 5)
                    {
                        double _escapeChance = 3d;
                        if (Tools.RandomChance(_escapeChance))
                        {
                            // O inimigo está fugindo
                            mobFound.State = MobState.Exploring;
                            s.ShowSubtitle(
                                s.GetSubtitle(mobFound.Name, "runAway"));
                            answer = Tools.Answer(s,
                                $"{s.GetSubtitle("Menu", "mobRunningAway").Replace("#MobName", mobFound.Name)}\n");

                            switch (answer)
                            {
                                case 0:
                                    // Permite a fuga do mob
                                    s.ShowSubtitle(
                                        s.GetSubtitle("Player", $"allow{mobFound.Name}Run") + "\n");
                                    player.State = MobState.Exploring;
                                    break;

                                // Verifica se você consegue evitar a fuga do inimigo ou não
                                case 1:
                                    if (!mobFound.TryRunAway())
                                        s.ShowSubtitle(
                                            s.GetSubtitle("Player", "catch" + mobFound.Name) + "\n");
                                    else
                                    {
                                        s.ShowSubtitle(
                                            s.GetSubtitle("Player", "noCatch" + mobFound.Name) + "\n");
                                        player.State = MobState.Exploring;
                                    }
                                    break;

                                default:
                                    InvalidOption(s);
                                    continue;
                            }
                        }
                    }

                    if (mobFound.State == MobState.Fighting)
                    {
                        // Ataque do inimigo
                        double _damage = mobFound.Attack(player); ;
                        string _critic = "";
                        if (mobFound.Weapon == null && _damage > mobFound.Damage)
                            _critic = s.GetSubtitle("Combat", "critic");

                        s.ShowSubtitle(
                            s.GetSubtitle(mobFound.Name, "attack" + r.Next(3)));
                        s.ShowSubtitle(
                            s.GetSubtitle("Combat", "getDamage").Replace("#Name", player.Name).Replace("#Damage", _damage.ToString("F2")) +
                            _critic + "\n");
                    }
                    #endregion
                }
            }
            while (player.State == MobState.Fighting || !Menu.GameOver(s, player));
        }

        public static void MerchantFound(LanguagesManager s, Player player)
        {
            int answer;
            WeaponCreate weaponFound = new Stick();
            double weaponPrice = Tools.RandomDouble(weaponFound.MaxPrice, weaponFound.MinPrice);

            // Encontra o mercador
            s.ShowSubtitle(s.GetSubtitle("Subtitles", "merchantFound"));
            do
            {
                answer = Tools.Answer(s, s.GetSubtitle("Menu", "merchantFound"));
                switch (answer)
                {
                    case 0:
                        // Ignora o mercador
                        s.ShowSubtitle(s.GetSubtitle("Subtitles", "merchantIgnore"));
                        break;

                    case 1:
                        // Legenda
                        s.ShowSubtitle(
                            $"[{s.GetSubtitle("Titles", "items")}] \n" +
                            $"[{s.GetSubtitle("Subtitles", "myCoins").Replace("#", player.Coins.ToString("F2", CultureInfo.InvariantCulture))}]");

                        answer = Tools.Answer(s,
                            s.GetSubtitle("Merchant", "shop").Replace("#1", weaponFound.Name).Replace("#2", weaponPrice.ToString("F2", CultureInfo.InvariantCulture)),
                            3);
                        switch (answer)
                        {
                            case 0:
                                break;

                            case 1:
                                if (player.Buy(5))
                                {
                                    player.Cure(5);
                                    // Compra e toma a poção
                                    s.ShowSubtitle($"[{s.GetSubtitle("Titles", "life")}] {player.Life.ToString("F2", CultureInfo.InvariantCulture)}\n");
                                }
                                else
                                {
                                    // Moedas insuficientes
                                    s.ShowSubtitle(s.GetSubtitle("Subtitles", "insufficientMoney"));
                                }
                                continue;

                            case 2:

                                if (player.Buy(weaponPrice))
                                {
                                    // Compra e equipa a arma
                                    player.WeaponEquip(weaponFound);
                                    s.ShowSubtitle(s.GetSubtitle("Subtitles", "buyWeapon"));
                                }
                                else
                                {
                                    // Moedas insuficientes
                                    s.ShowSubtitle(s.GetSubtitle("Subtitles", "insufficientMoney"));
                                }
                                continue;

                            default:
                                // Mensagem de erro
                                s.ShowSubtitle(s.GetSubtitle("Error", "invalidAction"));
                                continue;
                        }
                        continue;

                    default:
                        // Mensagem de erro
                        continue;
                }
            }
            while (answer > 0);
        }

        public static void TreasuryFound(LanguagesManager s, MobCreate player)
        {
            int answer;
            double coins = Tools.RandomDouble(20d, 5d); // Sorteia um número
            s.ShowSubtitle(s.GetSubtitle("Subtitles", "treasuryFound")); // Encontra um tesouro

            do
            {
                answer = Tools.Answer(s, s.GetSubtitle("Menu", "noYes"));
                switch (answer)
                {
                    case 0:
                        break;

                    case 1:
                        player.GetCoins(coins);
                        break;

                    default:
                        continue;
                }
                break;
            }
            while (answer > 0);
        }

        public static void InvalidOption(LanguagesManager s)
        {
            s.ShowSubtitle(s.GetSubtitle("Error", "invalidOption"));
        }
    }
}
