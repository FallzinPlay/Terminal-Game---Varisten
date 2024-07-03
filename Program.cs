using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();
            int action;
            int randomChoose;

            // Armas
            Weapon[] weapons = new Weapon[4];
            weapons[0] = new Weapon("Hand", 0, 1, 1);
            weapons[1] = new Weapon("Stick", 3, 4, 1);
            weapons[2] = new Weapon("Wooden sword", 4, 5, 2);
            weapons[3] = new Weapon("Wooden bow", 4, 6, 3);
            Weapon playerWeapon = weapons[0];
            Weapon enemyWeapon = weapons[0];
            Weapon weaponFound;


            // Entidades
            Mob[] mobs = new Mob[3];
            mobs[0] = new Mob("Zombie", 0, 2, 7, 7, 2, 1);
            mobs[1] = new Mob("Skeleton", 0, 3, 5, 5, 3, weapons[3].Name, weapons[3].NecessaryLvl, 4);
            mobs[2] = new Mob("Slime", 0, 2, 4, 10, 2, 3);
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

                                randomChoose = random.Next(0, weapons.Length); // Sorteia um número
                                weaponFound = weapons[randomChoose]; // Pega a arma de acordo com o número sorteado
                                weaponFound.Condition = random.Next(1, weaponFound.MaxCondition); // Determina a condição da arma aleatoriamente

                                Console.WriteLine("You have found a weapon!\n");

                                while (true)
                                {
                                    Console.WriteLine(
                                        "0: Ignore\n" +
                                        "1: Verify\n" +
                                        "2: Equip");
                                    Console.Write(">> ");
                                    answer = byte.Parse(Console.ReadLine());

                                    if (answer == 0)
                                    {
                                        Console.WriteLine("You ignore the weapon and continue your exploration.\n");
                                        break;
                                    }

                                    // Escolha
                                    switch (answer)
                                    {
                                        case 1:
                                            Console.WriteLine("\n" + weaponFound + "\n");
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

                                randomChoose = random.Next(1, mobs.Length); // Sorteia um número
                                mobFound = mobs[randomChoose]; // Seleciona um mob aleatório
                                mobFound.Cure((sbyte)random.Next(3, mobFound.MaxLife)); // Vida dos mobs determinadas aleatóriamente

                                // Configuração de arma dos mobs
                                if (mobFound.Name == "Skeleton") enemyWeapon = weapons[3]; // Equipa o esqueleto com um arco

                                sbyte coins = (sbyte)random.Next(5);
                                mobFound.Coins = coins;

                                double xp = (double)random.Next(5, 20);

                                Console.WriteLine($"You met a {mobFound.Name}!\n");

                                char escaped = 'y';
                                while (true)
                                {
                                    if (player.Life <= 0) break;

                                    Console.WriteLine("" +
                                        "0: Get away\n" +
                                        "1: Verify\n" +
                                        "2: Atack\n" +
                                        "3- My stats\n");
                                    Console.Write(">> ");
                                    answer = byte.Parse(Console.ReadLine());

                                    if (answer == 0)
                                    {
                                        // Sistema de chances de conseguir escapar
                                        int opportunity = random.Next(0, 3);

                                        if (opportunity != 3)
                                        {
                                            // Verifica se o jogador já tentou escapar
                                            if (escaped == 'y')
                                            {
                                                Console.WriteLine($"You got away far from {mobFound.Name}.\n");
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            escaped = 'n';
                                            Console.WriteLine("You couldn't escape. You have to fight!");
                                        }
                                    }

                                    // Escolha
                                    switch (answer)
                                    {
                                        case 1:
                                            Console.WriteLine("\n" + mobFound);
                                            break;

                                        case 2:
                                            sbyte _damage = Atack(random, player, mobFound, playerWeapon);
                                            if (player.WeaponEquiped == true && playerWeapon.Condition <= 0) // Se a arma quebrar, dropar ela
                                            {
                                                // Desequipa a arma
                                                player.WeaponUnequip();
                                                playerWeapon = weapons[0];
                                                Console.WriteLine("Your weapon have broke.");
                                            }

                                            Console.WriteLine($"You have taked {_damage} of damage!\n");
                                            break;

                                        case 3:
                                            Console.WriteLine(player);
                                            break;

                                        default:
                                            Console.WriteLine("Invalid action!\n");
                                            continue;
                                    }

                                    // Turno do mob
                                    if (answer == 2)
                                    {
                                        if (mobFound.Life > 0) // Se ele tiver vivo
                                        {
                                            sbyte mobDamage = Atack(random, mobFound, player, enemyWeapon);
                                            Console.WriteLine($"\n{mobFound.Name} have taked {mobDamage} of damage.\n");
                                        }
                                    }

                                    // Morte do inimigo
                                    if (mobFound.Life <= 0)
                                    {
                                        escaped = 'y';
                                        Console.WriteLine($"You defeated {mobFound.Name}.\n");

                                        #region Chance de drop
                                        // Chance do mob droppar a arma que ele está usando
                                        if (mobFound.WeaponEquiped == true)
                                        {
                                            int dropChance = random.Next(3);
                                            if (dropChance == 3)
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
                                                        Console.WriteLine("You ignore the weapon and go ahead in your journey.\n");
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

                                        player.GetXp(xp);

                                        player.Coins += mobFound.Coins;
                                        Console.WriteLine($"Coins received: {mobFound.Coins}\n");

                                        break;
                                    }
                                }
                            }
                            #endregion

                            #region Comerciante
                            else if (action <= 9)
                            {

                                randomChoose = random.Next(0, weapons.Length - 1); // Sorteia um número
                                weaponFound = weapons[randomChoose]; // Pega a arma de acordo com o número sorteado
                                weaponFound.Condition = weaponFound.MaxCondition;

                                sbyte weaponPrice = (sbyte)random.Next(5, 20);

                                Console.WriteLine("You have found a merchant.\n");
                                while (true)
                                {
                                    Console.WriteLine("" +
                                        "0: Ignore\n" +
                                        "1: Verify\n");
                                    Console.Write(">> ");
                                    answer = byte.Parse(Console.ReadLine());

                                    if (answer == 0)
                                    {
                                        Console.WriteLine("You ignore the merchant and continue your exploration.\n");
                                        break;
                                    }

                                    // Escolha
                                    switch (answer)
                                    {
                                        case 1:
                                            Console.WriteLine("" +
                                                "[ITEMS]\n" +
                                                $"[You have {player.Coins} coins]\n" +
                                                "0- Leave\n" +
                                                "1- HP Potion ($5)\n" +
                                                $"2- {weaponFound.Name} (${weaponPrice})\n");
                                            Console.Write(">> ");
                                            answer = byte.Parse(Console.ReadLine());

                                            if (answer == 0) break;
                                            switch (answer)
                                            {
                                                case 1:

                                                    if (player.Buy(5))
                                                    {
                                                        player.Cure(5);
                                                        Console.WriteLine($"[LIFE] {player.Life}\n");
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("You don't have enough coins!");
                                                    }
                                                    continue;

                                                case 2:

                                                    if (player.Buy(weaponPrice) && player.WeaponEquip(weaponFound.Name, weaponFound.NecessaryLvl))
                                                    {
                                                        playerWeapon = weaponFound;
                                                        Console.WriteLine("You bought and equipped the weapon.\n");
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("You don't have enough coins!");
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
                                randomChoose = random.Next(5, 20); // Sorteia um número
                                Console.WriteLine($"You have found a can with {randomChoose} coins!");
                                Console.WriteLine("Would you like to get this money?");
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
                                        player.Coins += (sbyte)randomChoose;
                                        Console.WriteLine($"you have collected {randomChoose} coins.\n");
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
                                Console.WriteLine("You have no weapons!");
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
            sbyte dodgeChance = 20;
            if ((sbyte)r.Next(0, dodgeChance) <= _mob.Dodge)
            {
                Console.WriteLine($"[{_mob.Name} DODGED]!!");
                return true;
            }
            return false;
        }

        // Ataque
        public static sbyte Atack(Random r, Mob mobSet, Mob mobGet, Weapon mobWeapon)
        {
            if (!Dodge(r, mobGet))
            {
                sbyte damage = mobSet.Damage;

                // Se estiver equipando uma arma, somar ataque
                if (mobSet.WeaponEquiped && mobWeapon.Name != "Hand")
                {
                    damage = mobWeapon.Damage;
                    mobWeapon.Condition--;
                }

                // Chance de crítico
                if (r.Next(mobSet.CriticChance) == 0)
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
    }
}
