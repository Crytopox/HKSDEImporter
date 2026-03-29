using System.Text.Json.Serialization;

namespace HKSDEImporter.Infrastructure.Json.Readers;

internal sealed class JsonlMapCelestialAttributesDto
{
    [JsonPropertyName("heightMap1")]
    public int? HeightMap1 { get; init; }

    [JsonPropertyName("heightMap2")]
    public int? HeightMap2 { get; init; }

    [JsonPropertyName("shaderPreset")]
    public int? ShaderPreset { get; init; }

    [JsonPropertyName("population")]
    public bool? Population { get; init; }
}

internal sealed class JsonlMapCelestialStatisticsDto
{
    [JsonPropertyName("temperature")]
    public double? Temperature { get; init; }

    [JsonPropertyName("spectralClass")]
    public string? SpectralClass { get; init; }

    [JsonPropertyName("luminosity")]
    public double? Luminosity { get; init; }

    [JsonPropertyName("age")]
    public double? Age { get; init; }

    [JsonPropertyName("life")]
    public double? Life { get; init; }

    [JsonPropertyName("orbitRadius")]
    public double? OrbitRadius { get; init; }

    [JsonPropertyName("eccentricity")]
    public double? Eccentricity { get; init; }

    [JsonPropertyName("massDust")]
    public double? MassDust { get; init; }

    [JsonPropertyName("massGas")]
    public double? MassGas { get; init; }

    [JsonPropertyName("fragmented")]
    public bool? Fragmented { get; init; }

    [JsonPropertyName("density")]
    public double? Density { get; init; }

    [JsonPropertyName("surfaceGravity")]
    public double? SurfaceGravity { get; init; }

    [JsonPropertyName("escapeVelocity")]
    public double? EscapeVelocity { get; init; }

    [JsonPropertyName("orbitPeriod")]
    public double? OrbitPeriod { get; init; }

    [JsonPropertyName("rotationRate")]
    public double? RotationRate { get; init; }

    [JsonPropertyName("locked")]
    public bool? Locked { get; init; }

    [JsonPropertyName("pressure")]
    public double? Pressure { get; init; }

    [JsonPropertyName("radius")]
    public double? Radius { get; init; }

    [JsonPropertyName("mass")]
    public long? Mass { get; init; }
}

internal sealed class JsonlMapPlanetDto
{
    [JsonPropertyName("_key")]
    public int Key { get; init; }

    [JsonPropertyName("typeID")]
    public int? TypeId { get; init; }

    [JsonPropertyName("solarSystemID")]
    public int? SolarSystemId { get; init; }

    [JsonPropertyName("orbitID")]
    public int? OrbitId { get; init; }

    [JsonPropertyName("celestialIndex")]
    public int? CelestialIndex { get; init; }

    [JsonPropertyName("position")]
    public JsonlPosition3DDto? Position { get; init; }

    [JsonPropertyName("radius")]
    public double? Radius { get; init; }

    [JsonPropertyName("attributes")]
    public JsonlMapCelestialAttributesDto? Attributes { get; init; }

    [JsonPropertyName("statistics")]
    public JsonlMapCelestialStatisticsDto? Statistics { get; init; }
}

internal sealed class JsonlMapMoonDto
{
    [JsonPropertyName("_key")]
    public int Key { get; init; }

    [JsonPropertyName("typeID")]
    public int? TypeId { get; init; }

    [JsonPropertyName("solarSystemID")]
    public int? SolarSystemId { get; init; }

    [JsonPropertyName("orbitID")]
    public int? OrbitId { get; init; }

    [JsonPropertyName("celestialIndex")]
    public int? CelestialIndex { get; init; }

    [JsonPropertyName("orbitIndex")]
    public int? OrbitIndex { get; init; }

    [JsonPropertyName("position")]
    public JsonlPosition3DDto? Position { get; init; }

    [JsonPropertyName("radius")]
    public double? Radius { get; init; }

    [JsonPropertyName("attributes")]
    public JsonlMapCelestialAttributesDto? Attributes { get; init; }

    [JsonPropertyName("statistics")]
    public JsonlMapCelestialStatisticsDto? Statistics { get; init; }
}

internal sealed class JsonlMapAsteroidBeltDto
{
    [JsonPropertyName("_key")]
    public int Key { get; init; }

    [JsonPropertyName("typeID")]
    public int? TypeId { get; init; }

    [JsonPropertyName("solarSystemID")]
    public int? SolarSystemId { get; init; }

    [JsonPropertyName("orbitID")]
    public int? OrbitId { get; init; }

    [JsonPropertyName("celestialIndex")]
    public int? CelestialIndex { get; init; }

    [JsonPropertyName("orbitIndex")]
    public int? OrbitIndex { get; init; }

    [JsonPropertyName("position")]
    public JsonlPosition3DDto? Position { get; init; }

    [JsonPropertyName("radius")]
    public double? Radius { get; init; }

    [JsonPropertyName("statistics")]
    public JsonlMapCelestialStatisticsDto? Statistics { get; init; }
}

internal sealed class JsonlMapStarDto
{
    [JsonPropertyName("_key")]
    public int Key { get; init; }

    [JsonPropertyName("typeID")]
    public int? TypeId { get; init; }

    [JsonPropertyName("solarSystemID")]
    public int? SolarSystemId { get; init; }

    [JsonPropertyName("radius")]
    public double? Radius { get; init; }

    [JsonPropertyName("statistics")]
    public JsonlMapCelestialStatisticsDto? Statistics { get; init; }
}

internal sealed class JsonlMapSecondarySunDto
{
    [JsonPropertyName("_key")]
    public int Key { get; init; }

    [JsonPropertyName("typeID")]
    public int? TypeId { get; init; }

    [JsonPropertyName("solarSystemID")]
    public int? SolarSystemId { get; init; }

    [JsonPropertyName("position")]
    public JsonlPosition3DDto? Position { get; init; }
}
