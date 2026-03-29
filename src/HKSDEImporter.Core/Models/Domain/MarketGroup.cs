namespace HKSDEImporter.Core.Models.Domain;

public sealed record MarketGroup(
    int MarketGroupId,
    int? ParentGroupId,
    string Name,
    string? Description,
    int? IconId,
    bool HasTypes);
