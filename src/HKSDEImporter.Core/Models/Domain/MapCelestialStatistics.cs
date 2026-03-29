namespace HKSDEImporter.Core.Models.Domain;

public sealed record MapCelestialStatistics(
    int CelestialId,
    double? Temperature,
    string? SpectralClass,
    double? Luminosity,
    double? Age,
    double? Life,
    double? OrbitRadius,
    double? Eccentricity,
    double? MassDust,
    double? MassGas,
    bool? Fragmented,
    double? Density,
    double? SurfaceGravity,
    double? EscapeVelocity,
    double? OrbitPeriod,
    double? RotationRate,
    bool? Locked,
    double? Pressure,
    double? Radius,
    long? Mass);
