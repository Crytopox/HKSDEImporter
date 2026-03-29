namespace HKSDEImporter.Core.Models.Domain;

public sealed record MetaGroup(
    int MetaGroupId,
    string Name,
    string? Description,
    int? IconId);
