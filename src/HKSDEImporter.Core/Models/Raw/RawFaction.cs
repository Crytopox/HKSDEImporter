namespace HKSDEImporter.Core.Models.Raw;

public sealed class RawFaction
{
    public int Key { get; init; }
    public RawLocalizedText? Name { get; init; }
    public RawLocalizedText? Description { get; init; }
    public List<int>? MemberRaces { get; init; }
    public int? SolarSystemId { get; init; }
    public int? CorporationId { get; init; }
    public double? SizeFactor { get; init; }
    public int? MilitiaCorporationId { get; init; }
    public int? IconId { get; init; }
}
