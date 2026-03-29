namespace HKSDEImporter.Core.Models.Raw;

public sealed class RawType
{
    public int Key { get; init; }
    public int GroupId { get; init; }
    public RawLocalizedText? Name { get; init; }
    public RawLocalizedText? Description { get; init; }
    public bool Published { get; init; }
    public int PortionSize { get; init; }

    public int? IconId { get; init; }
    public int? SoundId { get; init; }
    public int? GraphicId { get; init; }
    public int? MarketGroupId { get; init; }
    public int? RaceId { get; init; }
    public int? MetaGroupId { get; init; }
    public int? VariationParentTypeId { get; init; }

    public double? Mass { get; init; }
    public double? Volume { get; init; }
    public double? Capacity { get; init; }
    public double? Radius { get; init; }
    public decimal? BasePrice { get; init; }
}
