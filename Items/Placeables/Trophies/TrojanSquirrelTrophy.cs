using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace FargowiltasSouls.Items.Placeables.Trophies
{
    public class TrojanSquirrelTrophy : BaseTrophy
    {
        protected override int TileType => ModContent.TileType<Tiles.Trophies.TrojanSquirrelTrophy>();

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            DisplayName.SetDefault("Trojan Squirrel Trophy");
        }
    }
}