using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Classes.Entities
{
    internal class Weapon
    {
        public LanguagesManager s;
        public WeaponCreate None { get; set; }
        public WeaponCreate Stick { get; set; }
        public WeaponCreate WoodenSword {  get; set; }
        public WeaponCreate WoodenBow {  get; set; }

        public Weapon(LanguagesManager s)
        {
            this.s = s;

            // idioma, nome, dano, condição, nivel necessario, preço min, preço max
            this.None = new WeaponCreate(s, "--", 0d, 0, 0, 0d, 0d);
            this.Stick = new WeaponCreate(s, s.GetSubtitle("Weapons", "stick"), 3.2d, 4, 1, 5d, 10d);
            this.WoodenSword = new WeaponCreate(s, s.GetSubtitle("Weapons", "woodenSword"), 3.5d, 5, 2, 7d, 12d);
            this.WoodenBow = new WeaponCreate(s, s.GetSubtitle("Weapons", "woodenBow"), 4.2d, 6, 3, 10d, 15d);
        }
    }
}
