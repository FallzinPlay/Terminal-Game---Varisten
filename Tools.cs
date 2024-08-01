using Game.ClassManager;
using Game.Entities;
using Game.Items.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
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
            double totalChance = RandomDouble(25);
            bool result = totalChance <= chance;
            return result;
        }

        public static MobCreate RandomMob(EntityRegistry register, MobCreate player)
        {
            MobCreate mob = null;
            while (true)
            {
                switch (R.Next(3))
                {
                    case 0:
                        mob = new Zombie();
                        break;

                    case 1:
                        mob = new Skeleton();
                        break;

                    case 2:
                        mob = new Slime();
                        break;
                }
                if (mob != null) break;
            }

            // Mob spawn using a weapon chance
            if (mob.Weapon != null)
                mob.LvlUp(mob.Weapon.NecessaryLvl);
            else
                if (R.Next(5) < 2) mob.WeaponEquip(RandomWeapon(register));

            // Adjusting mob's lvl
            int _lvlMin = mob.Lvl;
            if (player.Lvl > _lvlMin)
                _lvlMin = player.Lvl;
            mob.LvlUp(R.Next(_lvlMin, _lvlMin + 3));

            // Randomizing mob's life
            mob.Cure(RandomDouble(mob.MaxLife, mob.MaxLife / 2));

            // Randomizing mob's xp
            mob.GetXp(RandomDouble(mob.NextLvlXp / 2, 10.0d));

            // Radomizing mob's coins
            mob.GetCoins(RandomDouble(mob.Lvl * 2.0d, mob.Lvl * 1.5d));

            register.AddEntity(mob);
            return mob;
        }

        public static WeaponCreate RandomWeapon(EntityRegistry register)
        {
            WeaponCreate weapon = null;
            while (true)
            {
                switch (R.Next(3))
                {
                    case 0:
                        weapon = new Stick();
                        break;

                    case 1:
                        weapon = new WoodenSword();
                        break;

                    case 2:
                        weapon = new WoodenBow();
                        break;
                }
                if (weapon != null) break;
            }
            weapon.Erode(true);

            register.AddEntity(weapon);
            return weapon;
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
