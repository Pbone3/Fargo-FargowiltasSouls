﻿using FargowiltasSouls.Content.Projectiles;

using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace FargowiltasSouls.Content.Items.Accessories.Enchantments
{
    public class BorealWoodEnchant : BaseEnchant
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        protected override Color nameColor => new(139, 116, 100);

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.rare = ItemRarityID.Green;
            Item.value = 10000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            BorealEffect(player, Item);
        }

        public static void BorealEffect(Player player, Item item)
        {
            FargoSoulsPlayer modPlayer = player.GetModPlayer<FargoSoulsPlayer>();
            modPlayer.BorealEnchantItem = item;

            if (modPlayer.BorealCD > 0)
                modPlayer.BorealCD--;
        }

        public static void BorealSnowballs(FargoSoulsPlayer modPlayer, int damage)
        {
            Player player = modPlayer.Player;

            Vector2 vel = Vector2.Normalize(Main.MouseWorld - player.Center) * 17f;
            int snowballDamage = damage / 2;
            if (!modPlayer.TerrariaSoul)
                snowballDamage = Math.Min(snowballDamage, FargoSoulsUtil.HighestDamageTypeScaling(player, modPlayer.WoodForce ? 300 : 30));
            int p = Projectile.NewProjectile(player.GetSource_Accessory(modPlayer.BorealEnchantItem), player.Center, vel, ProjectileID.SnowBallFriendly, snowballDamage, 1, Main.myPlayer);

            int numSnowballs = modPlayer.WoodForce ? 7 : 3;
            if (p != Main.maxProjectiles)
                FargoSoulsGlobalProjectile.SplitProj(Main.projectile[p], numSnowballs, MathHelper.Pi / 10, 1);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.BorealWoodHelmet)
            .AddIngredient(ItemID.BorealWoodBreastplate)
            .AddIngredient(ItemID.BorealWoodGreaves)
            .AddIngredient(ItemID.Shiverthorn)
            .AddIngredient(ItemID.Plum)
            .AddIngredient(ItemID.Snowball, 300)

            .AddTile(TileID.DemonAltar)
            .Register();
        }
    }
}
