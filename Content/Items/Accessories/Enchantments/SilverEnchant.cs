using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace FargowiltasSouls.Content.Items.Accessories.Enchantments
{
    [AutoloadEquip(EquipType.Shield)]
    public class SilverEnchant : BaseEnchant
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            // DisplayName.SetDefault("Silver Enchantment");

            string tooltip =
@"Right Click to guard with your shield
Guard just before being hit to parry and negate damage
Parry blocks up to 100 damage
'Reflection'";
            // Tooltip.SetDefault(tooltip);
        }

        protected override Color nameColor => new(180, 180, 204);
        

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.rare = ItemRarityID.Blue;
            Item.value = 30000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            FargoSoulsPlayer modPlayer = player.GetModPlayer<FargoSoulsPlayer>();
            if (player.GetToggleValue("SilverS"))
            {
                //shield
                modPlayer.SilverEnchantItem = Item;
            }

            // modPlayer.AddMinion(Item, player.GetToggleValue("Silver"), ModContent.ProjectileType<SilverSword>(), 20, 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe()

            .AddIngredient(ItemID.SilverHelmet)
            .AddIngredient(ItemID.SilverChainmail)
            .AddIngredient(ItemID.SilverGreaves)
            .AddIngredient(ItemID.EmptyBucket)
            .AddIngredient(ItemID.SilverBroadsword)
            .AddIngredient(ItemID.BlandWhip)

            .AddTile(TileID.DemonAltar)
            .Register();
        }
    }
}
