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
    }
}
