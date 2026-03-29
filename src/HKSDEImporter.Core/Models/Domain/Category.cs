namespace HKSDEImporter.Core.Models.Domain;

public sealed record Category(
    int CategoryId,
    string Name,
    bool Published,
    int? IconId);
