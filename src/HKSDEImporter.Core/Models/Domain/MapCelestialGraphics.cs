namespace HKSDEImporter.Core.Models.Domain;

public sealed record MapCelestialGraphics(
    int CelestialId,
    int? HeightMap1,
    int? HeightMap2,
    int? ShaderPreset,
    bool? Population);
