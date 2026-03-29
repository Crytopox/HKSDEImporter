namespace HKSDEImporter.Core.Models.Raw;

public sealed class RawBlueprint
{
    public int TypeId { get; init; }
    public int MaxProductionLimit { get; init; }
    public IReadOnlyList<RawBlueprintActivity> Activities { get; init; } = [];
}

public sealed class RawBlueprintActivity
{
    public int ActivityId { get; init; }
    public int? Time { get; init; }
    public IReadOnlyList<RawBlueprintMaterial> Materials { get; init; } = [];
    public IReadOnlyList<RawBlueprintProduct> Products { get; init; } = [];
    public IReadOnlyList<RawBlueprintSkill> Skills { get; init; } = [];
}

public sealed class RawBlueprintMaterial
{
    public int TypeId { get; init; }
    public int Quantity { get; init; }
}

public sealed class RawBlueprintProduct
{
    public int TypeId { get; init; }
    public int Quantity { get; init; }
    public double? Probability { get; init; }
}

public sealed class RawBlueprintSkill
{
    public int TypeId { get; init; }
    public int Level { get; init; }
}
