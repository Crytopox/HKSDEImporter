namespace HKSDEImporter.Core.Models.Raw;

public sealed class RawCelestialAttributes
{
    public int? HeightMap1 { get; init; }
    public int? HeightMap2 { get; init; }
    public int? ShaderPreset { get; init; }
    public bool? Population { get; init; }
}

public sealed class RawCelestialStatistics
{
    public double? Temperature { get; init; }
    public string? SpectralClass { get; init; }
    public double? Luminosity { get; init; }
    public double? Age { get; init; }
    public double? Life { get; init; }
    public double? OrbitRadius { get; init; }
    public double? Eccentricity { get; init; }
    public double? MassDust { get; init; }
    public double? MassGas { get; init; }
    public bool? Fragmented { get; init; }
    public double? Density { get; init; }
    public double? SurfaceGravity { get; init; }
    public double? EscapeVelocity { get; init; }
    public double? OrbitPeriod { get; init; }
    public double? RotationRate { get; init; }
    public bool? Locked { get; init; }
    public double? Pressure { get; init; }
    public double? Radius { get; init; }
    public long? Mass { get; init; }
}
