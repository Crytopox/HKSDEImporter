namespace HKSDEImporter.Core.Models.Domain;

public sealed record DogmaAttributeType(
    int AttributeId,
    string AttributeName,
    string? Description,
    int? IconId,
    double? DefaultValue,
    bool Published,
    string? DisplayName,
    int? UnitId,
    bool Stackable,
    bool HighIsGood,
    int? CategoryId);
