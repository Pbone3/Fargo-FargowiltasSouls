using FargowiltasSouls.Core.TogglerV2;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Default;

namespace FargowiltasSouls.Content.UI.TogglerV2; 

public class TogglerEntry {
    public readonly ToggleDefinition ToggleDefinition;
    
    /// <summary>
    /// Lazily initialized.
    /// </summary>
    protected ToggleDisplay? Display;

    public TogglerEntry(ToggleDefinition toggle) {
        ToggleDefinition = toggle;
    }
    
    public ToggleDisplay GetToggleDisplay() {
        if (Display is null) {
            Display = new(
                RequestIconTexture(),
                Language.GetText($"Mods.FargowiltasSouls.Toggles.{ToggleDefinition.CategoryName()}.{ToggleDefinition.Name}")
            );
        }

        return Display;
    }

    protected Asset<Texture2D> RequestIconTexture() {
        try {
            return ModContent.Request<Texture2D>($"FargowiltasSouls/Assets/Toggles/{ToggleDefinition.Name}");
        }
        catch (AssetLoadException) {
            // An icon for the toggle wasn't found. Fall back to the unloaded item texture
            return TextureAssets.Item[ModContent.ItemType<UnloadedItem>()];
        }
    }
}