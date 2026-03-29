namespace HKSDEImporter.Core.Models.Domain;

public sealed record PlanetSchematicTypeMap(
    int SchematicId,
    int TypeId,
    int Quantity,
    bool IsInput);
