namespace HKSDEImporter.Core.Models.Raw;

public sealed class RawMapSecondarySun
{
    public int Key { get; init; }
    public int? TypeId { get; init; }
    public int? SolarSystemId { get; init; }
    public RawPosition3D? Position { get; init; }
}
