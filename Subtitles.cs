using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    internal class Subtitles
    {
        public string SetName { get; set; }
        public string Wellcome { get; set; }
        public string GreatChoose { get; set; }
        public string MenuOptions { get; set; }
        public string MenuWeaponFound { get; set; }
        public string MenuMobFound { get; set; }
        public string Exiting { get; set;  }

        public Subtitles()
        {
            this.SetName = "Hey! Let's create your character!" +
                        "First write their name:";

            this.Wellcome = $"Alright, new adventurer! Now we're going to go to into that forest.\n" +
                        "But be careful, there are monsters over there.\n" +
                        "Let's go!\n";

            this.GreatChoose = "Great choose!";

            this.MenuOptions = "[ACTION]\n" +
                        "0- Exit\n" +
                        "1- Explore\n" +
                        "2- Status\n" +
                        "3- Weapon\n" +
                        "4- Description";

            this.MenuWeaponFound = "0: Ignore\n" +
                        "1: Compare\n" +
                        "2: Equip";

            this.MenuMobFound = "0: Run away\n" +
                        "1: Analize\n" +
                        "2: Atack!\n" +
                        "3- My status\n";

            this.Exiting = "Leaving from the adventure...";
        }

        public void Print(string s)
        {
            Console.WriteLine(s);
        }
    }
}
