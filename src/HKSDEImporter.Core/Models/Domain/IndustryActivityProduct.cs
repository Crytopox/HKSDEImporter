namespace HKSDEImporter.Core.Models.Domain;

public sealed record IndustryActivityProduct(
    int TypeId,
    int ActivityId,
    int ProductTypeId,
    int Quantity);
