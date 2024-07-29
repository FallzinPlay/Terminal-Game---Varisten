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
        private static readonly Random R = new Random();

        public static double RandomDouble(double max, double min = 0)
        {
            return min + (R.NextDouble() * (max - min));
        }

        public static bool RandomChance(double chance)
        {
            double totalChance = RandomDouble(20);
            bool result = totalChance <= chance;
            return result;
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
                    s.ShowSubtitle(" ");

                    if (answer >= max || answer < 0)
                    {
                        continue;
                    }

                    return answer;
                }
                catch (Exception)
                {
                    s.ShowSubtitle(" ");
                }
            }
        }

     
    }
}
