namespace HKSDEImporter.Core.Models.Domain;

public sealed record IndustryActivityMaterial(
    int TypeId,
    int ActivityId,
    int MaterialTypeId,
    int Quantity);
