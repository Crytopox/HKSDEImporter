namespace HKSDEImporter.Core.Models.Domain;

public sealed record MapRegion(
    int RegionId,
    string RegionName,
    int? FactionId,
    double? X,
    double? Y,
    double? Z,
    int? NebulaId,
    int? WormholeClassId);
