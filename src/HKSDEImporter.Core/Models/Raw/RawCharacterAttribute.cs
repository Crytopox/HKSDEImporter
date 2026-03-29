namespace HKSDEImporter.Core.Models.Raw;

public sealed class RawCharacterAttribute
{
    public int Key { get; init; }
    public RawLocalizedText? Name { get; init; }
    public RawLocalizedText? Description { get; init; }
    public int? IconId { get; init; }
    public string? Notes { get; init; }
    public RawLocalizedText? ShortDescription { get; init; }
}
