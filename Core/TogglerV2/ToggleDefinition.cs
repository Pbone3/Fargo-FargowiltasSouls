using FargowiltasSouls.Content.Buffs.Boss;
using Terraria;

namespace FargowiltasSouls.Core.TogglerV2;

public class ToggleDefinition {
    public readonly string Name;
    public readonly int Id;
    protected readonly ToggleCategory Category;

    public ToggleDefinition(string name, int id, ToggleCategory category) {
        Name = name;
        Id = id;
        Category = category;
    }

    public string CategoryName() {
        return Category.Name;
    }

    public bool IsInCategory(ToggleCategory category) {
        return Category == category;
    }
    
    public bool CanTakeEffect(Player player, bool ignoreMutantPresence = false) {
        return player.GetModPlayer<TogglePlayer>().Toggles.GetToggle(this).CanTakeEffect
               && (ignoreMutantPresence || !player.HasBuff<MutantPresenceBuff>());
    }

    public void Equip(Player player) {
        ToggleHelper.EquipToggle(player, this);
    }

    public Toggle CreateToggle() {
        return new(this);
    }

    public override string ToString() {
        return $"{Name}(Id: {Id}, Category: {Category})";
    }
}