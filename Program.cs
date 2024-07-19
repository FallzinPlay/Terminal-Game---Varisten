using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Net;
using Game.Classes;
using System.Diagnostics;

namespace Game
{
    internal class Program
    {
        static void Main(string[] args)
        {

            #region Escolha de idioma
            LanguagesManager s = new LanguagesManager();
            s.ShowSubtitle(
                "Choose your language:\n" +
                "0 - English, USA (default)\n" +
                "1 - Português, BR");
            int max_languages = s.LanguageOptions.Length - 1;
            s.LanguageChoose(ByteAnswer((byte)max_languages));// Enquanto não for uma opção valida ele continuará pedindo para escolher
            #endregion

            #region Configurações de classes

            Random random = new Random();
            int randomChoose;

            // Armas
            Weapon[] weapons = new Weapon[]
            {
            // idioma, nome, dano, condição, nivel necessario, preço min, preço max
            new Weapon(s, "--", 0d, 0, 0, 0d, 0d),
            new Weapon(s, s.GetSubtitle("Weapons", "stick"), 3.2d, 4, 1, 5d, 10d),
            new Weapon(s, s.GetSubtitle("Weapons", "woodenSword"), 3.5d, 5, 2, 7d, 12d),
            new Weapon(s, s.GetSubtitle("Weapons", "woodenBow"), 4.2d, 6, 3, 10d, 15d)
            };
            Weapon weaponFound;


            // Entidades
            Mob[] mobs = new Mob[]
            {
                // idioma, nome, raça, vida, dano, vida maxima, chace de critico, dano de critico, arma, nivel, nivel maximo, esquiva, chance de escapar
                new Mob(s, s.GetSubtitle("Mobs", "zombie"), s.GetSubtitle("Races", "zombies"), 0, 2.2d, 7, 1.1d, 1.7d, weapons[0], weapons[0].NecessaryLvl, 10, 0.8d, 1.2d),
                new Mob(s, s.GetSubtitle("Mobs", "skeleton"), s.GetSubtitle("Races", "skeletons"), 0, 3.2d, 5, 2.2d, 2.3d, weapons[3], weapons[3].NecessaryLvl, 12, 2.1d, 1.6d),
                new Mob(s, s.GetSubtitle("Mobs", "slime"), s.GetSubtitle("Races", "slimes"), 0, 2.7d, 4, 3.5d, 2.7d, weapons[0], weapons[0].NecessaryLvl, 15, 0.5d, 2d),
            };
            Mob mobFound;

            // Raças
            Mob[] race = new Mob[]
            {
                // idioma, nome, raça, vida, dano, vida maxima, chace de critico, dano de critico, arma, nivel, nivel maximo, esquiva, chance de escapar
                new Mob(s, null, s.GetSubtitle("Races", "humans"), 10d, 2.4d, 10, 1.7d, 1.5d, weapons[0], 1, 10, 1.7d, 1.7d),
                new Mob(s, null, s.GetSubtitle("Races", "dwarves"), 8d, 3d, 8, 1.3d, 1.6d, weapons[0], 1, 7, 1.2d, 1.3d),
            };

            #endregion

            #region Descrição das raças
            race[0].SetDescription($"\n[{race[0].Race.ToUpper()}]:\n" + s.GetSubtitle("Descriptions", "humans"));
            race[1].SetDescription($"\n[{race[1].Race.ToUpper()}]:\n" + s.GetSubtitle("Descriptions", "dwarves"));
            #endregion

            #region Jogo
            byte answer;
            if (s.Chose)
            {
                // criação do jogador
                Mob player = CreatePlayer(s, race);

                // Loop do jogo
                while (Menu(s))
                {
                    // Acaba o jogo se o jogador morrer
                    if (player.Life <= 0)
                    {
                        GameOver(s);
                        break;
                    }

                    s.ShowSubtitle(s.GetSubtitle("Menu", "options")); // Menu
                    answer = ByteAnswer((byte)Enum.GetValues(typeof(GameActions)).Length);

                    // Sai para o menu principal
                    if (answer == 0)
                    {
                        s.ShowSubtitle(s.GetSubtitle("Subtitles", "leaving")); // Saindo do jogo
                        break;
                    }
                    switch ((GameActions)answer)
                    {
                        case GameActions.Exit:
                            break;

                        case GameActions.Adventure:

                            #region Adventure

                            answer = (byte)random.Next(10);
                            #region Weapon
                            if (answer <= 3)
                            {

                                randomChoose = random.Next(1, weapons.Length); // Sorteia um número
                                weaponFound = weapons[randomChoose]; // Pega a arma de acordo com o número sorteado
                                weaponFound.Condition = random.Next(1, weaponFound.MaxCondition); // Determina a condição da arma aleatoriamente

                                s.ShowSubtitle($"{s.GetSubtitle("Subtitles", "weaponFound").Replace("#", weaponFound.Name)}"); // Encontra uma arma

                                while (true)
                                {
                                    s.ShowSubtitle(s.GetSubtitle("Menu", "weaponFound")); // Menu
                                    answer = ByteAnswer();
                                    switch (answer)
                                    {
                                        case 0:
                                            s.ShowSubtitle(s.GetSubtitle("Subtitles", "dontEquipWeapon")); // Abandona a arma
                                            break;

                                        case 1:
                                            // Mostra a arma do jogador (se tiver) e a arma encontrada
                                            if (player.WeaponEquiped != false)
                                            {
                                                // Compara a arma que já tem com a que encontrou
                                                s.ShowSubtitle($"\n[{s.GetSubtitle("Titles", "myWeapon")}]\n" + player.Weapon + $"\n\n[{s.GetSubtitle("Titles", "weaponFound")}]\n" + weaponFound + "\n");
                                            }
                                            else
                                            {
                                                // Diz que não tem arma e avalia a encontrada
                                                s.ShowSubtitle(s.GetSubtitle("Subtitles", "haveNoWeapon") + weaponFound + "\n");
                                            }
                                            break;

                                        case 2:
                                            // Equipa a arma
                                            WeaponCollect(s, player, weaponFound);
                                            break;

                                        default:
                                            // Mensagem de erro
                                            s.ShowSubtitle(s.GetSubtitle("Error", "invalidAction"));
                                            continue;
                                    }

                                    if (answer != 1) break; // Permite comparar sem sair do loop
                                }
                            }
                            #endregion

                            #region Mob
                            else if (answer <= 7)
                            {
                                randomChoose = random.Next(mobs.Length); // Sorteia um número
                                mobFound = mobs[randomChoose]; // Seleciona um mob aleatório

                                // Parelha o lvl dos inimigos
                                int mobLvl = mobFound.Lvl;
                                if (player.Lvl >= mobLvl) mobLvl = player.Lvl + 3;
                                mobFound.LvlUp(random.Next(mobFound.Lvl, mobLvl));

                                // Configuração dos mobs
                                if (mobFound.Name == mobs[1].Name)
                                {
                                    mobFound.WeaponEquip(weapons[3], mobFound.Lvl); // Equipa o esqueleto com um arco
                                }

                                mobFound.Cure(Tools.RandomDouble((double)mobFound.MaxLife / 2, (double)mobFound.MaxLife)); // Vida dos mobs determinadas aleatóriamente e impede de ser 0

                                // Loot do mob
                                mobFound.GetCoins(Tools.RandomDouble(10d));

                                s.ShowSubtitle(s.GetSubtitle("Subtitles", "mobFound").Replace("#", mobFound.Name)); // Encontra um mob
                                Fighting(true, player, mobFound); // Faz o player e o mob entrarem em modo de luta
                                while (mobFound.Fighting == true)
                                {

                                    s.ShowSubtitle(s.GetSubtitle("Menu", "mobFound")); // Menu
                                    answer = ByteAnswer();

                                    #region Chance de escapar
                                    if (answer == 0)
                                    {
                                        // Sistema de chances de conseguir escapar
                                        if (player.EscapeChance > player.EscapeChance / 3)
                                        {
                                            s.ShowSubtitle(s.GetSubtitle("Subtitles", "canRunAway").Replace("#", mobFound.Name)); // Foge do mob
                                            Fighting(false, player, mobFound);
                                            break;
                                        }
                                        else
                                        {
                                            s.ShowSubtitle(s.GetSubtitle("Subtitles", "cantRunAway")); // Não consegue fugir do mob
                                        }
                                    }
                                    #endregion

                                    // Escolha
                                    switch (answer)
                                    {
                                        case 0:
                                            break;

                                        case 1:
                                            s.ShowSubtitle("\n" + mobFound + "\n");
                                            break;

                                        case 2:
                                            double _damage = player.SetDamage(mobFound);
                                            if (player.WeaponEquiped == true && player.Weapon.Condition <= 0) // Se a arma quebrar, droppar ela
                                            {
                                                // Desequipa a arma
                                                player.WeaponUnequip();
                                                s.ShowSubtitle(s.GetSubtitle("Subtitles", "weaponBroke")); // Mostra que a arma quebrou
                                            }

                                            // Mostra quanto dano ô jogador causou ao inimigo
                                            s.ShowSubtitle(s.GetSubtitle("Combat", "damageToMob").Replace("#1", _damage.ToString("F2", CultureInfo.InvariantCulture)).Replace("#2", mobFound.Name));
                                            break;

                                        case 3:
                                            s.ShowSubtitle(player.ToString());
                                            break;

                                        default:
                                            // Erro de ação invalida
                                            s.ShowSubtitle(s.GetSubtitle("Error", "invalidAction"));
                                            continue;
                                    }

                                    // Morte do inimigo
                                    if (mobFound.Life <= 0)
                                    {
                                        s.ShowSubtitle(s.GetSubtitle("Combat", "mobDefeat").Replace("#", mobFound.Name));

                                        // Coleta o Xp
                                        XpCollect(s, player, mobFound.Xp);
                                        // Coleta as moedas
                                        CoinsCollect(s, player, mobFound.Coins);

                                        #region Chance de drop
                                        // Chance do mob droppar a arma que ele está usando
                                        if (mobFound.WeaponEquiped == true)
                                        {

                                            double dropChance = 3d;
                                            double dropped = Tools.RandomDouble(dropChance);
                                            if (dropped > dropChance / 1.3)
                                            {
                                                // Mostra o drop do mob
                                                s.ShowSubtitle(s.GetSubtitle("Subtitles", "mobDrop").Replace("#1", mobFound.Name).Replace("#2", mobFound.Weapon.Name));
                                                s.ShowSubtitle(s.GetSubtitle("Menu", "noYes")); // Mostra as opções sim e não
                                                answer = ByteAnswer(1);

                                                switch (answer)
                                                {
                                                    case 0:
                                                        // Ignora a arma
                                                        s.ShowSubtitle(s.GetSubtitle("Subtitles", "weaponIgnore"));
                                                        break;

                                                    case 1:
                                                        // Equipa a arma do inimigo
                                                        WeaponCollect(s, player, mobFound.Weapon);
                                                        break;

                                                    default:
                                                        // Mostra o erro de ação invalida
                                                        InvalidAction(s);
                                                        continue;
                                                }
                                            }
                                        }

                                        #endregion

                                        Fighting(false, player, mobFound); // Tira os dois de moto de luta
                                        break;
                                    }
                                    else
                                    {
                                        #region Turno do mob
                                        if (mobFound.Fighting == true)
                                        {
                                            // Chance do inimigo querer fugir
                                            if (mobFound.Life < mobFound.MaxLife / 5)
                                            {
                                                if (mobFound.EscapeChance > mobFound.EscapeChance / 3)
                                                {
                                                    // O inimigo está fugindo
                                                    s.ShowSubtitle(s.GetSubtitle("Subtitles", "mobRunningAway").Replace("#", mobFound.Name));
                                                    s.ShowSubtitle(s.GetSubtitle("Menu", "allowRunToward")); // permitir ou correr atrás
                                                    answer = ByteAnswer(1);

                                                    switch (answer)
                                                    {
                                                        case 0:
                                                            // Permite a fuga do mob
                                                            s.ShowSubtitle(s.GetSubtitle("Subtitles", "allowMobRun"));
                                                            Fighting(false, player, mobFound); // Tira os dois do modo de luta
                                                            break;

                                                        // Verifica se você consegue evitar a fuga do inimigo ou não
                                                        case 1:
                                                            if (mobFound.EscapeChance < mobFound.EscapeChance / 3)
                                                            {
                                                                // Consegue capturar o mob
                                                                s.ShowSubtitle(s.GetSubtitle("Subtitles", "canCatchMob"));
                                                            }
                                                            else
                                                            {
                                                                // O mob escapa
                                                                s.ShowSubtitle(s.GetSubtitle("Subtitles", "cantCatchMob").Replace("#", mobFound.Name));
                                                                Fighting(false, player, mobFound);
                                                            }
                                                            break;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                // Ataque do inimigo
                                                if (answer == 2)
                                                {
                                                    // Inimigo ataca
                                                    double mobDamage = mobFound.SetDamage(player);
                                                    // Mostra o dano do inimigo
                                                    s.ShowSubtitle(s.GetSubtitle("Combat", "damageToPlayer").Replace("#1", mobFound.Name).Replace("#2", mobDamage.ToString("F2", CultureInfo.InvariantCulture)));
                                                }
                                            }
                                        }
                                        #endregion
                                    }

                                    if (player.Life <= 0) break;
                                }
                            }

                            #endregion

                            #region Comerciante
                            else if (answer <= 9)
                            {

                                randomChoose = random.Next(1, weapons.Length); // Sorteia um número
                                weaponFound = weapons[randomChoose]; // Pega a arma de acordo com o número sorteado
                                weaponFound.Condition = weaponFound.MaxCondition;

                                double weaponPrice = Tools.RandomDouble(weaponFound.MaxPrice, weaponFound.MinPrice);

                                // Encontra o mercador
                                s.ShowSubtitle(s.GetSubtitle("Subtitles", "merchantFound"));
                                while (true)
                                {
                                    s.ShowSubtitle(s.GetSubtitle("Menu", "merchantFound")); // menu
                                    answer = ByteAnswer(1);
                                    if (answer == 0)
                                    {
                                        // Ignora o mercador
                                        s.ShowSubtitle(s.GetSubtitle("Subtitles", "merchantIgnore"));
                                        break;
                                    }

                                    // Escolha
                                    switch (answer)
                                    {
                                        case 1:
                                            // Titulo
                                            s.ShowSubtitle($"[{s.GetSubtitle("Titles", "items")}]\n");
                                            // moedas do jogador
                                            s.ShowSubtitle($"[{s.GetSubtitle("Subtitles", "myCoins").Replace("#", player.Coins.ToString("F2", CultureInfo.InvariantCulture))}]\n");

                                            #region Mercado
                                            // Mostra o mercado
                                            s.ShowSubtitle(s.GetSubtitle("Merchant", "shop").Replace("#1", weaponFound.Name).Replace("#2", weaponPrice.ToString("F2", CultureInfo.InvariantCulture)));
                                            #endregion
                                            answer = ByteAnswer(2);
                                            if (answer == 0) break;
                                            switch (answer)
                                            {
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

                                                    if (player.Buy(weaponPrice) && WeaponCollect(s, player, weaponFound))
                                                    {
                                                        // Compra e equipa a arma
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

                                        default:
                                            // Mensagem de erro
                                            InvalidAction(s);
                                            continue;
                                    }

                                    if (answer != 1) break;
                                }
                            }
                            #endregion

                            #region Tesouro
                            else if (answer <= 10)
                            {

                                // Gera uma quantidade aleatória de moedas
                                double coins = Tools.RandomDouble(20d, 5d); // Sorteia um número
                                s.ShowSubtitle(s.GetSubtitle("Subtitles", "treasuryFound")); // Encontra um tesouro
                                s.ShowSubtitle(s.GetSubtitle("Menu", "noYes")); // opções sim e não
                                answer = ByteAnswer(1);
                                switch (answer)
                                {
                                    case 0:
                                        break;

                                    case 1:
                                        CoinsCollect(s, player, coins);
                                        // Coleta as moedas
                                        s.ShowSubtitle(s.GetSubtitle("Subtitles", "coinsCollect").Replace("#", coins.ToString("F2", CultureInfo.InvariantCulture)));
                                        break;

                                    default:
                                        // Ação invalida
                                        InvalidAction(s);
                                        continue;

                                }

                            }

                            #endregion

                            else
                            {
                                // Mostra ação invalida
                                InvalidAction(s);
                            }
                            break;
                        #endregion

                        case GameActions.PlayerStatus:
                            s.ShowSubtitle(player.ToString());
                            break;

                        case GameActions.PlayerBag:
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

                        case GameActions.RaceDescription:
                            s.ShowSubtitle(player.Description.ToString());
                            break;

                        default:
                            InvalidAction(s);
                            break;
                    }
                }
            }

            s.ShowSubtitle(s.GetSubtitle("Subtitles", "thanksForPlayed"));
            #endregion
        }

        #region Menu
        public static bool Menu(LanguagesManager s)
        {
            s.ShowSubtitle(s.GetSubtitle("Menu", "start"));
            byte n = ByteAnswer(1);

            if (n == 1) return true;

            return false;
        }

        public static Mob CreatePlayer(LanguagesManager s, Mob[] race)
        {
            string playerName = null;
            s.ShowSubtitle(s.GetSubtitle("Subtitles", "setName")); // Pequena apresentação
            Console.Write(">> ");
            playerName = Console.ReadLine(); // Pega o nome do jogador
            s.ShowSubtitle(s.GetSubtitle("Subtitles", "greatChose"));
            Mob player = RaceChoose(s, race, playerName); // Cria o jogador
            s.ShowSubtitle(s.GetSubtitle("Subtitles", "wellcome"));

            return player;
        }

        public static void GameOver(LanguagesManager s)
        {
            s.ShowSubtitle(s.GetSubtitle("Menu", "gameOver"));
        }

        public static Mob RaceChoose(LanguagesManager s, Mob[] race, string name)
        {

            byte choose = 0;
            Mob raceChose = null;
            //*
            s.ShowSubtitle(s.GetSubtitle("Subtitles", "raceChoose"));
            while (raceChose == null)
            {
                s.ShowSubtitle($"\n[{s.GetSubtitle("Titles", "races")}]\n");
                int max = race.Length - 1;
                for (byte i = 0; i < (byte)race.Length; i++) s.ShowSubtitle($"{i}: {race[i].Race}");
                choose = ByteAnswer((byte)max);

                race[choose].Name = name;
                s.ShowSubtitle($"\n{race[choose]}\n{race[choose].Description}\n");

                s.ShowSubtitle(s.GetSubtitle("Menu", "returnConfirm"));
                switch (ByteAnswer(1))
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
            //*/
            return raceChose;
        }

        #endregion

        #region Interação com o usuário

        #endregion

        #region Actions
        // Modo luta
        public static void Fighting(bool mode, params Mob[] mobs)
        {
            foreach (Mob m in mobs)
            {
                m.Fighting = mode;
            }
        }

        #endregion

        #region Coleta
        public static void XpCollect(LanguagesManager s, Mob mob, double xp)
        {
            if (mob.GetXp(xp))
            {
                //Legenda
                s.ShowSubtitle(
                    $"{s.GetSubtitle("Subtitles", "xpReceived")}" +
                    $"{xp.ToString("F2", CultureInfo.InvariantCulture)}xp\n");
            }
        }

        public static void CoinsCollect(LanguagesManager s, Mob mob, double coins)
        {
            if (mob.GetCoins(coins))
            {
                // Legenda
                s.ShowSubtitle(
                    $"{s.GetSubtitle("Subtitles", "coinsReceived")}" +
                    $"{coins.ToString("F2", CultureInfo.InvariantCulture)}" +
                    $"{s.GetSubtitle("MobClass", "coins")}\n");
            }
        }

        public static bool WeaponCollect(LanguagesManager s, Mob mob, Weapon weapon)
        {
            if (mob.WeaponEquip(weapon, weapon.NecessaryLvl))
            {
                // Legenda
                s.ShowSubtitle(
                    s.GetSubtitle("Subtitles", "weaponEquipped"));

                return true;
            }
            else
            {
                s.ShowSubtitle(s.GetSubtitle("Subtitles", "insufficientLvl"));
                return false;
            }
        }
        #endregion

        #region Funções úteis

        public static void InvalidAction(LanguagesManager s)
        {
            // mostra ação invalida
            s.ShowSubtitle(s.GetSubtitle("Error", "invalidAction"));
        }

        public static byte ByteAnswer(byte max = 10)
        {
            byte answer = 0;
            bool allright = false;
            while (!allright || answer > max)
            {
                try
                {
                    Console.Write(">> ");
                    answer = byte.Parse(Console.ReadLine());
                    allright = true;
                }
                catch (Exception)
                {
                    Console.WriteLine($"Error.\nTry Again.");
                }
            }
            return answer;
        }

        #endregion
    }
}
