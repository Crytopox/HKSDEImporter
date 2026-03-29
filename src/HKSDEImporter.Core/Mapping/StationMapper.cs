using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Models.Raw;

namespace HKSDEImporter.Core.Mapping;

public sealed class StationMapper
{
    public StationOperation MapOperation(RawStationOperation raw)
    {
        int? StationTypeForRace(int raceKey) => raw.StationTypes?.FirstOrDefault(x => x.RaceKey == raceKey)?.StationTypeId;

        return new StationOperation(
            raw.Key,
            raw.ActivityId,
            raw.OperationName?.En?.Trim(),
            raw.Description?.En?.Trim(),
            raw.Fringe,
            raw.Corridor,
            raw.Hub,
            raw.Border,
            raw.Ratio,
            StationTypeForRace(1),
            StationTypeForRace(2),
            StationTypeForRace(4),
            StationTypeForRace(8),
            StationTypeForRace(16));
    }

    public IEnumerable<StationOperationService> MapOperationServices(RawStationOperation raw)
    {
        foreach (var serviceId in raw.Services ?? [])
        {
            yield return new StationOperationService(raw.Key, serviceId);
        }
    }

    public StationService MapService(RawStationService raw)
    {
        return new StationService(raw.Key, raw.ServiceName?.En?.Trim(), raw.Description?.En?.Trim());
    }

    public Station MapStation(RawNpcStation raw, IReadOnlyDictionary<int, RawMapSolarSystem> solarSystems)
    {
        solarSystems.TryGetValue(raw.SolarSystemId ?? 0, out var system);

        return new Station(
            raw.Key,
            system?.SecurityStatus,
            null,
            null,
            null,
            raw.OperationId,
            raw.TypeId,
            raw.OwnerId,
            raw.SolarSystemId,
            system?.ConstellationId,
            system?.RegionId,
            null,
            raw.Position?.X,
            raw.Position?.Y,
            raw.Position?.Z,
            raw.ReprocessingEfficiency,
            raw.ReprocessingStationsTake,
            raw.ReprocessingHangarFlag);
    }
}
