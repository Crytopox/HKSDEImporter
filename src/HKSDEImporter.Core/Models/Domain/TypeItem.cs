namespace HKSDEImporter.Core.Models.Domain;

public sealed record TypeItem(
    int TypeId,
    int GroupId,
    string Name,
    string? Description,
    bool Published,
    int PortionSize,
    int? IconId,
    double? Mass,
    double? Radius,
    double? Volume);
