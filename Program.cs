using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Net;
using System.Diagnostics;
using Game.ClassManager;
using Game.Entities;
using Game.Items.Weapons;
using Microsoft.Win32;
using Game.Items;

namespace Game
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random R = new Random();

            // Registro de Ids
            EntityRegistry register = new EntityRegistry();
            // Como pegar os objetos por id: WeaponCreate newWeapon = register.GetEntityById<WeaponCreate>(weapon[1].Id);

            // ------------- Choose the game language
            LanguagesManager s = new LanguagesManager();/*
            s.LanguageChoose(
                Tools.Answer(s, "Choose your language: \n0 - English, USA (default) \n1 - Português, BR", s.LanguageOptions.Length));
            // --------------------------------------------------

            //*/

            try
            {
                #region Jogo
                //*
                // criação do jogador
                s.ShowSubtitle(s.GetSubtitle("System", "presentation")); // Pequena apresentação
                Console.Write(">> ");
                string playerName = Console.ReadLine(); // Pega o nome do jogador
                s.ShowSubtitle(s.GetSubtitle("System", "jokeWithName"));
                Player player = new Player(s, playerName);
                register.AddEntity(player);

                // Loop do jogo
                s.ShowSubtitle($"{s.GetSubtitle("System", "wellcome")}\n");

                int answer;
                while (Menu.StartMenu(s))
                {
                    do
                    {
                        // Menu
                        answer = Tools.Answer(s,
                            s.GetSubtitle("Menu", "adventure"),
                            Enum.GetValues(typeof(StartActions)).Length);
                        switch ((StartActions)answer)
                        {
                            case StartActions.Exit:
                                s.ShowSubtitle(s.GetSubtitle("System", "leaving").Replace("#PlayerName", player.Name)); // Saindo do jogo
                                break;

                            case StartActions.Adventure:
                                byte totalActions = 10;
                                double randomAction = 8;// Tools.RandomDouble(totalActions);

                                // Mob
                                if (randomAction < totalActions * 0.5d) MobFound(register, s, R, player);

                                // Merchant
                                else if (randomAction < totalActions * 0.9d) MerchantFound(register, s, player);

                                // Treasury
                                else if (randomAction < totalActions * 1d) TreasuryFound(s, player);

                                break;

                            case StartActions.PlayerBag:
                                // Mostra a arma do player
                                player.BagVerify();
                                break;

                            default:
                                continue;
                        }
                        if (Menu.GameOver(s, player))
                        {
                            player = new Player(s, playerName);
                            register.AddEntity(player);
                            break;
                        }
                    } while (answer > 0);
                }
                s.ShowSubtitle(s.GetSubtitle("Me", "thanks"));
                //*/
                #endregion
            }
            catch (Exception ex)
            {
                s.ShowSubtitle("Error: " + ex);
            }
        }

        public static void MobFound(EntityRegistry register, LanguagesManager s, Random r, Player player)
        {
            int answer;
            MobCreate mobFound = Tools.RandomMob(register, player);
            s.ShowSubtitle(
                s.GetSubtitle("Player", mobFound.Name.ToLower() + "Found") + "\n");
            player.State = MobState.Fighting;
            do
            {
                answer = Tools.Answer(s, s.GetSubtitle("Menu", "mobFound").Replace("#MobName", mobFound.Name), 4);
                switch (answer)
                {
                    case 0:
                        if (!player.TryRunAway())
                            mobFound.State = MobState.Fighting;
                        break;

                    case 1:
                        double _damage = MobAttack(s, player, mobFound);
                        s.ShowSubtitle(
                            s.GetSubtitle("Player", "attack" + mobFound.Name + r.Next(3)));
                        s.ShowSubtitle(
                            s.GetSubtitle("Combat", "getDamage").Replace("#Name", mobFound.Name).Replace("#Damage", _damage.ToString("F2")) +
                            "\n");
                        break;

                    case 2:
                        s.ShowSubtitle(mobFound.ShowInfo(s));
                        continue;

                    case 3:
                        player.BagVerify();
                        continue;

                    default:
                        InvalidOption(s);
                        continue;
                }

                // Morte do inimigo
                if (mobFound.State == MobState.Death)
                {
                    s.ShowSubtitle(
                        $"{s.GetSubtitle(mobFound.Name, "death")}\n");

                    player.GetXp(mobFound.Xp);
                    player.GetCoins(mobFound.Coins);

                    //* Chance de drop
                    if (mobFound.Weapon != null)
                    {
                        double dropChance = 3d;
                        if (Tools.RandomChance(dropChance))
                        {
                            // Mostra o drop do mob
                            answer = Tools.Answer(s,
                                $"{s.GetSubtitle("Subtitles", "mobDrop").Replace("#1", mobFound.Name).Replace("#2", mobFound.Weapon.Name)}" +
                                $"{s.GetSubtitle("Menu", "noYes")}");

                            switch (answer)
                            {
                                case 0:
                                    // Ignora a arma
                                    s.ShowSubtitle(s.GetSubtitle("Subtitles", "weaponIgnore"));
                                    break;

                                case 1:
                                    // Equipa a arma do inimigo
                                    player.WeaponEquip(mobFound.Weapon);
                                    break;

                                default:
                                    // Mostra o erro de ação invalida
                                    continue;
                            }
                        }
                    }
                    //*/
                    player.State = MobState.Exploring;
                }
                else
                {
                    #region Turno do mob
                    // Chance do inimigo querer fugir
                    if (mobFound.Life < mobFound.MaxLife / 5)
                    {
                        double _escapeChance = 3d;
                        if (Tools.RandomChance(_escapeChance))
                        {
                            // O inimigo está fugindo
                            mobFound.State = MobState.Exploring;
                            s.ShowSubtitle(
                                s.GetSubtitle(mobFound.Name, "runAway"));
                            answer = Tools.Answer(s,
                                $"{s.GetSubtitle("Menu", "mobRunningAway").Replace("#MobName", mobFound.Name)}\n");

                            switch (answer)
                            {
                                case 0:
                                    // Permite a fuga do mob
                                    s.ShowSubtitle(
                                        s.GetSubtitle("Player", $"allow{mobFound.Name}Run") + "\n");
                                    player.State = MobState.Exploring;
                                    break;

                                // Verifica se você consegue evitar a fuga do inimigo ou não
                                case 1:
                                    if (!mobFound.TryRunAway())
                                        s.ShowSubtitle(
                                            s.GetSubtitle("Player", "catch" + mobFound.Name) + "\n");
                                    else
                                    {
                                        s.ShowSubtitle(
                                            s.GetSubtitle("Player", "noCatch" + mobFound.Name) + "\n");
                                        player.State = MobState.Exploring;
                                    }
                                    break;

                                default:
                                    InvalidOption(s);
                                    continue;
                            }
                        }
                    }

                    if (mobFound.State == MobState.Fighting)
                    {
                        // Ataque do inimigo
                        double _damage = MobAttack(s, mobFound, player); ;
                        s.ShowSubtitle(
                            s.GetSubtitle(mobFound.Name, "attack" + r.Next(3)));
                        s.ShowSubtitle(
                            s.GetSubtitle("Combat", "getDamage").Replace("#Name", player.Name).Replace("#Damage", _damage.ToString("F2")) +
                            "\n");
                    }
                    #endregion
                }
                if (player.Life <= 0) break;
            }
            while (player.State == MobState.Fighting);
        }

        public static double MobAttack(LanguagesManager s, MobCreate mob, MobCreate enemy)
        {
            double _damage = 0d;
            if (!enemy.DodgeCheck())
            {
                _damage = mob.SetDamage();
                double _criticDamage = mob.CriticCheck(_damage);

                if (mob.Weapon != null)
                {
                    mob.Weapon.Erode();
                    if (mob.Weapon.Condition <= 0)
                    {
                        s.ShowSubtitle(
                            s.GetSubtitle("Weapon", "broke").Replace("#Name", mob.Name));
                        mob.WeaponUnequip();
                    }
                }

                if (_criticDamage > _damage)
                {
                    _damage = _criticDamage;
                    s.ShowSubtitle(
                        s.GetSubtitle("Combat", "critic"));
                }
                enemy.GetDamage(_damage);
            }
            else s.ShowSubtitle(s.GetSubtitle("Combat", "dodge").Replace("#Name", enemy.Name)); // Dodge
            return _damage;
        }

        public static void MerchantFound(EntityRegistry register, LanguagesManager s, Player player)
        {
            // Encontra o mercador
            Merchant merchant = new Merchant(register);
            register.AddEntity(merchant as MobCreate);
            int answer;
            s.ShowSubtitle(s.GetSubtitle("Player", "merchantFound") + "\n");
            do
            {
                answer = Tools.Answer(s, s.GetSubtitle("Menu", "merchantFound"));
                switch (answer)
                {
                    case 0:
                        // Ignora o mercador
                        s.ShowSubtitle(s.GetSubtitle("Player", "leaveMerchant") + "\n");
                        break;

                    case 1:
                        s.ShowSubtitle($"[{s.GetSubtitle("Player", "myCoins").Replace("#Coins", player.Coins.ToString("F2", CultureInfo.InvariantCulture))}]");
                        int _action;
                        Inventory _shop = merchant.Shop;
                        do
                        { /// Arrumar a loja e fazer um sistema de compra /// Organizar os codigos no Player e o Inventario
                            _action = Tools.Answer(s,
                                s.GetSubtitle("Menu", "leave") + "\n" + _shop.CheckBag(s),
                                _shop.Bag.Count + 1);
                            if (_action > 0)
                            {
                                s.ShowSubtitle(_shop.Bag[_action++]);
                            }
                        } while (_action > 0);
                        continue;

                    case 2:

                        break;

                    default:
                        InvalidOption(s);
                        continue;
                }
            }
            while (answer > 0);
        }

        public static void TreasuryFound(LanguagesManager s, MobCreate player)
        {
            int answer;
            double coins = Tools.RandomDouble(20d, 5d); // Sorteia um número
            s.ShowSubtitle(s.GetSubtitle("Subtitles", "treasuryFound")); // Encontra um tesouro

            do
            {
                answer = Tools.Answer(s, s.GetSubtitle("Menu", "noYes"));
                switch (answer)
                {
                    case 0:
                        break;

                    case 1:
                        player.GetCoins(coins);
                        break;

                    default:
                        continue;
                }
                break;
            }
            while (answer > 0);
        }

        public static void InvalidOption(LanguagesManager s)
        {
            s.ShowSubtitle(s.GetSubtitle("Error", "invalidOption"));
        }
    }
}
