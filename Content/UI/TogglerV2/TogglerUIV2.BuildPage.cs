using System;
using FargowiltasSouls.Core.TogglerV2;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;

namespace FargowiltasSouls.Content.UI.TogglerV2; 

public partial class TogglerUIV2 {
    public const int BACKGROUND_PANEL_WIDTH = 560;
    public const int BACKGROUND_PANEL_HEIGHT = 420;

    public const int HEIGHT_TOP_AREAS = 28;
    public const int PADDING_ADJACENT_AREAS = 4;
    
    public const int WIDTH_SEARCH_AREA = 240;
    public const int HEIGHT_SEARCH_BAR = 28;

    public const int WIDTH_TOGGLE_LIST_SCROLLBAR = 20;

    public const int PADDING_DEBUG_BUTTONS = 2;

    public const int WIDTH_LOADOUT_AREA = 48;

    public static readonly Color BackgroundPanelColor = new Color(33, 43, 79) * 0.8f;
    public static readonly Color SearchBarPanelBackgroundColor = new(35, 40, 83);
    public static readonly Color SearchBarPanelFocusColor = Main.OurFavoriteColor;
    public static readonly Color LoadoutBackgroundPanelBGColor = new(89, 116, 213);
    public static readonly Color LoadoutBackgroundPanelBorderColor = new(73, 94, 171);

    public UIPanel BackgroundPanel;

    public UIElement FilterArea;
    public UIImageButton FilterButton;

    public UIElement SearchArea;
    public UIImageButton SearchButton;
    public UIPanel SearchBarPanel;
    public UISearchBar SearchBar;
    public UIImageButton SearchCancelButton;

    public UIElement DebugArea;
    public UIImageButton ReloadButton;
    public UIImageButton DebugDisplayButton;

    public UIElement ToggleListArea;
    public UIList ToggleList;
    public UIScrollbar ToggleListScrollbar;

    public UIElement LoadoutArea;
    public UIPanel LoadoutBackgroundPanel;

    public void BuildPage() {
        RemoveAllChildren();

        StyleDimension? oldTop = BackgroundPanel?.Top;
        StyleDimension? oldLeft = BackgroundPanel?.Left;

        BackgroundPanel = new() {
            Width = StyleDimension.FromPixels(BACKGROUND_PANEL_WIDTH),
            Height = StyleDimension.FromPixels(BACKGROUND_PANEL_HEIGHT),
        };
        BackgroundPanel.BackgroundColor = BackgroundPanelColor;
        BackgroundPanel.BorderColor *= 0.8f;
        BackgroundPanel.OnLeftClick += delegate { ClickedAnything = true; };
        Append(BackgroundPanel);
        CalculatedStyle dimensions = BackgroundPanel.GetDimensions();
        BackgroundPanel.Left = oldLeft ?? StyleDimension.FromPixelsAndPercent(-dimensions.Width / 2, 0.5f);
        BackgroundPanel.Top = oldTop ?? StyleDimension.FromPixelsAndPercent(-dimensions.Height / 2, 0.5f);
        Recalculate();

        BuildPage_FilterArea();
        BuildPage_DebugArea();
        BuildPage_SearchArea();
        BuildPage_ToggleListArea();
        BuildPage_LoadoutArea();
    }

    public void BuildPage_FilterArea() {
        FilterButton = new UIImageButton(FargoUIManagerV2.FilterButton);
        FilterButton.SetHoverImage(FargoUIManagerV2.FilterButton_Border);
        FilterButton.SetVisibility(1f, 1f);

        FilterArea = new() {
            Height = StyleDimension.FromPixels(HEIGHT_TOP_AREAS),
        };
        FilterArea.SetPadding(0);
        FilterArea.Width = StyleDimension.FromPixels(FilterButton.Width.Pixels);
        
        FilterArea.Append(FilterButton);
        BackgroundPanel.Append(FilterArea);
    }

    public void BuildPage_SearchArea() {
        SearchArea = new() {
            Width = StyleDimension.FromPixels(WIDTH_SEARCH_AREA),
            Height = StyleDimension.FromPixels(HEIGHT_TOP_AREAS),
        };
        SearchArea.SetPadding(0);
        SearchArea.Left = StyleDimension.FromPixelsAndPercent(
            -SearchArea.Width.Pixels,
            1f
        );
        BackgroundPanel.Append(SearchArea);

        SearchButton = new(FargoUIManagerV2.SearchButton) {
            Left = StyleDimension.Empty,
            VAlign = 0.5f,
        };
        SearchButton.SetHoverImage(FargoUIManagerV2.SearchButton_Border);
        SearchButton.SetVisibility(1f, 1f);
        SearchButton.OnLeftClick += ClickSearchArea;
        SearchArea.Append(SearchButton);
    
        int off = (int)SearchButton.Width.Pixels + 4;
        SearchBarPanel = new() {
            Width = StyleDimension.FromPixelsAndPercent(-off, 1f),
            Height = StyleDimension.FromPixels(HEIGHT_SEARCH_BAR),
            Left = StyleDimension.FromPixels(off),
            Top = StyleDimension.Empty,
            VAlign = 0.5f,
            BackgroundColor = SearchBarPanelBackgroundColor,
            BorderColor = SearchBarPanelBackgroundColor,
        };
        SearchBarPanel.OnLeftClick += ClickSearchArea;
        SearchBarPanel.SetPadding(0);
        SearchArea.Append(SearchBarPanel);

        SearchCancelButton = new(FargoUIManagerV2.SearchCancelButton) {
            /* These magic numbers are a workaround to get the button
                positioned properly but I have no better ideas. */
            Left = StyleDimension.FromPixels(-2),
            Top = StyleDimension.FromPixels(-1),
            HAlign = 1f,
            VAlign = 0.5f,
        };
        SearchCancelButton.OnMouseOver += delegate { SoundEngine.PlaySound(SoundID.MenuTick); };
        SearchCancelButton.OnLeftClick += delegate {
            if (SearchBar.HasContents) {
                SearchBar.SetContents(null, true);
                SoundEngine.PlaySound(SoundID.MenuClose);
            }
            else {
                SoundEngine.PlaySound(SoundID.MenuTick);
            }
        };
        SearchBarPanel.Append(SearchCancelButton);
        SearchBar = new(Language.GetText("UI.PlayerNameSlot"), 0.875f) {
            Width = StyleDimension.FromPixelsAndPercent(-SearchCancelButton.Width.Pixels, 1f),
            Height = StyleDimension.Fill,
            VAlign = 0.5f,
            IgnoresMouseInteraction = true,
        };
        SearchBar.SetContents(null, true);
        SearchBar.OnStartTakingInput += delegate { SearchBarPanel.BorderColor = SearchBarPanelFocusColor; };
        SearchBar.OnEndTakingInput += delegate { SearchBarPanel.BorderColor = SearchBarPanelBackgroundColor; };
        SearchBarPanel.Append(SearchBar);
    }

