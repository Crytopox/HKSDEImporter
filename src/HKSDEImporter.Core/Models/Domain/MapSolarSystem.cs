namespace HKSDEImporter.Core.Models.Domain;

public sealed record MapSolarSystem(
    int SolarSystemId,
    int? RegionId,
    int? ConstellationId,
    string SolarSystemName,
    double? Security,
    string? SecurityClass,
    int? FactionId,
    int? StarId,
    double? X,
    double? Y,
    double? Z,
    double? Radius,
    double? Luminosity,
    bool Border,
    bool Fringe,
    bool Corridor,
    bool Hub,
    bool International,
    bool Regional,
    int? WormholeClassId);
