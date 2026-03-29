namespace HKSDEImporter.Core.Models.Domain;

public sealed record DogmaUnit(
    int UnitId,
    string UnitName,
    string? DisplayName,
    string? Description);
