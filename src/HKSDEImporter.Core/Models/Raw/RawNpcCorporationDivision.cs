namespace HKSDEImporter.Core.Models.Raw;

public sealed class RawNpcCorporationDivision
{
    public int Key { get; init; }
    public RawLocalizedText? Name { get; init; }
    public string? DisplayName { get; init; }
    public RawLocalizedText? LeaderTypeName { get; init; }
}
