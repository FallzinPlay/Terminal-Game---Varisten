using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Game
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Proxima atualização
            //--------!! Adicionar biomas !!-----------//

            Random random = new Random();
            int action;
            int randomChoose;

            // Armas
            Weapon[] weapons = new Weapon[4];
            // nome, dano, condição, nivel necessario
            weapons[0] = new Weapon("Hand", 2.5d, 1, 1, 0d, 0d);
            weapons[1] = new Weapon("Stick", 3.2d, 4, 1, 5d, 10d);
            weapons[2] = new Weapon("Wooden sword", 4.5d, 5, 2, 7d, 12d);
            weapons[3] = new Weapon("Wooden bow", 5d, 6, 3, 10d, 15d);
            Weapon playerWeapon = weapons[0];
            Weapon enemyWeapon = weapons[0];
            Weapon weaponFound;


            // Entidades
            Mob[] mobs = new Mob[3];
            // nome, vida, dano, vida maxima, chace de critico, dano de critico, arma, nivel, esquiva
            mobs[0] = new Mob("Zombie", 0, 2.2d, 7, 1.1d, 1.7d, null, weapons[0].NecessaryLvl, 0.8d);
            mobs[1] = new Mob("Skeleton", 0, 3.2d, 5, 2.2d, 2.3d, weapons[3].Name, weapons[3].NecessaryLvl, 2.1d);
            mobs[2] = new Mob("Slime", 0, 2.7d, 4, 3.5d, 2.7d, null, weapons[0].NecessaryLvl, 0.5d);
            Mob mobFound;

            byte answer;


            while (Menu())
            {
                // Apresentação do jogo e criação de personagem
                Console.WriteLine("Hey! Let's create your character!");
                Console.Write("First write their name: ");
                Mob player = new Mob(Console.ReadLine(), 10); // Cria o jogador
                Console.WriteLine(
                    $"Alright, {player.Name}! Now we're going to go to into that forest.\n" +
                    $"But be careful, there are monsters over there.\n" +
                    $"Let's go!\n");


                // Loop do jogo
                while (true)
                {
                    // Acaba o jogo se o jogador morrer
                    if (player.Life <= 0)
                    {
                        Console.WriteLine("You died!\nGAME OVER!\n");
                        break;
                    }

                    Console.WriteLine("" +
                        "[ACTION]\n" +
                        "0- Exit\n" +
                        "1- Explore\n" +
                        "2- Status\n" +
                        "3- Weapon\n");
                    Console.Write(">> ");

                    action = int.Parse(Console.ReadLine());

                    if (action == 0) break;

                    switch (action)
                    {
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
                                    Console.WriteLine(
                                        "0: Ignore\n" +
                                        "1: Compare\n" +
                                        "2: Equip");
                                    Console.Write(">> ");
                                    answer = byte.Parse(Console.ReadLine());

                                    if (answer == 0)
                                    {

                                        break;
                                    }

                                    // Escolha
                                    switch (answer)
                                    {
                                        case 0:
                                            Console.WriteLine("I don't need it for now...\n");
                                            break;
                                            
                                        case 1:
                                            if (player.WeaponEquiped != false)
                                            {
                                                Console.WriteLine("\n[My weapon]\n" + playerWeapon + "\n\n[Weapon Found]\n" + weaponFound + "\n");
                                            }
                                            else
                                            {
                                                Console.WriteLine("\nI don't have any weapon...\n" + weaponFound + "\n");
                                            }
                                            break;

                                        case 2:
                                            // Equipa a arma
                                            if (player.WeaponEquip(weaponFound.Name, weaponFound.NecessaryLvl)) playerWeapon = weaponFound;
                                            break;

                                        default:
                                            Console.WriteLine("Invalid action!\n");
                                            continue;
                                    }

                                    if (answer != 1) break;
                                }
                            }
                            #endregion

                            #region Mob
                            else if (action <= 7)
                            {

                                randomChoose = random.Next(0, mobs.Length); // Sorteia um número
                                mobFound = mobs[randomChoose]; // Seleciona um mob aleatório
                                while (mobFound.Life < 1) mobFound.Cure(RandomDouble(random, (double)mobFound.MaxLife/2, (double)mobFound.MaxLife)); // Vida dos mobs determinadas aleatóriamente e impede de ser 0

                                // Configuração de arma dos mobs
                                if (mobFound.Name == mobs[1].Name) enemyWeapon = weapons[3]; // Equipa o esqueleto com um arco

                                double coins = RandomDouble(random, 0d, 10d);
                                mobFound.Coins = coins;

                                double xp = RandomDouble(random, 5d, 20d);

                                Console.WriteLine($"A {mobFound.Name}! What should I do?\n");

                                bool escaped = true;
                                while (true)
                                {

                                    Console.WriteLine("" +
                                        "0: Run away\n" +
                                        "1: Analize\n" +
                                        "2: Atack!\n" +
                                        "3- My status\n");
                                    Console.Write(">> ");
                                    answer = byte.Parse(Console.ReadLine());

                                    if (answer == 0)
                                    {
                                        // Sistema de chances de conseguir escapar
                                        double escapeChance = 5d;
                                        double opportunity = RandomDouble(random, 0, escapeChance);

                                        if (opportunity > escapeChance / 1.3d)
                                        {
                                            // Verifica se o jogador já tentou escapar
                                            if (escaped == true)
                                            {
                                                Console.WriteLine($"You run away from the {mobFound.Name} as far as you can.\n");
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            escaped = false;
                                            Console.WriteLine("I couldn't escape. I will have to fight!");
                                        }
                                    }

                                    // Escolha
                                    switch (answer)
                                    {
                                        case 1:
                                            Console.WriteLine("\n" + mobFound + "\n");
                                            break;

                                        case 2:
                                            double _damage = Atack(random, player, mobFound, playerWeapon, weapons);
                                            if (player.WeaponEquiped == true && playerWeapon.Condition <= 0) // Se a arma quebrar, droppar ela
                                            {
                                                // Desequipa a arma
                                                player.WeaponUnequip();
                                                playerWeapon = weapons[0];
                                                Console.WriteLine("Your weapon broke.");
                                            }

                                            Console.WriteLine($"You dealt {_damage} damage to the {mobFound.Name}.\n");
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
                                        escaped = true;
                                        Console.WriteLine($"You defeated {mobFound.Name}.\n");

                                        player.GetXp(xp);
                                        player.GetCoins(coins);

                                        #region Chance de drop
                                        // Chance do mob droppar a arma que ele está usando
                                        if (mobFound.WeaponEquiped == true)
                                        {

                                            double dropChance = 3d;
                                            double dropped = RandomDouble(random, 0d, dropChance);
                                            if (dropped > dropChance / 1.3)
                                            {
                                                Console.WriteLine(
                                                    $"The {mobFound.Name} dropped a {enemyWeapon.Name}.\n" +
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
                                                        if (player.WeaponEquip(enemyWeapon.Name, enemyWeapon.NecessaryLvl)) playerWeapon = enemyWeapon;
                                                        break;

                                                    default:
                                                        Console.WriteLine("Invalid action!\n");
                                                        continue;
                                                }
                                            }
                                        }

                                        #endregion
                                        break;
                                    }
                                    else
                                    {
                                        // Turno do mob
                                        if (answer == 2)
                                        {
                                            double mobDamage = Atack(random, mobFound, player, enemyWeapon, weapons);
                                            Console.WriteLine($"\n{mobFound.Name} dealt {mobDamage} damage to you.\n");
                                        }
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

                                                    if (player.Buy(weaponPrice) && player.WeaponEquip(weaponFound.Name, weaponFound.NecessaryLvl))
                                                    {
                                                        playerWeapon = weaponFound;
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
                                Console.WriteLine(playerWeapon);
                            }
                            else
                            {
                                Console.WriteLine("I have no weapons!");
                            }
                            break;

                        default:
                            Console.WriteLine("Invalid action!\n");
                            break;
                    }
                }
            }

            Console.WriteLine("Thanks so much for have played!");
            Console.WriteLine("Shutting down...");
        }

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

        #region Actions


        #endregion

        #region Combat

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
                    damage = mobWeapon.Damage;
                    mobWeapon.Condition--;
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
