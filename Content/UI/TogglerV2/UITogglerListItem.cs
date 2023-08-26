using FargowiltasSouls.Core.TogglerV2;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace FargowiltasSouls.Content.UI.TogglerV2; 

public class UITogglerListItem : UIElement {
    protected const int PaddingAroundCategoryIconBackground = 2;

    protected const int PaddingBetweenSeparatorAndEdge = 4;

    protected static Asset<Texture2D> CategoryIconBackgroundTexture => FargoUIManagerV2.Toggler_CategoryIconBackground;
    
    protected UIPanel BackgroundPanel;
    protected UIPanel BorderOverEverything;

    protected UIElement CategoryArea;
    protected UIImage CategoryIconBackground;
    protected UIImage CategoryIcon;
    protected UIText CategoryTile;

    protected UIHorizontalSeparator TitleContentSeparator;

    protected UIElement ToggleDisablersArea;

    public UITogglerListItem(ToggleCategory category) {
        Color backgroundColor = new(73, 94, 171);
        Color borderColor = new(89, 116, 213);

        SetPadding(0);

        BackgroundPanel = new() {
            Width = StyleDimension.Fill,
            Height = StyleDimension.Fill,
            BackgroundColor = backgroundColor,
            BorderColor = borderColor,
        };
        BackgroundPanel.SetPadding(6);
        Append(BackgroundPanel);

        int categoryAreaHeight = CategoryIconBackgroundTexture.Height() + PaddingAroundCategoryIconBackground * 2;
        CategoryArea = new() {
            Width = StyleDimension.Fill,
            Height = StyleDimension.FromPixels(categoryAreaHeight)
        };
        CategoryArea.SetPadding(0);
        BackgroundPanel.Append(CategoryArea);

        CategoryIconBackground = new(CategoryIconBackgroundTexture) {
            Top = StyleDimension.FromPixels(PaddingAroundCategoryIconBackground),
            Left = StyleDimension.FromPixels(PaddingAroundCategoryIconBackground),
        };
        CategoryArea.Append(CategoryIconBackground);

        int paddingBetweenAreas = (int)BackgroundPanel.PaddingTop;
        int top = categoryAreaHeight + paddingBetweenAreas;
        TitleContentSeparator = new() {
            Width = StyleDimension.Fill,
            Top = StyleDimension.FromPixels(top + BackgroundPanel.PaddingTop),
            Color = borderColor * 0.9f,
        };
        Append(TitleContentSeparator);
        top += (int)TitleContentSeparator.Height.Pixels + paddingBetweenAreas;

        ToggleDisablersArea = new() {
            Width = StyleDimension.Fill,
            Height = StyleDimension.FromPixelsAndPercent(-top, 1f),
            Top = StyleDimension.FromPixels(top),
        };
        BackgroundPanel.Append(ToggleDisablersArea);

        BorderOverEverything = new() {
            Width = StyleDimension.Fill,
            Height = StyleDimension.Fill,
            BackgroundColor = Color.Transparent,
            BorderColor = borderColor,
        };
        Append(BorderOverEverything);
    }

    protected override void DrawChildren(SpriteBatch spriteBatch) {
        base.DrawChildren(spriteBatch);
        //TogglerUIV2.DrawArea(spriteBatch, CategoryArea, Color.Orange);
        //TogglerUIV2.DrawArea(spriteBatch, ToggleDisablersArea, Color.Aqua);
    }
}