namespace HKSDEImporter.Core.Models.Domain;

public sealed record TypeItem(
    int TypeId,
    int GroupId,
    string Name,
    string? Description,
    bool Published,
    int PortionSize,
    int? IconId,
    int? SoundId,
    int? GraphicId,
    int? MarketGroupId,
    int? RaceId,
    double? Mass,
    double? Volume,
    double? Capacity,
    double? Radius,
    decimal? BasePrice);
