﻿using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Content.Projectiles.Masomode
{
    public class BloodScytheFriendly : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Blood Scythe");
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.DemonScythe);
            AIType = ProjectileID.DemonScythe;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 2;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;
            Projectile.GetGlobalProjectile<FargoSoulsGlobalProjectile>().CanSplit = false;
            Projectile.GetGlobalProjectile<FargoSoulsGlobalProjectile>().noInteractionWithNPCImmunityFrames = true;

            if (ModLoader.TryGetMod("Fargowiltas", out Mod fargo))
                fargo.Call("LowRenderProj", Projectile);
        }
    }
}
