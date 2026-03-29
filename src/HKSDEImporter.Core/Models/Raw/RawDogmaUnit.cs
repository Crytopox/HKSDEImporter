namespace HKSDEImporter.Core.Models.Raw;

public sealed class RawDogmaUnit
{
    public int Key { get; init; }
    public string? Name { get; init; }
    public RawLocalizedText? DisplayName { get; init; }
    public RawLocalizedText? Description { get; init; }
}
