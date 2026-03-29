namespace HKSDEImporter.Core.Models.Domain;

public sealed record MapConstellation(
    int ConstellationId,
    int? RegionId,
    string ConstellationName,
    int? FactionId,
    double? X,
    double? Y,
    double? Z,
    int? WormholeClassId);
