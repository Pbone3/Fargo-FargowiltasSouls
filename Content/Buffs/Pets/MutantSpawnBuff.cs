﻿using FargowiltasSouls.Content.Projectiles.Pets;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace FargowiltasSouls.Content.Buffs.Pets
{
    public class MutantSpawnBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Mutant Spawn");
            // Description.SetDefault("Mutant Spawn");
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            player.GetModPlayer<FargoSoulsPlayer>().MutantSpawn = true;
            if (player.ownedProjectileCounts[ModContent.ProjectileType<MutantSpawn>()] <= 0 && player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(player.GetSource_Buff(buffIndex), player.Center, Vector2.Zero, ModContent.ProjectileType<MutantSpawn>(), 0, 0f, player.whoAmI);
            }
        }
    }
}