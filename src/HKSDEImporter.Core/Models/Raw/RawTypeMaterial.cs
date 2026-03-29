namespace HKSDEImporter.Core.Models.Raw;

public sealed class RawTypeMaterial
{
    public int TypeId { get; init; }
    public IReadOnlyList<RawTypeMaterialItem> Materials { get; init; } = [];
}

public sealed class RawTypeMaterialItem
{
    public int MaterialTypeId { get; init; }
    public int Quantity { get; init; }
}
