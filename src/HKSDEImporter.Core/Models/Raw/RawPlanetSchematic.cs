namespace HKSDEImporter.Core.Models.Raw;

public sealed class RawPlanetSchematic
{
    public int SchematicId { get; init; }
    public string? Name { get; init; }
    public int CycleTime { get; init; }
    public IReadOnlyList<int> PinTypeIds { get; init; } = [];
    public IReadOnlyList<RawPlanetSchematicType> Types { get; init; } = [];
}

public sealed class RawPlanetSchematicType
{
    public int TypeId { get; init; }
    public int Quantity { get; init; }
    public bool IsInput { get; init; }
}
