using FargowiltasSouls.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace FargowiltasSouls.Content.Items.Accessories.Enchantments
{
    public class MeteorEnchant : BaseEnchant
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            // DisplayName.SetDefault("Meteor Enchantment");

            string tooltip =
@"A meteor shower initiates every few seconds while attacking
'Drop a draco on 'em'";
            // Tooltip.SetDefault(tooltip);
        }

        protected override Color nameColor => new(95, 71, 82);
        

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.rare = ItemRarityID.Pink;
            Item.value = 100000;
        }

        public const int METEOR_ADDED_DURATION = 450;

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            FargoSoulsPlayer modPlayer = player.GetModPlayer<FargoSoulsPlayer>();

            modPlayer.MeteorEffect(Item);

        }

        public override void AddRecipes()
        {
            CreateRecipe()

            .AddIngredient(ItemID.MeteorHelmet)
            .AddIngredient(ItemID.MeteorSuit)
            .AddIngredient(ItemID.MeteorLeggings)
            .AddIngredient(ItemID.SpaceGun)
            .AddIngredient(ItemID.StarCannon)
            .AddIngredient(ItemID.PlaceAbovetheClouds)

            .AddTile(TileID.DemonAltar)
            .Register();
        }
    }

    public class MeteorGlobalProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
            => entity.type == ProjectileID.Meteor1 || entity.type == ProjectileID.Meteor2 || entity.type == ProjectileID.Meteor3;

        bool fromEnch;

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is EntitySource_ItemUse itemSource && itemSource.Item.type == ModContent.ItemType<MeteorEnchant>())
            {
                fromEnch = true;
                projectile.GetGlobalProjectile<FargoSoulsGlobalProjectile>().CanSplit = false;

                //if (ModLoader.GetMod("Fargowiltas") != null)
                //    ModLoader.GetMod("Fargowiltas").Call("LowRenderProj", Main.projectile[p]);
            }
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (fromEnch)
            {
                const int maxHits = 75;
                Main.player[projectile.owner].GetModPlayer<FargoSoulsPlayer>().MeteorTimer -= MeteorEnchant.METEOR_ADDED_DURATION / maxHits;
            }
        }
    }
}
