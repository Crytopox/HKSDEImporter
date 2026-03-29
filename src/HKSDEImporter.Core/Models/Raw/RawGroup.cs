namespace HKSDEImporter.Core.Models.Raw;

public sealed class RawGroup
{
    public int Key { get; init; }
    public int CategoryId { get; init; }
    public int? IconId { get; init; }
    public bool UseBasePrice { get; init; }
    public bool Anchored { get; init; }
    public bool Anchorable { get; init; }
    public bool FittableNonSingleton { get; init; }
    public RawLocalizedText? Name { get; init; }
    public bool Published { get; init; }
}
