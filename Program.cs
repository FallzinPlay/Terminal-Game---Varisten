using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Net;
using Game.Classes;

namespace Game
{

    static class Constants
    {
        public const double EscapeChance = 1.5;
    }

    internal class Program
    {
        static void Main(string[] args)
        {

            #region Escolha de idioma
            Console.WriteLine(
                "Choose your language:\n" +
                "0 - English, USA (default)\n" +
                "1 - Português, BR");
            LanguagesManager s = new LanguagesManager(ByteAnswer(1)); // Enquanto não for uma opção valida ele continuará pedindo para escolher
            #endregion

            #region Configurações de classes

            Random random = new Random();
            int action;
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
            race[0].Description = $"\n[{race[0].Race.ToUpper()}]:\n" + s.GetSubtitle("Descriptions", "humans"); // Humano
            race[1].Description = $"\n[{race[1].Race.ToUpper()}]:\n" + s.GetSubtitle("Descriptions", "dwarves"); // Anão
            #endregion

            #region Jogo
            byte answer;
            if (s.Chose)
            {
                while (Menu(s))
                {
                    #region Apresentação do jogo e criação de personagem

                    Console.WriteLine(s.GetSubtitle("Subtitles", "setName")); // Pequena apresentação
                    Console.Write(">> ");
                    string name = Console.ReadLine(); // Pega o nome do jogador
                    Console.WriteLine(s.GetSubtitle("Subtitles", "greatChose"));
                    Mob player = RaceChoose(s, race, name); // Cria o jogador
                    Console.WriteLine(s.GetSubtitle("Subtitles", "wellcome"));

                    #endregion

                    // Loop do jogo
                    while (true)
                    {

                        // Acaba o jogo se o jogador morrer
                        if (player.Life <= 0)
                        {
                            GameOver(s);
                            break;
                        }

                        Console.WriteLine(s.GetSubtitle("Menu", "options")); // Menu
                        Console.Write(">> ");
                        action = int.Parse(Console.ReadLine());

                        // Sai para o menu principal
                        if (action == 0)
                        {
                            Console.WriteLine(s.GetSubtitle("Subtitles", "leaving")); // Saindo do jogo
                            break;
                        }
                        switch (action)
                        {
                            case 0:
                                break;

                            #region Aventura
                            case 1:

                                action = random.Next(10);
                                #region Weapon
                                if (action <= 3)
                                {

                                    randomChoose = random.Next(1, weapons.Length); // Sorteia um número
                                    weaponFound = weapons[randomChoose]; // Pega a arma de acordo com o número sorteado
                                    weaponFound.Condition = random.Next(1, weaponFound.MaxCondition); // Determina a condição da arma aleatoriamente

                                    Console.WriteLine($"{s.GetSubtitle("Subtitles", "weaponFound").Replace("#", weaponFound.Name)}"); // Encontra uma arma

                                    while (true)
                                    {
                                        Console.WriteLine(s.GetSubtitle("Menu", "weaponFound")); // Menu
                                        answer = ByteAnswer();
                                        switch (answer)
                                        {
                                            case 0:
                                                Console.WriteLine(s.GetSubtitle("Subtitles", "dontEquipWeapon")); // Abandona a arma
                                                break;

                                            case 1:
                                                // Mostra a arma do jogador (se tiver) e a arma encontrada
                                                if (player.WeaponEquiped != false)
                                                {
                                                    // Compara a arma que já tem com a que encontrou
                                                    Console.WriteLine($"\n[{s.GetSubtitle("Titles", "myWeapon")}]\n" + player.Weapons + $"\n\n[{s.GetSubtitle("Titles", "weaponFound")}]\n" + weaponFound + "\n");
                                                }
                                                else
                                                {
                                                    // Diz que não tem arma e avalia a encontrada
                                                    Console.WriteLine(s.GetSubtitle("Subtitles", "haveNoWeapon") + weaponFound + "\n");
                                                }
                                                break;

                                            case 2:
                                                // Equipa a arma
                                                player.WeaponEquip(weaponFound, weaponFound.NecessaryLvl);
                                                break;

                                            default:
                                                // Mensagem de erro
                                                Console.WriteLine(s.GetSubtitle("Error", "invalidAction"));
                                                continue;
                                        }

                                        if (answer != 1) break; // Permite comparar sem sair do loop
                                    }
                                }
                                #endregion

                                #region Mob
                                else if (action <= 7)
                                {
                                    randomChoose = random.Next(0, mobs.Length); // Sorteia um número
                                    mobFound = mobs[randomChoose]; // Seleciona um mob aleatório

                                    // Parelha o lvl dos inimigos
                                    int mobLvl = mobFound.Lvl;
                                    if (player.Lvl >= mobLvl) mobLvl = player.Lvl + 3;
                                    mobFound.ForceLvlUp(random.Next(mobFound.Lvl, mobLvl));

                                    // Configuração dos mobs
                                    if (mobFound.Name == mobs[1].Name)
                                    {
                                        mobFound.Weapons = weapons[3]; // Equipa o esqueleto com um arco
                                    }

                                    mobFound.Cure(RandomDouble(random, (double)mobFound.MaxLife / 2, (double)mobFound.MaxLife)); // Vida dos mobs determinadas aleatóriamente e impede de ser 0

                                    // Loot do mob
                                    mobFound.Coins = RandomDouble(random, 0d, 10d);
                                    double xp = RandomDouble(random, 5d, 20d);

                                    Console.WriteLine(s.GetSubtitle("Subtitles", "mobFound").Replace("#", mobFound.Name)); // Encontra um mob
                                    Fighting(true, player, mobFound); // Faz o player e o mob entrarem em modo de luta
                                    while (mobFound.Fighting == true)
                                    {

                                        Console.WriteLine(s.GetSubtitle("Menu", "mobFound")); // Menu
                                        answer = ByteAnswer();

                                        #region Chance de escapar
                                        if (answer == 0)
                                        {
                                            // Sistema de chances de conseguir escapar
                                            if (player.EscapeChance > player.EscapeChance / Constants.EscapeChance)
                                            {
                                                Console.WriteLine(s.GetSubtitle("Subtitles", "canRunAway").Replace("#", mobFound.Name)); // Foge do mob
                                                Fighting(false, player, mobFound);
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine(s.GetSubtitle("Subtitles", "cantRunAway")); // Não consegue fugir do mob
                                            }
                                        }
                                        #endregion

                                        // Escolha
                                        switch (answer)
                                        {
                                            case 0:
                                                break;

                                            case 1:
                                                Console.WriteLine("\n" + mobFound + "\n");
                                                break;

                                            case 2:
                                                double _damage = Atack(s, random, player, mobFound, player.Weapons, weapons);
                                                if (player.WeaponEquiped == true && player.Weapons.Condition <= 0) // Se a arma quebrar, droppar ela
                                                {
                                                    // Desequipa a arma
                                                    player.WeaponUnequip();
                                                    Console.WriteLine(s.GetSubtitle("Subtitles", "weaponBroke")); // Mostra que a arma quebrou
                                                }

                                                // Mostra quanto dano ô jogador causou ao inimigo
                                                Console.WriteLine(s.GetSubtitle("Combat", "damageToMob").Replace("#1", _damage.ToString("F2", CultureInfo.InvariantCulture)).Replace("#2", mobFound.Name));
                                                break;

                                            case 3:
                                                Console.WriteLine(player);
                                                break;

                                            default:
                                                // Erro de ação invalida
                                                Console.WriteLine(s.GetSubtitle("Error", "invalidAction"));
                                                continue;
                                        }

                                        // Morte do inimigo
                                        if (mobFound.Life <= 0)
                                        {
                                            Console.WriteLine(s.GetSubtitle("Combat", "mobDefeat").Replace("#", mobFound.Name));

                                            player.GetXp(xp);
                                            player.GetCoins(mobFound.Coins);

                                            #region Chance de drop
                                            // Chance do mob droppar a arma que ele está usando
                                            if (mobFound.WeaponEquiped == true)
                                            {

                                                double dropChance = 3d;
                                                double dropped = RandomDouble(random, 0d, dropChance);
                                                if (dropped > dropChance / 1.3)
                                                {
                                                    // Mostra o drop do mob
                                                    Console.WriteLine(s.GetSubtitle("Subtitles", "mobDrop").Replace("#1", mobFound.Name).Replace("#2", mobFound.Weapons.Name));
                                                    Console.WriteLine(s.GetSubtitle("Menu", "noYes")); // Mostra as opções sim e não
                                                    answer = ByteAnswer(1);

                                                    switch (answer)
                                                    {
                                                        case 0:
                                                            // Ignora a arma
                                                            Console.WriteLine(s.GetSubtitle("Subtitles", "weaponIgnore"));
                                                            break;

                                                        case 1:
                                                            player.WeaponEquip(mobFound.Weapons, mobFound.Weapons.NecessaryLvl);
                                                            break;

                                                        default:
                                                            // Mostra o erro de ação invalida
                                                            Console.WriteLine(s.GetSubtitle("Error", "invalidAction"));
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
                                                    if (mobFound.EscapeChance > mobFound.EscapeChance / Constants.EscapeChance)
                                                    {
                                                        // O inimigo está fugindo
                                                        Console.WriteLine(s.GetSubtitle("Subtitles", "mobRunningAway").Replace("#", mobFound.Name));
                                                        Console.WriteLine(s.GetSubtitle("Menu", "allowRunToward")); // permitir ou correr atrás
                                                        answer = ByteAnswer(1);

                                                        switch (answer)
                                                        {
                                                            case 0:
                                                                // Permite a fuga do mob
                                                                Console.WriteLine(s.GetSubtitle("Subtitles", "allowMobRun"));
                                                                Fighting(false, player, mobFound); // Tira os dois do modo de luta
                                                                break;

                                                            // Verifica se você consegue evitar a fuga do inimigo ou não
                                                            case 1:
                                                                if (mobFound.EscapeChance < mobFound.EscapeChance / Constants.EscapeChance)
                                                                {
                                                                    // Consegue capturar o mob
                                                                    Console.WriteLine(s.GetSubtitle("Subtitles", "canCatchMob"));
                                                                }
                                                                else
                                                                {
                                                                    // O mob escapa
                                                                    Console.WriteLine(s.GetSubtitle("Subtitles", "cantCatchMob").Replace("#", mobFound.Name));
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
                                                        double mobDamage = Atack(s, random, mobFound, player, mobFound.Weapons, weapons);
                                                        // Mostra o dano do inimigo
                                                        Console.WriteLine(s.GetSubtitle("Combat", "damageToPlayer").Replace("#1", mobFound.Name).Replace("#2", mobDamage.ToString("F2", CultureInfo.InvariantCulture)));
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
                                else if (action <= 9)
                                {

                                    randomChoose = random.Next(1, weapons.Length); // Sorteia um número
                                    weaponFound = weapons[randomChoose]; // Pega a arma de acordo com o número sorteado
                                    weaponFound.Condition = weaponFound.MaxCondition;

                                    double weaponPrice = RandomDouble(random, weaponFound.MinPrice, weaponFound.MaxPrice);

                                    // Encontra o mercador
                                    Console.WriteLine(s.GetSubtitle("Subtitles", "merchantFound"));
                                    while (true)
                                    {
                                        Console.WriteLine(s.GetSubtitle("Menu", "merchantFound")); // menu
                                        answer = ByteAnswer(1);
                                        if (answer == 0)
                                        {
                                            // Ignora o mercador
                                            Console.WriteLine(s.GetSubtitle("Subtitles", "merchantIgnore"));
                                            break;
                                        }

                                        // Escolha
                                        switch (answer)
                                        {
                                            case 1:
                                                // Titulo
                                                Console.WriteLine($"[{s.GetSubtitle("Titles", "items")}]\n");
                                                // moedas do jogador
                                                Console.WriteLine($"[{s.GetSubtitle("Subtitles", "myCoins").Replace("#", player.Coins.ToString("F2", CultureInfo.InvariantCulture))}]\n");

                                                #region Mercado
                                                // Mostra o mercado
                                                Console.WriteLine(s.GetSubtitle("Merchant", "shop").Replace("#1", weaponFound.Name).Replace("#2", weaponPrice.ToString("F2", CultureInfo.InvariantCulture)));
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
                                                            Console.WriteLine($"[{s.GetSubtitle("Titles", "life")}] {player.Life.ToString("F2", CultureInfo.InvariantCulture)}\n");
                                                        }
                                                        else
                                                        {
                                                            // Moedas insuficientes
                                                            Console.WriteLine(s.GetSubtitle("Subtitles", "insufficientMoney"));
                                                        }
                                                        continue;

                                                    case 2:

                                                        if (player.Buy(weaponPrice) && player.WeaponEquip(weaponFound, weaponFound.NecessaryLvl))
                                                        {
                                                            // Compra e equipa a arma
                                                            Console.WriteLine(s.GetSubtitle("Subtitles", "buyWeapon"));
                                                        }
                                                        else
                                                        {
                                                            // Moedas insuficientes
                                                            Console.WriteLine(s.GetSubtitle("Subtitles", "insufficientMoney"));
                                                        }
                                                        continue;

                                                    default:
                                                        // Mensagem de erro
                                                        Console.WriteLine(s.GetSubtitle("Error", "invalidAction"));
                                                        continue;
                                                }

                                            default:
                                                // Mensagem de erro
                                                Console.WriteLine(s.GetSubtitle("Error", "invalidAction"));
                                                continue;
                                        }

                                        if (answer != 1) break;
                                    }
                                }
                                #endregion

                                #region Tesouro
                                else if (action <= 10)
                                {

                                    // Gera uma quantidade aleatória de moedas
                                    double coins = RandomDouble(random, 5d, 20d); // Sorteia um número
                                    Console.WriteLine(s.GetSubtitle("Subtitles", "treasuryFound")); // Encontra um tesouro
                                    Console.WriteLine(s.GetSubtitle("Menu", "noYes")); // opções sim e não
                                    answer = ByteAnswer(1);
                                    switch (answer)
                                    {
                                        case 0:
                                            break;

                                        case 1:
                                            player.GetCoins(coins);
                                            // Coleta as moedas
                                            Console.WriteLine(s.GetSubtitle("Subtitles", "coinsCollect").Replace("#", coins.ToString("F2", CultureInfo.InvariantCulture)));
                                            break;

                                        default:
                                            // Ação invalida
                                            Console.WriteLine(s.GetSubtitle("Error", "invalidAction"));
                                            continue;

                                    }

                                }

                                #endregion

                                else
                                {
                                    // Mostra ação invalida
                                    Console.WriteLine(s.GetSubtitle("Error", "invalidAction"));
                                }
                                break;
                            #endregion

                            case 2:
                                // Mostra o status do player
                                Console.WriteLine(player);
                                break;

                            case 3:
                                // Mostra a arma do player
                                if (player.WeaponEquiped != false)
                                {
                                    Console.WriteLine(player.Weapons);
                                }
                                else
                                {
                                    // Diz que não tem arma
                                    Console.WriteLine(s.GetSubtitle("Subtitles", "haveNoWeapon"));
                                }
                                break;

                            case 4:
                                Console.WriteLine(player.Description);
                                break;

                            default:
                                // mostra ação invalida
                                Console.WriteLine(s.GetSubtitle("Error", "invalidAction"));
                                break;
                        }
                    }
                }
            }

            Console.WriteLine(s.GetSubtitle("Subtitles", "thanksForPlayed"));
            #endregion
        }

        #region Menu
        public static bool Menu(LanguagesManager s)
        {
            Console.WriteLine(s.GetSubtitle("Menu", "start"));
            byte n = ByteAnswer(1);

            if (n == 1) return true;

            return false;
        }

        public static void GameOver(LanguagesManager s)
        {
            Console.WriteLine(s.GetSubtitle("Menu", "gameOver"));
        }

        public static Mob RaceChoose(LanguagesManager s, Mob[] race, string name)
        {
            byte choose;
            Mob raceChose = null;

            Console.WriteLine(s.GetSubtitle("Subtitles", "raceChoose"));
            while (raceChose == null)
            {
                Console.WriteLine(
                    $"\n[{s.GetSubtitle("Titles", "races")}]\n" +
                    $"0 - {race[0].Race}\n" +
                    $"1 - {race[1].Race}");
                Console.Write(">> ");
                choose = byte.Parse(Console.ReadLine());

                race[choose].Name = name;
                Console.WriteLine($"\n{race[choose]}\n{race[choose].Description}\n");

                Console.WriteLine(s.GetSubtitle("Menu", "returnConfirm"));
                switch (ByteAnswer(1))
                {
                    case 0:
                        continue;

                    case 1:
                        raceChose = race[choose];
                        break;

                    default:
                        Console.WriteLine(s.GetSubtitle("Error", "invalidAction"));
                        continue;
                }
            }
            return raceChose;
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
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex}\nTry Again.");
                }
            }
            return answer;
        }

        #endregion

        #region Interação com o usuário

        #endregion

        #region Actions


        #endregion

        #region Combat

        // Modo luta
        public static void Fighting(bool mode, params Mob[] mobs)
        {
            foreach (Mob m in mobs)
            {
                m.Fighting = mode;
            }
        }

        // Esquiva
        public static bool Dodge(LanguagesManager s, Random r, Mob _mob)
        {
            double dodgeChance = 20;
            if (RandomDouble(r, 0d, dodgeChance) <= _mob.Dodge)
            {
                Console.WriteLine($"[{_mob.Name} {s.GetSubtitle("Combat", "dodged")}]!!");
                return true;
            }
            return false;
        }

        // Ataque
        public static double Atack(LanguagesManager s, Random r, Mob mobSet, Mob mobGet, Weapon mobWeapon, Weapon[] w)
        {
            if (!Dodge(s, r, mobGet))
            {
                double damage = mobSet.Damage;

                // Se estiver equipando uma arma, somar ataque
                if (mobSet.WeaponEquiped && mobWeapon.Name != w[0].Name)
                {
                    damage = mobWeapon.Damage + mobSet.Damage / 5;
                    mobWeapon.Erode();
                }

                // Chance de crítico
                double criticChance = 20;
                if (RandomDouble(r, 0d, criticChance) <= mobSet.CriticChance)
                {
                    damage *= mobSet.CriticDamage;
                    Console.WriteLine($"[{s.GetSubtitle("Combat", "critical")}]!!\n");
                }

                // O mob atacado recebe o dano
                mobGet.GetDamage(damage);
                return damage;
            }

            return 0;
        }

        #endregion

        #region Funções úteis

        public static double RandomDouble(Random r, double min, double max)
        {
            return min + (r.NextDouble() * (max - min));
        }

        #endregion
    }
}
