namespace HKSDEImporter.Core.Models.Raw;

public sealed class RawMapStar
{
    public int Key { get; init; }
    public int? TypeId { get; init; }
    public int? SolarSystemId { get; init; }
    public double? Radius { get; init; }
    public RawCelestialStatistics? Statistics { get; init; }
}
