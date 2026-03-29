namespace HKSDEImporter.Core.Models.Domain;

public sealed record MapDenormalizeRow(
    int ItemId,
    int? TypeId,
    int? GroupId,
    int? SolarSystemId,
    int? ConstellationId,
    int? RegionId,
    int? OrbitId,
    double? X,
    double? Y,
    double? Z,
    double? Radius,
    string? ItemName,
    double? Security,
    int? CelestialIndex,
    int? OrbitIndex);
