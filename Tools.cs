﻿using System;
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
            double a = RandomDouble(chance);
            double b = chance / 3;
            Console.WriteLine($"{a} {b}");
            return a < b;
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

        public static WeaponCreate RandomWeapon(WeaponCreate[] weapons, bool randomCondition = false)
        {
            int randomWeapon = R.Next(1, weapons.Length); // Sorteia um número
            WeaponCreate weaponFound = weapons[randomWeapon]; // Pega a arma de acordo com o número sorteado
            weaponFound.Erode(randomCondition); // Determina a condição da arma

            return weaponFound;
        }

        public static MobCreate RandomMob(MobCreate[] mobs, WeaponCreate[] weapons, bool randomLvl = false, int lvlMin = 1)
        {
            int randomMob = R.Next(mobs.Length); // Sorteia um número
            MobCreate mobFound = mobs[randomMob]; // Seleciona um mob aleatório

            // Weapons
            WeaponCreate mobWeapon = weapons[0];
            if (mobFound.Name == mobs[1].Name) mobWeapon = weapons[3]; // Skeleton

            // Parelha o lvl dos inimigos
            if (randomLvl)
            {
                if (mobWeapon.Name != weapons[0].Name) lvlMin = mobWeapon.NecessaryLvl;
                mobFound.LvlUp(Tools.R.Next(lvlMin, lvlMin + 3));
            }

            mobFound.WeaponEquip(mobWeapon);
            mobFound.Cure(Tools.RandomDouble(mobFound.MaxLife, mobFound.MaxLvl / 2));
            mobFound.GetCoins(Tools.RandomDouble(10d));

            return mobFound;
        }
    }
}
