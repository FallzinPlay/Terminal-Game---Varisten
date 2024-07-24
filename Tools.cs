using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    internal class Tools
    {
        public static double RandomDouble(double max, double min = 0)
        {
            Random r = new Random();
            return min + (r.NextDouble() * (max - min));
        }

        public static Random Random()
        {
            return new Random();
        }

        public static int Answer(LanguagesManager s, string options = null, int max = 2)
        {
            int answer;
            while (true)
            {
                if (options != null) s.ShowSubtitle(options);
                try
                {
                    Console.Write(">> ");
                    answer = int.Parse(Console.ReadLine());

                    if (answer >= max || answer < 0)
                    {
                        InvalidAction(s);
                        continue;
                    }

                    return answer;
                }
                catch (Exception)
                {
                    InvalidAction(s);
                }
            }
        }

        public static void InvalidAction(LanguagesManager s)
        {
            // mostra ação invalida
            s.ShowSubtitle(s.GetSubtitle("Error", "invalidAction"));
        }

        public static WeaponCreate RandomWeapon(WeaponCreate[] weapons, bool randomCondition = false)
        {
            int randomWeapon = Random().Next(1, weapons.Length); // Sorteia um número
            WeaponCreate weaponFound = weapons[randomWeapon]; // Pega a arma de acordo com o número sorteado
            weaponFound.Erode(randomCondition); // Determina a condição da arma

            return weaponFound;
        }
    }
}