    public void BuildPage_DebugArea() {
        int left = 0;
        ReloadButton = BuildPage_DebugArea_Button(FargoUIManagerV2.CloudButton_Inactive,
            FargoUIManagerV2.CloudButton_Active, ref left, BuildPage
        );
        left += PADDING_DEBUG_BUTTONS;

        DebugDisplayButton = BuildPage_DebugArea_Button(FargoUIManagerV2.FavoriteButton_Inactive,
            FargoUIManagerV2.FavoriteButton_Active, ref left, delegate { DebugDisplayAreas = !DebugDisplayAreas; }
        );

        DebugArea = new() {
            Left = StyleDimension.FromPixels(FilterArea.Width.Pixels + PADDING_ADJACENT_AREAS),
            Height = StyleDimension.FromPixels(HEIGHT_TOP_AREAS),
            Width =
            StyleDimension.FromPixels(
                ReloadButton.Width.Pixels +
                PADDING_DEBUG_BUTTONS +
                DebugDisplayButton.Width.Pixels
            ),
        };
        DebugArea.SetPadding(0);
        
        DebugArea.Append(ReloadButton);
        DebugArea.Append(DebugDisplayButton);
        BackgroundPanel.Append(DebugArea);
    }

    public static UIImageButton BuildPage_DebugArea_Button(Asset<Texture2D> tex, Asset<Texture2D> hover, ref int left, Action onClick) {
        UIImageButton ui = new(tex) {
            Left = StyleDimension.FromPixels(left),
            VAlign = 0.5f,
        };
        ui.SetHoverImage(hover);
        ui.SetVisibility(1f, 1f);
        ui.OnLeftClick += delegate { onClick(); };

        left += (int)ui.Width.Pixels;

        return ui;
    }

    public void BuildPage_ToggleListArea() {
        ToggleListArea = new() {
            Width = StyleDimension.FromPixelsAndPercent(-WIDTH_LOADOUT_AREA - PADDING_ADJACENT_AREAS, 1f),
            Height = StyleDimension.FromPixelsAndPercent(-HEIGHT_TOP_AREAS - PADDING_ADJACENT_AREAS, 1f),
            Top = StyleDimension.FromPixels(HEIGHT_TOP_AREAS + PADDING_ADJACENT_AREAS),
        };
        ToggleListArea.SetPadding(0);
        BackgroundPanel.Append(ToggleListArea);

        ToggleListScrollbar = new UIScrollbar() {
            Width = StyleDimension.FromPixels(WIDTH_TOGGLE_LIST_SCROLLBAR),
            Height = StyleDimension.FromPixelsAndPercent(-12, 1f),
            VAlign = 0.5f,
        };
        ToggleListScrollbar.Left = StyleDimension.FromPixelsAndPercent(-ToggleListScrollbar.Width.Pixels, 1f);
        ToggleListScrollbar.OverflowHidden = true;
        ToggleListScrollbar.SetView(ToggleListArea.Height.Pixels, ToggleListArea.MaxHeight.Pixels);
        ToggleListArea.Append(ToggleListScrollbar);

        UITogglerListItem elem = new(ToggleCategory.UniverseSoul) {
            Width = StyleDimension.FromPixelsAndPercent(-ToggleListScrollbar.Width.Pixels - 2, 1f),
            Height = StyleDimension.FromPixels(180),
        };
        ToggleListArea.Append(elem);
    }

    public void BuildPage_LoadoutArea() {
        LoadoutArea = new() {
            Width = StyleDimension.FromPixels(WIDTH_LOADOUT_AREA),
            Height = StyleDimension.FromPixelsAndPercent(-HEIGHT_TOP_AREAS - PADDING_ADJACENT_AREAS, 1f),
            Left = StyleDimension.FromPixels(
                ToggleListArea.Width.GetValue(BackgroundPanel.GetInnerDimensions().Width) +
                PADDING_ADJACENT_AREAS
            ),
            Top = StyleDimension.FromPixels(HEIGHT_TOP_AREAS + PADDING_ADJACENT_AREAS),
        };
        LoadoutArea.SetPadding(0);
        BackgroundPanel.Append(LoadoutArea);

        LoadoutBackgroundPanel = new() {
            Width = StyleDimension.Fill,
            Height = StyleDimension.Fill,
            BackgroundColor = LoadoutBackgroundPanelBGColor,
            BorderColor = LoadoutBackgroundPanelBorderColor,
        };
        LoadoutArea.Append(LoadoutBackgroundPanel);
    }
}