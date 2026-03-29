namespace HKSDEImporter.Core.Models.Raw;

public sealed class RawCategory
{
    public int Key { get; init; }
    public int? IconId { get; init; }
    public RawLocalizedText? Name { get; init; }
    public bool Published { get; init; }
}
