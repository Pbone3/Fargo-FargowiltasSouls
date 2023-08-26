using Terraria;

namespace FargowiltasSouls.Core.TogglerV2; 

public static class ToggleHelper {
    public static ToggleRepository InitializePlayerToggleRepo() {
        return ToggleManager.Instance.CreateRepository();
    }

    public static void EquipToggle(Player player, ToggleDefinition toggle) {
        TogglePlayer mPlayer = player.GetModPlayer<TogglePlayer>();
        mPlayer.Toggles.SetEquipped(toggle, true);
    }

    public static ToggleDefinition NewToggle(string name, ToggleCategory category) {
        return ToggleManager.Instance.NewToggle(name, category);
    }
}