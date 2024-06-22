using System;
using System.Collections.Generic;
using System.Linq;
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

            // Entidades
            Mob[] mobs = new Mob[4];
            mobs[1] = new Mob("Zombie", 0, 2, 7, 7, 2);
            mobs[2] = new Mob("Skeleton", 0, 3, 5, 5, 3);
            mobs[3] = new Mob("Slime", 0, 2, 4, 10, 2);
            Mob mobFound;

            // Armas
            Weapon[] weapons = new Weapon[2];
            weapons[0] = new Weapon("Stick", 3, 4);
            weapons[1] = new Weapon("Wooden sword", 4, 5);
            Weapon weaponFound;

            byte answer;


            while (Menu())
            {
                // Apresentação do jogo e criação de personagem
                Console.WriteLine("Hey! Let's create your character!");
                Console.Write("First write their name: ");
                mobs[0] = new Mob(Console.ReadLine(), 10); // Cria o jogador
                Console.WriteLine($"Alright, {mobs[0].Name}! Now we're going to go to into that forest.\nBut be careful, there are monsters over there.\nLet's go!\n");


                // Loop do jogo
                while (true)
                {
                    // Acaba o jogo se o jogador morrer
                    if (mobs[0].Life <= 0)
                    {
                        Console.WriteLine("You died!\nGAME OVER!\n");
                        break;
                    }

                    Console.WriteLine("" +
                        "[ACTION]\n" +
                        "0- Exit\n" +
                        "1- Explore\n" +
                        "2- Inventory\n");
                    Console.Write(">> ");

                    action = int.Parse(Console.ReadLine());

                    if (action == 0) break;

                    switch (action)
                    {
                        #region Aventura
                        case 1:

                            action = random.Next(3);
                            switch (action)
                            {
                                #region Weapon
                                case 0:

                                    randomChoose = random.Next(0, weapons.Length - 1); // Sorteia um número
                                    weaponFound = weapons[randomChoose]; // Pega a arma de acordo com o número sorteado
                                    weaponFound.Condition = random.Next(weaponFound.MaxCondition); // Determina a condição da arma aleatoriamente

                                    Console.WriteLine("You have found a weapon!\n");

                                    while (true)
                                    {
                                        Console.WriteLine("0: Ignore\n1: Verify\n2: Equip");
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
                                                Console.WriteLine("\n" + weaponFound);
                                                break;

                                            case 2:
                                                mobs[0].WeaponEquip(weaponFound.Name, weaponFound.Damage);
                                                Console.WriteLine("You equiped.\n");
                                                break;

                                            default:
                                                Console.WriteLine("Invalid action!\n");
                                                continue;
                                        }

                                        if (answer != 1) break;
                                    }
                                    break;
                                #endregion

                                #region Mob
                                case 1:

                                    randomChoose = random.Next(1, weapons.Length - 1); // Sorteia um número
                                    mobFound = mobs[randomChoose]; // Seleciona um mob aleatório
                                    mobFound.Cure((sbyte)random.Next(mobFound.MaxLife)); // Determina a vida inicial do mob

                                    sbyte coins = (sbyte)random.Next(5);
                                    mobFound.GetCoin(coins);

                                    Console.WriteLine($"You have met with a {mobFound.Name}!\n");

                                    while (true)
                                    {
                                        if (mobFound.Life <= 0)
                                        {
                                            Console.WriteLine($"You have defeated {mobFound.Name}.\n");

                                            #region Chance de drop


                                            #endregion

                                            mobs[0].GetCoin(mobFound.Coins);
                                            Console.WriteLine($"Coins received: {coins}\n");

                                            break;
                                        }

                                        Console.WriteLine("" +
                                            "0: Get away\n" +
                                            "1: Verify\n" +
                                            "2: Atack\n" +
                                            "3- My stats\n");
                                        Console.Write(">> ");
                                        answer = byte.Parse(Console.ReadLine());

                                        // Escolha
                                        switch (answer)
                                        {
                                            case 1:
                                                Console.WriteLine("\n" + mobFound);
                                                break;

                                            case 2:
                                                sbyte _damage = mobs[0].TakeDamage();
                                                mobFound.GetDamage( _damage );

                                                if (mobs[0].WeaponEquiped) // Fazer um sistema de desgaste da arma

                                                Console.WriteLine($"You have taked {_damage} of damage!\n");
                                                break;

                                            case 3:
                                                Console.WriteLine(mobs[0]);
                                                break;

                                            default:
                                                Console.WriteLine("You ignore the weapon and continue your exploration.\n");
                                                break;
                                        }

                                        // Turno do mob
                                        if (answer == 2)
                                        {
                                            if (mobFound.Life > 0) // Se ele tiver vivo
                                            {
                                                sbyte mobDamage = mobFound.TakeDamage();
                                                mobs[0].GetDamage(mobDamage);
                                                Console.WriteLine($"\n{mobFound.Name} have taked {mobDamage} of damage.\n");
                                            }
                                        }
                                    }
                                    break;
                                #endregion

                                #region Comerciante
                                case 2:

                                    randomChoose = random.Next(0, weapons.Length - 1); // Sorteia um número
                                    weaponFound = weapons[randomChoose]; // Pega a arma de acordo com o número sorteado
                                    weaponFound.Condition = random.Next(weaponFound.MaxCondition); // Determina a condição da arma aleatoriamente

                                    sbyte weaponPrice = (sbyte)random.Next(10, 40);

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
                                                    "0- Leave\n" +
                                                    "1- HP Potion ($5)\n" +
                                                    $"2- {weaponFound.Name} (${weaponPrice})\n");
                                                Console.Write(">> ");
                                                answer = byte.Parse(Console.ReadLine());

                                                if (answer == 0) break;
                                                switch (answer)
                                                {
                                                    case 1:

                                                        if (mobs[0].Buy(5))
                                                        {
                                                            mobs[0].Cure(5);
                                                            Console.WriteLine($"[LIFE] {mobs[0].Life}\n");
                                                        }
                                                        break;

                                                    case 2:

                                                        if (mobs[0].Buy(weaponPrice))
                                                        {
                                                            mobs[0].WeaponEquip(weaponFound.Name, weaponFound.Damage);
                                                            Console.WriteLine("You bought and equipped the weapon.\n");
                                                        }
                                                        break;
                                                }

                                                break;

                                            default:
                                                Console.WriteLine("Invalid action!\n");
                                                continue;
                                        }

                                        if (answer != 1) break;
                                    }
                                    break;
                                #endregion

                                #region
                                case 3:

                                    Console.WriteLine("You have found a full gold can!\n");

                                    break;

                                #endregion

                                default:
                                    break;
                            }

                            weaponFound = null;
                            mobFound = null;
                            break;
                        #endregion

                        case 2:

                            Console.WriteLine(mobs[0]);
                            break;

                        case 3:
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
    }
}
