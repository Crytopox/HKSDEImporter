namespace HKSDEImporter.Core.Models.Raw;

public sealed class RawStationService
{
    public int Key { get; init; }
    public RawLocalizedText? ServiceName { get; init; }
    public RawLocalizedText? Description { get; init; }
}
