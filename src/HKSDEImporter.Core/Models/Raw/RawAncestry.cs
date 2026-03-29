namespace HKSDEImporter.Core.Models.Raw;

public sealed class RawAncestry
{
    public int Key { get; init; }
    public RawLocalizedText? Name { get; init; }
    public int? BloodlineId { get; init; }
    public RawLocalizedText? Description { get; init; }
    public int? Perception { get; init; }
    public int? Willpower { get; init; }
    public int? Charisma { get; init; }
    public int? Memory { get; init; }
    public int? Intelligence { get; init; }
    public int? IconId { get; init; }
    public RawLocalizedText? ShortDescription { get; init; }
}
