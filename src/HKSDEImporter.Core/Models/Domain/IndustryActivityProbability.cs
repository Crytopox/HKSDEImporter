namespace HKSDEImporter.Core.Models.Domain;

public sealed record IndustryActivityProbability(
    int TypeId,
    int ActivityId,
    int ProductTypeId,
    double Probability);
