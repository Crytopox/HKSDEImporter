namespace HKSDEImporter.Core.Models.Domain;

public sealed record TypeDogmaAttribute(
    int TypeId,
    int AttributeId,
    int? ValueInt,
    double? ValueFloat);
