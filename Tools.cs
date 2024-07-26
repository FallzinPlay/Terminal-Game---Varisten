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
                        InvalidAction(s);
                        continue;
                    }

                    return answer;
                }
                catch (Exception)
                {
                    InvalidAction(s);
                    s.ShowSubtitle(" ");
                }
            }
        }

        public static void InvalidAction(LanguagesManager s)
        {
            // mostra ação invalida
            s.ShowSubtitle(s.GetSubtitle("Error", "invalidAction"));
        }

        public static WeaponCreate RandomWeapon(EntityRegistry register, LanguagesManager s, WeaponCreate[] weapons, bool randomCondition = false)
        {
            int randomWeapon = R.Next(1, weapons.Length); // Sorteia um número
            WeaponCreate weaponGenerate = weapons[randomWeapon]; // Pega a arma de acordo com o número sorteado
            WeaponCreate weaponFound = new WeaponCreate(register, s, weaponGenerate.Name, weaponGenerate.Damage, weaponGenerate.Condition, weaponGenerate.NecessaryLvl, weaponGenerate.MinPrice, weaponGenerate.MaxPrice);
            weaponFound.Erode(randomCondition); // Determina a condição da arma

            return weaponFound;
        }

        public static MobCreate RandomMob(MobCreate[] mobs, WeaponCreate[] weapons, bool randomLvl = false, int lvlMin = 1)
        {
            int randomMob = R.Next(mobs.Length); // Sorteia um número
            MobCreate mobFound = mobs[randomMob]; // Seleciona um mob aleatório
            mobFound.GetDamage(mobFound.MaxLife);

            // Weapons
            WeaponCreate mobWeapon = weapons[0];
            if (mobFound.Name == mobs[1].Name) mobWeapon = weapons[3]; // Skeleton

            // Parelha o lvl dos inimigos
            if (randomLvl)
            {
                if (mobWeapon.Name != weapons[0].Name) lvlMin = mobWeapon.NecessaryLvl;
                int mobLvl = Tools.R.Next(lvlMin, lvlMin + 3);
                mobFound.LvlUp(mobLvl);
            }

            mobFound.WeaponEquip(mobWeapon);
            mobFound.Cure(RandomDouble(mobFound.MaxLife, mobFound.MaxLife / 2));
            mobFound.GetCoins(Tools.RandomDouble(10d));
            mobFound.GetXp(Tools.RandomDouble(mobFound.NextLvlXp / 2, 10d));

            return mobFound;
        }
    }
}
