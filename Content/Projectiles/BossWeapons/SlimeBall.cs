using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Content.Projectiles.BossWeapons
{
    public class SlimeBall : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Slime Ball");
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 14;
            Projectile.aiStyle = 14;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 180;

            if (ModLoader.TryGetMod("Fargowiltas", out Mod fargo))
                fargo.Call("LowRenderProj", Projectile);
        }

        bool oil;

        public override void OnSpawn(IEntitySource source)
        {
            if (source.Context == "SlimyShield")
                oil = true;
        }

        public override void AI()
        {
            int dust = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.BlueTorch, Projectile.velocity.X * 0.2f,
                Projectile.velocity.Y * 0.2f, 100, default, 2f);
            Main.dust[dust].noGravity = true;
        }

        public override void Kill(int timeleft)
        {
            for (int i = 0; i < 20; i++)
            {
                int num469 = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.BlueTorch, -Projectile.velocity.X * 0.2f,
                    -Projectile.velocity.Y * 0.2f, 100, default, 2f);
                Main.dust[num469].noGravity = true;
                Main.dust[num469].velocity *= 2f;
                num469 = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.BlueTorch, -Projectile.velocity.X * 0.2f,
                    -Projectile.velocity.Y * 0.2f, 100);
                Main.dust[num469].velocity *= 2f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Slimed, 240);
            if (oil)
                target.AddBuff(BuffID.Oiled, 240);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.Slimed, 240);
        }
    }
}