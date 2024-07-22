using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Game.Classes
{
    internal class Tools
    {
        public LanguagesManager Language { get; private set; }

        public Tools(LanguagesManager language)
        {
            this.Language = language;
        }

        public static double RandomDouble(double max, double min = 0)
        {
            Random r = new Random();
            return min + (r.NextDouble() * (max - min));
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

        public static void InvalidAction(LanguagesManager s)
        {
            // mostra ação invalida
            s.ShowSubtitle(s.GetSubtitle("Error", "invalidAction"));
        }
    }
}
