using Terraria.ModLoader;

namespace FargowiltasSouls.Core.TogglerV2; 

public class TogglePlayer : ModPlayer {
    public ToggleRepository Toggles;

    public override void Initialize() {
        Toggles = ToggleHelper.InitializePlayerToggleRepo();
    }

    public override void ResetEffects() {
        Toggles?.SetAllEquipped(false);
    }
}