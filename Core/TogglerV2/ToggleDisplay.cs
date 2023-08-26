using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Localization;

namespace FargowiltasSouls.Core.TogglerV2; 

public record class ToggleDisplay(Asset<Texture2D> Icon, LocalizedText DisplayName);