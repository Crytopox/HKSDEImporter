namespace HKSDEImporter.Core.Models.Domain;

public sealed record Group(
    int GroupId,
    int CategoryId,
    string Name,
    bool Published);
