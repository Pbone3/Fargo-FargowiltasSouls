﻿using FargowiltasSouls.Content.Projectiles.Masomode;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using FargowiltasSouls.Content.Buffs.Masomode;
using FargowiltasSouls.Core.Globals;
using FargowiltasSouls.Core.NPCMatching;

namespace FargowiltasSouls.Content.NPCs.EternityModeNPCs.VanillaEnemies.OOA
{
    public class DD2KoboldFlyer : EModeNPCBehaviour
    {
        public override NPCMatcher CreateMatcher() => new NPCMatcher().MatchTypeRange(
            NPCID.DD2KoboldFlyerT2,
            NPCID.DD2KoboldFlyerT3
        );

        public int Counter;

        public override void AI(NPC npc)
        {
            base.AI(npc);

            if (++Counter > 60)
            {
                Counter = 0;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                    Projectile.NewProjectile(npc.GetSource_FromThis(), npc.Center, new Vector2(Main.rand.NextFloat(-2f, 2f), -5f),
                        ModContent.ProjectileType<GoblinSpikyBall>(), FargoSoulsUtil.ScaledProjectileDamage(npc.damage, 0.8f), 0f, Main.myPlayer);
            }
        }

        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {
            base.OnHitPlayer(npc, target, hurtInfo);

            target.AddBuff(ModContent.BuffType<FusedBuff>(), 1800);
        }
    }
}
