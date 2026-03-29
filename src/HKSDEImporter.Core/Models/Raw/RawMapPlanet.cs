namespace HKSDEImporter.Core.Models.Raw;

public sealed class RawMapPlanet
{
    public int Key { get; init; }
    public int? TypeId { get; init; }
    public int? SolarSystemId { get; init; }
    public int? OrbitId { get; init; }
    public int? CelestialIndex { get; init; }
    public RawPosition3D? Position { get; init; }
    public double? Radius { get; init; }
    public RawCelestialAttributes? Attributes { get; init; }
    public RawCelestialStatistics? Statistics { get; init; }
}
