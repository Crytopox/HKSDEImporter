namespace HKSDEImporter.Core.Models.Raw;

public sealed class RawType
{
    public int Key { get; init; }
    public int GroupId { get; init; }
    public RawLocalizedText? Description { get; init; }
    public int? IconId { get; init; }
    public double? Mass { get; init; }
    public RawLocalizedText? Name { get; init; }
    public int PortionSize { get; init; }
    public bool Published { get; init; }
    public double? Radius { get; init; }
    public double? Volume { get; init; }
}
