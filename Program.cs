using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

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
            // Proxima atualização
            //--------!! Adicionar biomas !!-----------//

            Subtitles s = new Subtitles();

            Random random = new Random();
            int action;
            int randomChoose;

            // Armas
            Weapon[] weapons = new Weapon[4];
            // nome, dano, condição, nivel necessario, preço min, preço max
            weapons[0] = new Weapon("--", 0d, 0, 0, 0d, 0d);
            weapons[1] = new Weapon("Stick", 3.2d, 4, 1, 5d, 10d);
            weapons[2] = new Weapon("Wooden sword", 3.5d, 5, 2, 7d, 12d);
            weapons[3] = new Weapon("Wooden bow", 4.2d, 6, 3, 10d, 15d);
            Weapon weaponFound;
            

            // Entidades
            Mob[] mobs = new Mob[3];
            // nome, raça, vida, dano, vida maxima, chace de critico, dano de critico, arma, nivel, nivel maximo, esquiva, chance de escapar
            mobs[0] = new Mob("Zombie", "zombies", 0, 2.2d, 7, 1.1d, 1.7d, weapons[0], weapons[0].NecessaryLvl, 10, 0.8d, 1.2d);
            mobs[1] = new Mob("Skeleton", "skeletons", 0, 3.2d, 5, 2.2d, 2.3d, weapons[3], weapons[3].NecessaryLvl, 12, 2.1d, 1.6d);
            mobs[2] = new Mob("Slime", "slimes", 0, 2.7d, 4, 3.5d, 2.7d, weapons[0], weapons[0].NecessaryLvl, 15, 0.5d, 2d);
            Mob mobFound;

            // Raças
            Mob[] race = new Mob[2];
            // nome, raça, vida, dano, vida maxima, chace de critico, dano de critico, arma, nivel, nivel maximo, esquiva, chance de escapar
            race[0] = new Mob(null, "humans", 10d, 2.4d, 10, 1.7d, 1.5d, weapons[0], 1, 10, 1.7d, 1.7d);
            race[1] = new Mob(null, "dwarves", 8d, 3d, 8, 1.3d, 1.6d, weapons[0], 1, 7, 1.2d, 1.3d);

            #region Descrição das raças

            // Humano
            race[0].Description = $"\n[{race[0].Race.ToUpper()}]:\n" +
                "If you want to be adaptable, this race is perfect to you!\n" +
                "The humans are good learning new things and using all kind of weapons.\n" +
                "They living as in small civilizations as in big cities, and\n" +
                "can learning any language." +
                "\n";

            // Anão
            race[1].Description = $"\n[{race[1].Race.ToUpper()}]:\n" +
                $"The dwarves are masters building anything.\n" +
                "They making the most powerful weapons in any civilization, and\n" +
                "working in the mines obtaining the most valuable ores.\n" +
                "But these armorers can be a bit unstable at times.\n" +
                "\n";

            #endregion

            byte answer;

            #region Jogo
            while (Menu())
            {
                #region Apresentação do jogo e criação de personagem

                s.Print(s.SetName); // Pequena apresentação
                Console.Write(">> ");
                string name = Console.ReadLine(); // Pega o nome do jogador
                s.Print(s.GreatChoose);
                Mob player = RaceChoose(race, name); // Cria o jogador
                s.Print(s.Wellcome);

                #endregion

                // Loop do jogo
                while (true)
                {
                    
                    // Acaba o jogo se o jogador morrer
                    if (player.Life <= 0)
                    {
                        GameOver();
                        break;
                    }

                    s.Print(s.MenuOptions);
                    Console.Write(">> ");
                    action = int.Parse(Console.ReadLine());
                    switch (action)
                    {
                        case 0:
                            Console.WriteLine(s.Exiting);
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

                                Console.WriteLine($"A {weaponFound.Name}! Should I equip it?\n");

                                while (true)
                                {
                                    Console.WriteLine(s.MenuWeaponFound);
                                    answer = ByteAnswer();
                                    switch (answer)
                                    {
                                        case 0:
                                            Console.WriteLine("I don't need it for now...\n");
                                            break;

                                        case 1:
                                            // Mostra a arma do jogador (se tiver) e a arma encontrada
                                            if (player.WeaponEquiped != false)
                                            {
                                                Console.WriteLine("\n[My weapon]\n" + player.Weapons + "\n\n[Weapon Found]\n" + weaponFound + "\n");
                                            }
                                            else
                                            {
                                                Console.WriteLine("\nI don't have any weapon...\n" + weaponFound + "\n");
                                            }
                                            break;

                                        case 2:
                                            // Equipa a arma
                                            player.WeaponEquip(weaponFound, weaponFound.NecessaryLvl);
                                            break;

                                        default:
                                            Console.WriteLine("Invalid action!\n");
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

                                Console.WriteLine($"A {mobFound.Name}! What should I do?\n");
                                Fighting(true, player, mobFound); // Faz o player e o mob entrarem em modo de luta
                                while (mobFound.Fighting == true)
                                {

                                    Console.WriteLine(s.MenuMobFound);
                                    answer = ByteAnswer();

                                    #region Chance de escapar
                                    if (answer == 0)
                                    {
                                        // Sistema de chances de conseguir escapar
                                        if (player.EscapeChance > player.EscapeChance / Constants.EscapeChance)
                                        {
                                            Console.WriteLine($"You run away from the {mobFound.Name} as far as you can.\n");
                                            Fighting(false, player, mobFound);
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine("I couldn't escape. I will have to fight!\n");
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
                                            double _damage = Atack(random, player, mobFound, player.Weapons, weapons);
                                            if (player.WeaponEquiped == true && player.Weapons.Condition <= 0) // Se a arma quebrar, droppar ela
                                            {
                                                // Desequipa a arma
                                                player.WeaponUnequip();
                                                Console.WriteLine("Your weapon broke.\n");
                                            }

                                            Console.WriteLine($"You dealt {_damage.ToString("F2", CultureInfo.InvariantCulture)} damage to the {mobFound.Name}.\n");
                                            break;

                                        case 3:
                                            Console.WriteLine(player);
                                            break;

                                        default:
                                            Console.WriteLine("Invalid action!\n");
                                            continue;
                                    }

                                    // Morte do inimigo
                                    if (mobFound.Life <= 0)
                                    {
                                        Console.WriteLine($"You defeated {mobFound.Name}.\n");

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
                                                Console.WriteLine(
                                                    $"The {mobFound.Name} dropped a {mobFound.Weapons.Name}.\n" +
                                                    $"Would you like to equip it?\n" +
                                                    $"0 - No\n" +
                                                    $"1 - Yes");
                                                Console.Write(">> ");
                                                answer = byte.Parse(Console.ReadLine());

                                                switch (answer)
                                                {
                                                    case 0:
                                                        Console.WriteLine("You ignored the weapon and went ahead in your journey.\n");
                                                        break;

                                                    case 1:
                                                        player.WeaponEquip(mobFound.Weapons, mobFound.Weapons.NecessaryLvl);
                                                        break;

                                                    default:
                                                        Console.WriteLine("Invalid action!\n");
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
                                                    Console.WriteLine($"The {mobFound.Name} is running away!");
                                                    Console.WriteLine(
                                                        "0- Allow\n" +
                                                        "1- Run toward it!");
                                                    Console.Write(">> ");
                                                    answer = byte.Parse(Console.ReadLine());

                                                    switch (answer)
                                                    {
                                                        case 0:
                                                            Console.WriteLine("I'll allow you escape today.");
                                                            Fighting(false, player, mobFound); // Tira os dois do modo de luta
                                                            break;

                                                        // Verifica se você consegue evitar a fuga do inimigo ou não
                                                        case 1:
                                                            if (mobFound.EscapeChance < mobFound.EscapeChance / Constants.EscapeChance)
                                                            {
                                                                Console.WriteLine("I catched it! Let's continue the fight!\n");
                                                            }
                                                            else
                                                            {
                                                                Console.WriteLine($"The {mobFound.Name} escaped!\n");
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
                                                    double mobDamage = Atack(random, mobFound, player, mobFound.Weapons, weapons);
                                                    Console.WriteLine($"\n{mobFound.Name} dealt {mobDamage.ToString("F2", CultureInfo.InvariantCulture)} damage to you.\n");
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

                            Console.WriteLine("A Merchant!\n");
                            while (true)
                            {
                                Console.WriteLine(
                                    "0: Ignore\n" +
                                    "1: Verify\n");
                                Console.Write(">> ");
                                answer = byte.Parse(Console.ReadLine());

                                if (answer == 0)
                                {
                                    Console.WriteLine("I don't need to buy anything.\n");
                                    break;
                                }

                                // Escolha
                                switch (answer)
                                {
                                    case 1:
                                        Console.WriteLine(
                                            "[ITEMS]\n" +
                                            $"[I have {player.Coins.ToString("F2", CultureInfo.InvariantCulture)} coins]\n" +
                                            "0- Leave\n" +
                                            "1- HP Potion ($5)\n" +
                                            $"2- {weaponFound.Name} (${weaponPrice.ToString("F2", CultureInfo.InvariantCulture)})\n");
                                        Console.Write(">> ");
                                        answer = byte.Parse(Console.ReadLine());

                                        if (answer == 0) break;
                                        switch (answer)
                                        {
                                            case 1:

                                                if (player.Buy(5))
                                                {
                                                    player.Cure(5);
                                                    Console.WriteLine($"[LIFE] {player.Life.ToString("F2", CultureInfo.InvariantCulture)}\n");
                                                }
                                                else
                                                {
                                                    Console.WriteLine("I don't have enough coins!");
                                                }
                                                continue;

                                            case 2:

                                                if (player.Buy(weaponPrice) && player.WeaponEquip(weaponFound, weaponFound.NecessaryLvl))
                                                {
                                                    Console.WriteLine("You bought and equiped the weapon.\n");
                                                }
                                                else
                                                {
                                                    Console.WriteLine("I don't have enough coins!");
                                                }
                                                continue;

                                            default:
                                                Console.WriteLine("Invalid action!\n");
                                                continue;
                                        }

                                    default:
                                        Console.WriteLine("Invalid action!\n");
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
                                Console.WriteLine($"A can with full coins!");
                                Console.WriteLine("Would you like to collect this treasury?");
                                Console.WriteLine("" +
                                    "0 - No" +
                                    "1 - Yes");
                                Console.Write(">> ");
                                answer = byte.Parse(Console.ReadLine());

                                switch (answer)
                                {
                                    case 0:
                                        break;

                                    case 1:
                                        player.GetCoins(coins);
                                        Console.WriteLine($"I collected {coins.ToString("F2", CultureInfo.InvariantCulture)} coins.\n");
                                        break;

                                    default:
                                        Console.WriteLine("Invalid action!\n");
                                        continue;

                                }

                            }

                            #endregion

                            else
                            {
                                Console.WriteLine("Error! Adventure actions invalid!");
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
                                Console.WriteLine("I have no weapons!");
                            }
                            break;

                        case 4:
                            Console.WriteLine(player.Description);
                            break;

                        default:
                            Console.WriteLine("Invalid action!\n");
                            break;
                    }
                }
            }

            Console.WriteLine("Thanks so much for have played!");
            Console.WriteLine("Shutting down...");
            #endregion
        }

        #region Menu
        public static bool Menu()
        {
            Console.WriteLine("Wellcome to Varisten!");
            Console.WriteLine("Here is a magic world with so many adventures to go.");
            Console.WriteLine("[MENU]");
            Console.WriteLine("1- Play \n0- Exit");
            Console.Write(">> ");
            byte n = byte.Parse(Console.ReadLine());

            if (n == 1) return true;

            return false;
        }

        public static void GameOver()
        {
            Console.WriteLine("You died!\nGAME OVER!\n");
        }

        public static Mob RaceChoose(Mob[] race, string name)
        {
            byte choose;
            Mob raceChose = null;

            Console.WriteLine("Now, tell me which race would you like?");
            while (raceChose == null)
            {
                Console.WriteLine(
                    "\n[Races]\n" +
                    $"0 - {race[0].Race}\n" +
                    $"1 - {race[1].Race}");
                Console.Write(">> ");
                choose = byte.Parse(Console.ReadLine());

                race[choose].Name = name;
                Console.WriteLine($"\n{race[choose]}\n{race[choose].Description}\n");

                Console.WriteLine(
                    "0 - Return\n" +
                    "1 - Confirm");
                Console.Write(">> ");
                switch (byte.Parse(Console.ReadLine()))
                {
                    case 0:
                        continue;

                    case 1:
                        raceChose = race[choose];
                        break;

                    default:
                        Console.WriteLine("Invalid action!");
                        continue;
                }
            }
            return raceChose;
        }

        public static byte ByteAnswer()
        {
            Console.WriteLine(">> ");
            byte answer = byte.Parse(Console.ReadLine());
            return answer;
        }

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
        public static bool Dodge(Random r, Mob _mob)
        {
            double dodgeChance = 20;
            if (RandomDouble(r, 0d, dodgeChance) <= _mob.Dodge)
            {
                Console.WriteLine($"[{_mob.Name} DODGED]!!");
                return true;
            }
            return false;
        }

        // Ataque
        public static double Atack(Random r, Mob mobSet, Mob mobGet, Weapon mobWeapon, Weapon[] w)
        {
            if (!Dodge(r, mobGet))
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
                    Console.WriteLine("[Critical]!!\n");
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
