namespace HKSDEImporter.Core.Models.Raw;

public sealed class RawMetaGroup
{
    public int Key { get; init; }
    public RawLocalizedText? Name { get; init; }
    public RawLocalizedText? Description { get; init; }
    public int? IconId { get; init; }
}
