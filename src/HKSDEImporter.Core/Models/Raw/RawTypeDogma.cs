namespace HKSDEImporter.Core.Models.Raw;

public sealed class RawTypeDogma
{
    public int TypeId { get; init; }
    public IReadOnlyList<RawTypeDogmaAttribute> Attributes { get; init; } = [];
    public IReadOnlyList<RawTypeDogmaEffect> Effects { get; init; } = [];
}

public sealed class RawTypeDogmaAttribute
{
    public int AttributeId { get; init; }
    public double Value { get; init; }
}

public sealed class RawTypeDogmaEffect
{
    public int EffectId { get; init; }
    public bool IsDefault { get; init; }
}
