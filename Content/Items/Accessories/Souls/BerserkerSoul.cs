using FargowiltasSouls.Core.TogglerV2;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Content.Items.Accessories.Souls
{
    //[AutoloadEquip(EquipType.Waist)]
    public class BerserkerSoul : BaseSoul {
        public static ToggleDefinition TMeleeSpeed;
        public static ToggleDefinition TMagmaStone;
        public static ToggleDefinition TYoyoBag;
        public static ToggleDefinition TMoonCharm;
        public static ToggleDefinition TNeptuneShell;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            TMeleeSpeed = ToggleHelper.NewToggle("BerserkerMeleeSpeed", ToggleCategory.UniverseSoul);
            TMagmaStone = ToggleHelper.NewToggle("BerserkerMagmaStone", ToggleCategory.UniverseSoul);
            TYoyoBag = ToggleHelper.NewToggle("BerserkerYoyoBag", ToggleCategory.UniverseSoul);
            TMoonCharm = ToggleHelper.NewToggle("BerserkerMoonCharm", ToggleCategory.UniverseSoul);
            TNeptuneShell = ToggleHelper.NewToggle("BerserkerNeptuneShell", ToggleCategory.UniverseSoul);

            // DisplayName.SetDefault("Berserker's Soul");

            //DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "狂战士之魂");

            string tooltip =
@"30% increased melee damage
20% increased melee speed
15% increased melee crit chance
Increased melee knockback
Effects of the Fire Gauntlet, Yoyo Bag, and Celestial Shell
'None shall live to tell the tale'";
            // Tooltip.SetDefault(tooltip);
            //string tooltip_ch =
            //@"增加30%近战伤害
            //增加20%近战攻速
            //增加15%近战暴击率
            //增加近战击退
            //拥有烈火手套、悠悠球袋和天界壳效果
            //'吾之传说生者弗能传颂'";
            //Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, tooltip_ch);


        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.defense = 4;
        }

        protected override Color? nameColor => new Color(255, 111, 6);

        public static void EquipToggles(Player player) {
            TMeleeSpeed.Equip(player);
            TMagmaStone.Equip(player);
            TYoyoBag.Equip(player);
            TMoonCharm.Equip(player);
            TNeptuneShell.Equip(player);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            EquipToggles(player);

            player.GetDamage(DamageClass.Melee) += 0.3f;
            player.GetCritChance(DamageClass.Melee) += 15;

            if (TMeleeSpeed.CanTakeEffect(player))
                player.GetAttackSpeed(DamageClass.Melee) += .2f;

            //gauntlet
            if (TMagmaStone.CanTakeEffect(player))
                player.magmaStone = true;

            player.kbGlove = true;
            player.autoReuseGlove = true;
            player.meleeScaleGlove = true;

            if (TYoyoBag.CanTakeEffect(player, true))
            {
                player.counterWeight = 556 + Main.rand.Next(6);
                player.yoyoGlove = true;
                player.yoyoString = true;
            }

            //celestial shell
            if (TMoonCharm.CanTakeEffect(player))
            {
                player.wolfAcc = true;
            }

            if (TNeptuneShell.CanTakeEffect(player))
            {
                player.accMerman = true;
            }

            if (hideVisual)
            {
                player.hideMerman = true;
                player.hideWolf = true;
            }

            player.lifeRegen += 2;
        }

        public override void AddRecipes()
        {
            CreateRecipe()

            .AddIngredient(null, "BarbariansEssence")
            .AddIngredient(ItemID.StingerNecklace)
            .AddIngredient(ItemID.YoyoBag)
            .AddIngredient(ItemID.FireGauntlet)
            .AddIngredient(ItemID.BerserkerGlove)
            .AddIngredient(ItemID.CelestialShell)

            .AddIngredient(ItemID.KOCannon)
            .AddIngredient(ItemID.IceSickle)
            .AddIngredient(ItemID.DripplerFlail)
            .AddIngredient(ItemID.ScourgeoftheCorruptor)
            .AddIngredient(ItemID.Kraken)
            .AddIngredient(ItemID.Flairon)
            .AddIngredient(ItemID.MonkStaffT3)
            .AddIngredient(ItemID.NorthPole)
            .AddIngredient(ItemID.Zenith)

            .AddTile(ModContent.Find<ModTile>("Fargowiltas", "CrucibleCosmosSheet"))
            .Register();
        }
    }
}
