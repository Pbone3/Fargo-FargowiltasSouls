using System.Collections.Generic;
using FargowiltasSouls.Content.UI.TogglerV2;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace FargowiltasSouls.Content.UI; 

public class FargoUIManagerV2 : ModSystem {
    public UserInterface? TogglerInterface;
    public TogglerUIV2? TogglerUIState;

    public static Asset<Texture2D> SearchButton;
    public static Asset<Texture2D> SearchButton_Border;
    public static Asset<Texture2D> SearchCancelButton;
    public static Asset<Texture2D> FavoriteButton_Active;
    public static Asset<Texture2D> FavoriteButton_Inactive;
    public static Asset<Texture2D> CloudButton_Active;
    public static Asset<Texture2D> CloudButton_Inactive;
    public static Asset<Texture2D> FilterButton;
    public static Asset<Texture2D> FilterButton_Border;

    public static Asset<Texture2D> Toggler_CategoryIconBackground;

    public override void Load() {
        if (Main.dedServ) {
            return;
        }

        Main.RunOnMainThread(delegate {
            SearchButton = VanillaUITexture("Bestiary/Button_Search");
            SearchButton_Border = VanillaUITexture("Bestiary/Button_Search_Border");
            SearchCancelButton = VanillaUITexture("SearchCancel");
            FavoriteButton_Active = VanillaUITexture("ButtonFavoriteActive");
            FavoriteButton_Inactive = VanillaUITexture("ButtonFavoriteInactive");
            CloudButton_Active = VanillaUITexture("ButtonCloudActive");
            CloudButton_Inactive = VanillaUITexture("ButtonCloudInactive");
            FilterButton = VanillaUITexture("Bestiary/Button_Filtering");
            FilterButton_Border = VanillaUITexture("Bestiary/Button_Wide_Border");

            Toggler_CategoryIconBackground = ModdedUITexture("TogglerV2/CategoryIconBackground");
        }).Wait();

        TogglerInterface = new();
        TogglerUIState = new();
        TogglerUIState.Activate();

        TogglerInterface.IsVisible = true;
        TogglerInterface.SetState(TogglerUIState);
    }

    private Asset<Texture2D> VanillaUITexture(string path) {
        return Main.Assets.Request<Texture2D>(string.Concat("Images/UI/", path), AssetRequestMode.ImmediateLoad);
    }

    private Asset<Texture2D> ModdedUITexture(string path) {
        return ModContent.Request<Texture2D>(string.Concat("FargowiltasSouls/Assets/UI/", path), AssetRequestMode.ImmediateLoad);
    }

    public override void UpdateUI(GameTime gameTime) {
        if (TogglerInterface?.CurrentState != null) {
            TogglerInterface.Update(gameTime);
        }
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
        int i = layers.FindIndex(layer => layer.Name == "Vanilla: Inventory");
        if (i != -1) {
            layers.Insert(i + 1, new LegacyGameInterfaceLayer(
                "FargowiltasSouls: TogglerV2",
                delegate {
                    TogglerInterface?.CurrentState?.Draw(Main.spriteBatch);
                    return true;
                },
                InterfaceScaleType.UI)
            );
        }
    }
}