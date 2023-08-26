using System.Collections.Generic;

namespace FargowiltasSouls.Core.TogglerV2; 

public class ToggleManager {
    public static readonly ToggleManager Instance = new();
    
    protected readonly List<ToggleDefinition> ToggleDefinitions = new();

    public ToggleDefinition NewToggle(string name, ToggleCategory category) {
        ToggleDefinition toggleDef = new(name, ToggleDefinitions.Count, category);
        ToggleDefinitions.Add(toggleDef);
        return toggleDef;
    }

    public ToggleRepository CreateRepository() {
        return new(ToggleDefinitions);
    }
}