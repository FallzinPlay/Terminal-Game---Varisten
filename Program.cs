using System;
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
                if (s.Chose)
                {
                    // criação do jogador
                    s.ShowSubtitle(s.GetSubtitle("System", "presentation")); // Pequena apresentação
                    Console.Write(">> ");
                    string playerName = Console.ReadLine(); // Pega o nome do jogador
                    s.ShowSubtitle(s.GetSubtitle("System", "jokeWithName"));
                    Player player = new Player(s, playerName);

                    // Loop do jogo
                    Menu.StartMenu(s);
                    s.ShowSubtitle($"{s.GetSubtitle("System", "wellcome")}\n");
                    s.ShowSubtitle($"{s.GetSubtitle("Menu", "Start")}\n");
                    // Fazer as opções jogar e sair
                    int answer;
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

                                int randomAction = R.Next(10);

                                // Weapon
                                if (randomAction <= 2) WeaponFound(s, player);

                                // Mob
                                else if (randomAction <= 7) MobFound(s, player);

                                // Merchant
                                else if (randomAction <= 8) MerchantFound(s, player);

                                // Treasury
                                else if (randomAction <= 10) TreasuryFound(s, player);

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
                s.ShowSubtitle("Error: " + ex.Message);
            }
        }

        public static void WeaponFound(LanguagesManager s, MobCreate player)
        {
            int answer;
            WeaponCreate weaponFound = new Stick();
            s.ShowSubtitle($"{s.GetSubtitle("Subtitles", "weaponFound").Replace("#", weaponFound.Name)}"); // Encontra uma arma

            do
            {
                // Menu
                answer = Tools.Answer(s, s.GetSubtitle("Menu", "weaponFound"), 3);
                switch (answer)
                {
                    case 0:
                        s.ShowSubtitle(s.GetSubtitle("Subtitles", "dontEquipWeapon")); // Abandona a arma
                        break;

                    case 1:
                        // Mostra a arma do jogador (se tiver) e a arma encontrada
                        if (player.Weapon != null)
                        {
                            // Compara a arma que já tem com a que encontrou
                            s.ShowSubtitle(
                                $"[{s.GetSubtitle("Titles", "myWeapon")}]\n" +
                                player.Weapon);
                            s.ShowSubtitle(
                                $"[{s.GetSubtitle("Titles", "weaponFound")}]\n" +
                                weaponFound);
                        }
                        else
                        {
                            // Diz que não tem arma e avalia a encontrada
                            s.ShowSubtitle(
                                $"{s.GetSubtitle("Subtitles", "haveNoWeapon")}\n" +
                                weaponFound);
                        }
                        continue;

                    case 2:
                        // Equipa a arma
                        player.WeaponEquip(weaponFound);
                        break;

                    default:
                        // Mensagem de erro
                        continue;
                }
                break;
            }
            while (answer > 0);
        }

        public static void MobFound(LanguagesManager s, MobCreate player)
        {
            int answer;
            MobCreate mobFound = new Zombie();
            s.ShowSubtitle(s.GetSubtitle("Subtitles", "mobFound").Replace("#", mobFound.Name)); // Encontra um mob
            player.State = MobState.Fighting;
            do
            {
                // Menu
                answer = Tools.Answer(s, s.GetSubtitle("Menu", "mobFound"), 4);

                // Escolha
                switch (answer)
                {
                    case 0:
                        #region Chance de escapar
                        // Sistema de chances de conseguir escapar
                        if (Tools.RandomChance(player.EscapeChance))
                        {
                            s.ShowSubtitle(s.GetSubtitle("Subtitles", "canRunAway").Replace("#", mobFound.Name)); // Foge do mob
                            player.State = MobState.Exploring;
                            break;
                        }
                        else
                        {
                            s.ShowSubtitle(s.GetSubtitle("Subtitles", "cantRunAway")); // Não consegue fugir do mob
                        }
                        #endregion
                        break;

                    case 1:
                        s.ShowSubtitle("\n" + mobFound + "\n");
                        continue;

                    case 2:
                        player.Atack(mobFound);
                        mobFound.State = MobState.Fighting;
                        break;

                    case 3:
                        s.ShowSubtitle(player.ToString());
                        continue;

                    default:
                        // Erro de ação invalida

                        continue;
                }

                // Morte do inimigo
                if (mobFound.State == MobState.Death)
                {
                    s.ShowSubtitle($"{s.GetSubtitle("Combat", "mobDefeat").Replace("#", mobFound.Name)}\n");

                    // Coleta o Xp
                    player.GetXp(mobFound.Xp);
                    // Coleta as moedas
                    player.GetCoins(mobFound.Coins);

                    #region Chance de drop
                    // Chance do mob droppar a arma que ele está usando
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
                    player.State = MobState.Exploring;
                    #endregion
                }
                else
                {
                    #region Turno do mob
                    // Chance do inimigo querer fugir
                    if (mobFound.Life < mobFound.MaxLife / 5)
                    {
                        if (Tools.RandomChance(mobFound.EscapeChance))
                        {
                            // O inimigo está fugindo
                            mobFound.State = MobState.Exploring;
                            answer = Tools.Answer(s,
                                $"{s.GetSubtitle("Subtitles", "mobRunningAway").Replace("#", mobFound.Name)}\n" +
                                $"{s.GetSubtitle("Menu", "allowRunToward")}");

                            switch (answer)
                            {
                                case 0:
                                    // Permite a fuga do mob
                                    s.ShowSubtitle(s.GetSubtitle("Subtitles", "allowMobRun"));
                                    break;

                                // Verifica se você consegue evitar a fuga do inimigo ou não
                                case 1:
                                    if (Tools.RandomChance(mobFound.EscapeChance))
                                    {
                                        // O mob escapa
                                        s.ShowSubtitle(s.GetSubtitle("Subtitles", "cantCatchMob").Replace("#", mobFound.Name));
                                    }
                                    else
                                    {
                                        // Consegue capturar o mob
                                        s.ShowSubtitle(s.GetSubtitle("Subtitles", "canCatchMob"));
                                    }
                                    break;

                                default:
                                    continue;
                            }
                        }
                    }

                    if (mobFound.State == MobState.Fighting)
                    {
                        // Ataque do inimigo
                        mobFound.Atack(player);
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
    }
}
