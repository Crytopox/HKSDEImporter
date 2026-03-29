namespace HKSDEImporter.Core.Models.Raw;

public sealed class RawMarketGroup
{
    public int Key { get; init; }
    public int? ParentGroupId { get; init; }
    public RawLocalizedText? Name { get; init; }
    public RawLocalizedText? Description { get; init; }
    public int? IconId { get; init; }
    public bool HasTypes { get; init; }
}
