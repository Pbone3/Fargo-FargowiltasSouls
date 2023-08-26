namespace FargowiltasSouls.Core.TogglerV2;

public class Toggle {
    public ToggleDefinition Definition { get; }
    public bool Equipped;
    public bool Disabled;

    public Toggle(ToggleDefinition definition) {
        Definition = definition;
    }

    public bool CanTakeEffect => Equipped && !Disabled;

    public bool IsInCategory(ToggleCategory category) {
        return Definition.IsInCategory(category);
    }

    public override string ToString() {
        return $"{Definition.Name}(Equipped: {Equipped}, Disabled: {Disabled})";
    }
}