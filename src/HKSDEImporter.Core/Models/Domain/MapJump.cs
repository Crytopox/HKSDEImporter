namespace HKSDEImporter.Core.Models.Domain;

public sealed record MapJump(
    int? StargateId,
    int? DestinationStargateId,
    int FromRegionId,
    int FromConstellationId,
    int FromSolarSystemId,
    int ToRegionId,
    int ToConstellationId,
    int ToSolarSystemId);
