namespace HKSDEImporter.Core.Models.Raw;

public sealed class RawBloodline
{
    public int Key { get; init; }
    public RawLocalizedText? Name { get; init; }
    public int? RaceId { get; init; }
    public RawLocalizedText? Description { get; init; }
    public RawLocalizedText? MaleDescription { get; init; }
    public RawLocalizedText? FemaleDescription { get; init; }
    public int? ShipTypeId { get; init; }
    public int? CorporationId { get; init; }
    public int? Perception { get; init; }
    public int? Willpower { get; init; }
    public int? Charisma { get; init; }
    public int? Memory { get; init; }
    public int? Intelligence { get; init; }
    public int? IconId { get; init; }
    public RawLocalizedText? ShortDescription { get; init; }
    public RawLocalizedText? ShortMaleDescription { get; init; }
    public RawLocalizedText? ShortFemaleDescription { get; init; }
}
