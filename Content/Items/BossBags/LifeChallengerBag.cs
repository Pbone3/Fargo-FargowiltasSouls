﻿using FargowiltasSouls.Content.Bosses.DeviBoss;
using FargowiltasSouls.Content.Bosses.Lieflight;
using FargowiltasSouls.Content.Items.Materials;
using FargowiltasSouls.Content.Items.Placables;
using FargowiltasSouls.Content.Items.Weapons.Challengers;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Content.Items.BossBags
{
    public class LifeChallengerBag : BossBag
    {
        protected override bool IsPreHMBag => false;

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<LifeRevitalizer>()));
            itemLoot.Add(ItemDropRule.CoinsBasedOnNPCValue(ModContent.NPCType<LifeChallenger>()));
            itemLoot.Add(new OneFromOptionsDropRule(1, 1, new int[] 
            {
                ModContent.ItemType<EnchantedLifeblade>(),
                ModContent.ItemType<Lightslinger>(),
                ModContent.ItemType<CrystallineCongregation>(),
                ModContent.ItemType<KamikazePixieStaff>()
            }));
            itemLoot.Add(ItemDropRule.Common(ItemID.HallowedFishingCrate, 1, 5, 5));
            itemLoot.Add(ItemDropRule.Common(ItemID.SoulofLight, 1, 3, 3));
            itemLoot.Add(ItemDropRule.Common(ItemID.PixieDust, 1, 25, 25));
        }
    }
}