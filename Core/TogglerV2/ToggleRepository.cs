using System.Collections.Generic;

namespace FargowiltasSouls.Core.TogglerV2; 

public class ToggleRepository {
    public readonly Toggle[] Toggles;

    public ToggleRepository(List<ToggleDefinition> toggleDefinitions) {
        Toggles = new Toggle[toggleDefinitions.Count];
        for (int i = 0; i < toggleDefinitions.Count; i++) {
            Toggles[i] = toggleDefinitions[i].CreateToggle();
        }
    }

    public Toggle GetToggle(ToggleDefinition toggle) {
        return Toggles[toggle.Id];
    }

    public void SetDisabled(ToggleDefinition toggle, bool value) {
        Toggles[toggle.Id].Disabled = value;
    }

    public void SetAllDisabled(bool value) {
        for (int i = 0; i < Toggles.Length; i++) {
            Toggles[i].Disabled = value;
        }
    }

    public void SetAllDisabled(bool value, ToggleCategory category) {
        for (int i = 0; i < Toggles.Length; i++) {
            if (Toggles[i].IsInCategory(category)) {
                Toggles[i].Disabled = value;
            }
        }
    }

    public void SetEquipped(ToggleDefinition toggle, bool value) {
        Toggles[toggle.Id].Equipped = value;
    }

    public void SetAllEquipped(bool value) {
        for (int i = 0; i < Toggles.Length; i++) {
            Toggles[i].Equipped = value;
        }
    }
    
    public void SetAllEquipped(bool value, ToggleCategory category) {
        for (int i = 0; i < Toggles.Length; i++) {
            if (Toggles[i].IsInCategory(category)) {
                Toggles[i].Equipped = value;
            }
        }
    }
}