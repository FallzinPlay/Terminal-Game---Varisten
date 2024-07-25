using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    internal class Adventure
    {
        public LanguagesManager Language { get; private set; }
        public MobCreate Player { get; private set; }
        public MobCreate[] Mobs { get; private set; }
        public WeaponCreate[] Weapons { get; private set; }

        private int answer;

        public Adventure(LanguagesManager language, MobCreate player, MobCreate[] mobs, WeaponCreate[] weapons)
        {
            this.Language = language;
            this.Player = player;
            this.Mobs = mobs;
            this.Weapons = weapons;
        }

        public void WeaponFound()
        {
            LanguagesManager s = this.Language;

            // Generate a weapon randomly
            WeaponCreate weaponFound = Tools.RandomWeapon(this.Weapons, true);
            s.ShowSubtitle($"{s.GetSubtitle("Subtitles", "weaponFound").Replace("#", weaponFound.Name)}"); // Encontra uma arma

            do
            {
                // Menu
                answer = Tools.Answer(s, s.GetSubtitle("Menu", "weaponFound"), 3);
                switch (answer)
                {
                    case 0:
                        s.ShowSubtitle(s.GetSubtitle("Subtitles", "dontEquipWeapon")); // Abandona a arma
                        break;

                    case 1:
                        // Mostra a arma do jogador (se tiver) e a arma encontrada
                        if (this.Player.WeaponEquiped != false)
                        {
                            // Compara a arma que já tem com a que encontrou
                            s.ShowSubtitle(
                                $"\n[{s.GetSubtitle("Titles", "myWeapon")}]\n" +
                                this.Player.Weapon +
                                $"\n\n[{s.GetSubtitle("Titles", "weaponFound")}]\n" +
                                weaponFound + "\n");
                        }
                        else
                        {
                            // Diz que não tem arma e avalia a encontrada
                            s.ShowSubtitle(
                                s.GetSubtitle("Subtitles", "haveNoWeapon") +
                                weaponFound + "\n");
                        }
                        break;

                    case 2:
                        // Equipa a arma
                        this.Player.WeaponEquip(weaponFound);
                        break;

                    default:
                        // Mensagem de erro
                        Tools.InvalidAction(s);
                        continue;
                }

                if (answer != 1) break; // Permite comparar sem sair do loop
            }
            while (answer > 0);
        }

        public void MobFound()
        {
            LanguagesManager s = this.Language;

            MobCreate mobFound = Tools.RandomMob(this.Mobs, this.Weapons, true, this.Player.Lvl);
            s.ShowSubtitle(s.GetSubtitle("Subtitles", "mobFound").Replace("#", mobFound.Name)); // Encontra um mob
            this.Player.State = MobState.Fighting;
            do
            {
                if (this.Player.Life <= 0) break;

                // Menu
                answer = Tools.Answer(s, s.GetSubtitle("Menu", "mobFound"), 4);

                // Escolha
                switch (answer)
                {
                    case 0:
                        #region Chance de escapar
                        // Sistema de chances de conseguir escapar
                        if (Tools.RandomChance(this.Player.EscapeChance))
                        {
                            s.ShowSubtitle(s.GetSubtitle("Subtitles", "canRunAway").Replace("#", mobFound.Name)); // Foge do mob
                            this.Player.State = MobState.Exploring;
                            break;
                        }
                        else
                        {
                            s.ShowSubtitle(s.GetSubtitle("Subtitles", "cantRunAway")); // Não consegue fugir do mob
                        }
                        #endregion
                        break;

                    case 1:
                        s.ShowSubtitle("\n" + mobFound + "\n");
                        continue;

                    case 2:
                        double _damage = this.Player.SetDamage(mobFound);
                        mobFound.State = MobState.Fighting;
                        if (this.Player.WeaponEquiped == true && this.Player.Weapon.Condition <= 0) // Se a arma quebrar, droppar ela
                        {
                            // Desequipa a arma
                            this.Player.WeaponUnequip();
                            s.ShowSubtitle(s.GetSubtitle("Subtitles", "weaponBroke")); // Mostra que a arma quebrou
                        }

                        // Mostra quanto dano ô jogador causou ao inimigo
                        s.ShowSubtitle(s.GetSubtitle("Combat", "damageToMob").Replace("#1", _damage.ToString("F2", CultureInfo.InvariantCulture)).Replace("#2", mobFound.Name));
                        break;

                    case 3:
                        s.ShowSubtitle(this.Player.ToString());
                        continue;

                    default:
                        // Erro de ação invalida
                        Tools.InvalidAction(s);
                        continue;
                }

                // Morte do inimigo
                if (mobFound.Life <= 0)
                {
                    s.ShowSubtitle(s.GetSubtitle("Combat", "mobDefeat").Replace("#", mobFound.Name));

                    // Coleta o Xp
                    this.Player.GetXp(mobFound.Xp);
                    // Coleta as moedas
                    this.Player.GetCoins(mobFound.Coins);

                    #region Chance de drop
                    // Chance do mob droppar a arma que ele está usando
                    if (mobFound.WeaponEquiped == true)
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
                                    this.Player.WeaponEquip(mobFound.Weapon);
                                    break;

                                default:
                                    // Mostra o erro de ação invalida
                                    Tools.InvalidAction(s);
                                    continue;
                            }
                        }
                    }
                    this.Player.State = MobState.Exploring;
                    #endregion
                }

                #region Turno do mob
                // Chance do inimigo querer fugir
                if (mobFound.Life < mobFound.MaxLife / 5)
                {
                    if (Tools.RandomChance(mobFound.EscapeChance))
                    {
                        // O inimigo está fugindo
                        mobFound.State = MobState.Exploring;
                        answer = Tools.Answer(s,
                            $"{s.GetSubtitle("Subtitles", "mobRunningAway").Replace("#", mobFound.Name)}" +
                            $"{s.GetSubtitle("Menu", "allowRunToward")}");

                        switch (answer)
                        {
                            case 0:
                                // Permite a fuga do mob
                                s.ShowSubtitle(s.GetSubtitle("Subtitles", "allowMobRun"));
                                break;

                            // Verifica se você consegue evitar a fuga do inimigo ou não
                            case 1:
                                if (Tools.RandomChance(mobFound.EscapeChance))
                                {
                                    // O mob escapa
                                    s.ShowSubtitle(s.GetSubtitle("Subtitles", "cantCatchMob").Replace("#", mobFound.Name));
                                }
                                else
                                {
                                    // Consegue capturar o mob
                                    s.ShowSubtitle(s.GetSubtitle("Subtitles", "canCatchMob"));
                                }
                                break;

                            default:
                                Tools.InvalidAction(s);
                                continue;
                        }
                    }
                }

                if (mobFound.State == MobState.Fighting)
                {
                    // Ataque do inimigo
                    double mobDamage = mobFound.SetDamage(this.Player);
                    // Mostra o dano do inimigo
                    s.ShowSubtitle(s.GetSubtitle("Combat", "damageToPlayer").Replace("#1", mobFound.Name).Replace("#2", mobDamage.ToString("F2", CultureInfo.InvariantCulture)));
                }
                #endregion
            }
            while (this.Player.State == MobState.Fighting);
        }

        public void MerchantFound()
        {
            LanguagesManager s = this.Language;

            WeaponCreate weaponFound = Tools.RandomWeapon(this.Weapons);
            double weaponPrice = Tools.RandomDouble(weaponFound.MaxPrice, weaponFound.MinPrice);

            // Encontra o mercador
            s.ShowSubtitle(s.GetSubtitle("Subtitles", "merchantFound"));
            while (true)
            {
                answer = Tools.Answer(s, s.GetSubtitle("Menu", "merchantFound"));
                if (answer == 0)
                {
                    // Ignora o mercador
                    s.ShowSubtitle(s.GetSubtitle("Subtitles", "merchantIgnore"));
                    break;
                }

                // Escolha
                switch (answer)
                {
                    case 1:
                        // Legenda
                        s.ShowSubtitle(
                            $"[{s.GetSubtitle("Titles", "items")}] \n" +
                            $"[{s.GetSubtitle("Subtitles", "myCoins").Replace("#", this.Player.Coins.ToString("F2", CultureInfo.InvariantCulture))}]\n");

                        answer = Tools.Answer(s,
                            s.GetSubtitle("Merchant", "shop").Replace("#1", weaponFound.Name).Replace("#2", weaponPrice.ToString("F2", CultureInfo.InvariantCulture)),
                            3);
                        if (answer == 0) break;
                        switch (answer)
                        {
                            case 1:

                                if (this.Player.Buy(5))
                                {
                                    this.Player.Cure(5);
                                    // Compra e toma a poção
                                    s.ShowSubtitle($"[{s.GetSubtitle("Titles", "life")}] {this.Player.Life.ToString("F2", CultureInfo.InvariantCulture)}\n");
                                }
                                else
                                {
                                    // Moedas insuficientes
                                    s.ShowSubtitle(s.GetSubtitle("Subtitles", "insufficientMoney"));
                                }
                                continue;

                            case 2:

                                if (this.Player.Buy(weaponPrice) && this.Player.WeaponEquip(weaponFound))
                                {
                                    // Compra e equipa a arma
                                    s.ShowSubtitle(s.GetSubtitle("Subtitles", "buyWeapon"));
                                }
                                else
                                {
                                    // Moedas insuficientes
                                    s.ShowSubtitle(s.GetSubtitle("Subtitles", "insufficientMoney"));
                                }
                                continue;

                            default:
                                // Mensagem de erro
                                s.ShowSubtitle(s.GetSubtitle("Error", "invalidAction"));
                                continue;
                        }

                    default:
                        // Mensagem de erro
                        Tools.InvalidAction(s);
                        continue;
                }

                if (answer != 1) break;
            }
        }

        public void TreasuryFound()
        {
            LanguagesManager s = this.Language;

            // Gera uma quantidade aleatória de moedas
            double coins = Tools.RandomDouble(20d, 5d); // Sorteia um número
            s.ShowSubtitle(s.GetSubtitle("Subtitles", "treasuryFound")); // Encontra um tesouro

            while (true)
            {
                answer = Tools.Answer(s, s.GetSubtitle("Menu", "noYes"));
                switch (answer)
                {
                    case 0:
                        break;

                    case 1:
                        this.Player.GetCoins(coins);
                        // Coleta as moedas
                        s.ShowSubtitle(s.GetSubtitle("Subtitles", "coinsCollect").Replace("#", coins.ToString("F2", CultureInfo.InvariantCulture)));
                        break;

                    default:
                        Tools.InvalidAction(s);
                        continue;
                }
                break;
            }
        }

    }
}
