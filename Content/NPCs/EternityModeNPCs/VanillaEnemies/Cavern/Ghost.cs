﻿using Terraria;
using Terraria.ID;
using FargowiltasSouls.Core.Globals;
using FargowiltasSouls.Core.NPCMatching;

namespace FargowiltasSouls.Content.NPCs.EternityModeNPCs.VanillaEnemies.Cavern
{
    public class Ghost : EModeNPCBehaviour
    {
        public override NPCMatcher CreateMatcher() => new NPCMatcher().MatchType(NPCID.Ghost);

        public override void OnFirstTick(NPC npc)
        {
            base.OnFirstTick(npc);

            if (Main.rand.NextBool(5))
                EModeGlobalNPC.Horde(npc, 3);
        }

        public override void AI(NPC npc)
        {
            base.AI(npc);

            EModeGlobalNPC.Aura(npc, 100, BuffID.Cursed, false, 20);
        }
    }
}
