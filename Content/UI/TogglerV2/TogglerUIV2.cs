using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.UI;

namespace FargowiltasSouls.Content.UI.TogglerV2; 

public partial class TogglerUIV2 : UIState {
    public bool DebugDisplayAreas;

    public bool ClickedSearchBar;
    public bool ClickedAnything;

    public override void OnInitialize() {
        base.OnInitialize();
        OnLeftClick += delegate { ClickedAnything = true; };
        
        BuildPage();
    }

    public void ClickSearchArea(UIMouseEvent evt, UIElement listeningElement) {
        SearchBar.ToggleTakingText();

        ClickedSearchBar = true;
        ClickedAnything = true;
    }

    public override void Update(GameTime gameTime) {
        base.Update(gameTime);
        
        if (
            // I'm not sure _why_ but I need to check this manually
            BackgroundPanel.ContainsPoint(Main.MouseScreen) ||
            /* If the search bar is writing text then clicking anywhere should cancel
                that instead of using an item */
            SearchBar.IsWritingText
        ) {
            Main.LocalPlayer.mouseInterface = true;
        }

        // Unfocus the search bar if something is clicked that isn't the search bar
        if (SearchBar.IsWritingText && ClickedAnything && !ClickedSearchBar) {
            SearchBar.ToggleTakingText();
        }

        ClickedAnything = false;
        ClickedSearchBar = false;
    }

    public override void Draw(SpriteBatch spriteBatch) {
        base.Draw(spriteBatch);

        if (DebugDisplayAreas) {
            DrawArea(spriteBatch, FilterArea, Color.Red);
            DrawArea(spriteBatch, DebugArea, Color.Green);
            DrawArea(spriteBatch, SearchArea, Color.Blue);
            DrawArea(spriteBatch, ToggleListArea, Color.Yellow);
            DrawArea(spriteBatch, LoadoutArea, Color.Purple);
        }
    }

    public static void DrawArea(SpriteBatch spriteBatch, UIElement element, Color color) {
        float alpha = element.ContainsPoint(Main.MouseScreen) ? 0.65f : 0.45f;
        spriteBatch.Draw(TextureAssets.MagicPixel.Value, element.GetDimensions().ToRectangle(), color * alpha);
    }
}