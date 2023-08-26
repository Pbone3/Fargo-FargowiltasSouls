using System;

namespace FargowiltasSouls.Core.TogglerV2;

public readonly record struct ToggleCategory(string Name) {
    public static readonly ToggleCategory UniverseSoul = new("UniverseSoul");
}