namespace HKSDEImporter.Core.Models.Raw;

public sealed class RawMapRegion
{
    public int Key { get; init; }
    public string? NameEn { get; init; }
    public int? FactionId { get; init; }
    public int? NebulaId { get; init; }
    public int? WormholeClassId { get; init; }
    public RawPosition3D? Position { get; init; }
}
