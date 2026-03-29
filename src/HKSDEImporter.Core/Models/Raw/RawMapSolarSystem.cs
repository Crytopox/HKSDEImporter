namespace HKSDEImporter.Core.Models.Raw;

public sealed class RawMapSolarSystem
{
    public int Key { get; init; }
    public int? ConstellationId { get; init; }
    public int? RegionId { get; init; }
    public int? FactionId { get; init; }
    public int? WormholeClassId { get; init; }
    public int? StarId { get; init; }
    public string? NameEn { get; init; }
    public double? SecurityStatus { get; init; }
    public string? SecurityClass { get; init; }
    public double? Luminosity { get; init; }
    public bool Border { get; init; }
    public bool Fringe { get; init; }
    public bool Corridor { get; init; }
    public bool Hub { get; init; }
    public bool International { get; init; }
    public bool Regional { get; init; }
    public RawPosition3D? Position { get; init; }
    public double? Radius { get; init; }
}
