namespace HKSDEImporter.Core.Models.Raw;

public sealed class RawGroup
{
    public int Key { get; init; }
    public int CategoryId { get; init; }
    public RawLocalizedText? Name { get; init; }
    public bool Published { get; init; }
}
