using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Models.Raw;

namespace HKSDEImporter.Core.Mapping;

public sealed class MapDataMapper
{
    public MapRegion MapRegion(RawMapRegion raw)
    {
        return new MapRegion(
            raw.Key,
            raw.NameEn?.Trim() ?? string.Empty,
            raw.FactionId,
            raw.Position?.X,
            raw.Position?.Y,
            raw.Position?.Z,
            raw.NebulaId,
            raw.WormholeClassId);
    }

    public MapConstellation MapConstellation(RawMapConstellation raw)
    {
        return new MapConstellation(
            raw.Key,
            raw.RegionId,
            raw.NameEn?.Trim() ?? string.Empty,
            raw.FactionId,
            raw.Position?.X,
            raw.Position?.Y,
            raw.Position?.Z,
            raw.WormholeClassId);
    }

    public MapSolarSystem MapSolarSystem(RawMapSolarSystem raw)
    {
        return new MapSolarSystem(
            raw.Key,
            raw.RegionId,
            raw.ConstellationId,
            raw.NameEn?.Trim() ?? string.Empty,
            raw.SecurityStatus,
            raw.SecurityClass?.Trim(),
            raw.FactionId,
            raw.StarId,
            raw.Position?.X,
            raw.Position?.Y,
            raw.Position?.Z,
            raw.Radius,
            raw.Luminosity,
            raw.Border,
            raw.Fringe,
            raw.Corridor,
            raw.Hub,
            raw.International,
            raw.Regional,
            raw.WormholeClassId);
    }

    public MapLandmark MapLandmark(RawLandmark raw)
    {
        return new MapLandmark(
            raw.Key,
            raw.NameEn?.Trim() ?? string.Empty,
            raw.DescriptionEn?.Trim(),
            raw.IconId,
            raw.Position?.X,
            raw.Position?.Y,
            raw.Position?.Z);
    }
}
