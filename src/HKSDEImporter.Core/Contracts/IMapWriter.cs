using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Contracts;

public interface IMapWriter
{
    Task WriteAsync(
        string outputPath,
        IReadOnlyCollection<MapRegion> regions,
        IReadOnlyCollection<MapConstellation> constellations,
        IReadOnlyCollection<MapSolarSystem> solarSystems,
        IReadOnlyCollection<MapJump> jumps,
        IReadOnlyCollection<MapRegionJump> regionJumps,
        IReadOnlyCollection<MapConstellationJump> constellationJumps,
        IReadOnlyCollection<MapSolarSystemJump> solarSystemJumps,
        IReadOnlyCollection<MapLandmark> landmarks,
        IReadOnlyCollection<MapDenormalizeRow> denormalizedRows,
        IReadOnlyCollection<MapCelestialGraphics> celestialGraphics,
        IReadOnlyCollection<MapCelestialStatistics> celestialStatistics,
        IReadOnlyCollection<MapUniverse> universes,
        CancellationToken cancellationToken);
}
