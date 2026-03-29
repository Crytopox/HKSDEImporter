using System.Globalization;
using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Mapping;
using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Models.Raw;
using HKSDEImporter.Core.Validation;

namespace HKSDEImporter.Core.ImportSteps;

public sealed class ImportMapDataStep : IImportStep
{
    private const int GroupRegion = 3;
    private const int GroupConstellation = 4;
    private const int GroupSolarSystem = 5;
    private const int GroupStar = 6;
    private const int GroupPlanet = 7;
    private const int GroupMoon = 8;
    private const int GroupAsteroidBelt = 9;
    private const int GroupStargate = 10;
    private const int GroupSecondarySun = 995;

    private readonly IRawSdeReader _reader;
    private readonly MapDataMapper _mapper;
    private readonly IValidator<MapRegion> _regionValidator;
    private readonly IValidator<MapConstellation> _constellationValidator;
    private readonly IValidator<MapSolarSystem> _solarSystemValidator;
    private readonly IValidator<MapLandmark> _landmarkValidator;
    private readonly IMapWriter _writer;
    private readonly IImportObserver _observer;

    public ImportMapDataStep(
        IRawSdeReader reader,
        MapDataMapper mapper,
        IValidator<MapRegion> regionValidator,
        IValidator<MapConstellation> constellationValidator,
        IValidator<MapSolarSystem> solarSystemValidator,
        IValidator<MapLandmark> landmarkValidator,
        IMapWriter writer,
        IImportObserver observer)
    {
        _reader = reader;
        _mapper = mapper;
        _regionValidator = regionValidator;
        _constellationValidator = constellationValidator;
        _solarSystemValidator = solarSystemValidator;
        _landmarkValidator = landmarkValidator;
        _writer = writer;
        _observer = observer;
    }

    public string Name => "Import Map Data";

    public async Task ExecuteAsync(ImportContext context, CancellationToken cancellationToken)
    {
        var input = context.ResolvedInputDirectory!;
        var total =
            await _reader.CountMapRegionsAsync(input, cancellationToken) +
            await _reader.CountMapConstellationsAsync(input, cancellationToken) +
            await _reader.CountMapSolarSystemsAsync(input, cancellationToken) +
            await _reader.CountMapStargatesAsync(input, cancellationToken) +
            await _reader.CountLandmarksAsync(input, cancellationToken) +
            await _reader.CountMapPlanetsAsync(input, cancellationToken) +
            await _reader.CountMapMoonsAsync(input, cancellationToken) +
            await _reader.CountMapAsteroidBeltsAsync(input, cancellationToken) +
            await _reader.CountMapStarsAsync(input, cancellationToken) +
            await _reader.CountMapSecondarySunsAsync(input, cancellationToken) +
            await _reader.CountNpcStationsAsync(input, cancellationToken);

        _observer.OnStepStarted(Name, total);

        var regions = new List<MapRegion>();
        var constellations = new List<MapConstellation>();
        var solarSystems = new List<MapSolarSystem>();
        var jumps = new List<MapJump>();
        var regionJumps = new List<MapRegionJump>();
        var constellationJumps = new List<MapConstellationJump>();
        var solarSystemJumps = new List<MapSolarSystemJump>();
        var landmarks = new List<MapLandmark>();

        var denormalizedRows = new List<MapDenormalizeRow>();
        var celestialGraphics = new List<MapCelestialGraphics>();
        var celestialStatistics = new List<MapCelestialStatistics>();
        var universes = new List<MapUniverse>();

        var seenRegion = new HashSet<int>();
        var seenConstellation = new HashSet<int>();
        var seenSystem = new HashSet<int>();
        var seenJump = new HashSet<string>(StringComparer.Ordinal);
        var seenRegionJump = new HashSet<string>(StringComparer.Ordinal);
        var seenConstellationJump = new HashSet<string>(StringComparer.Ordinal);
        var seenSystemJump = new HashSet<string>(StringComparer.Ordinal);
        var seenLandmark = new HashSet<int>();

        var seenDenormalized = new HashSet<int>();
        var seenCelestialGraphics = new HashSet<int>();
        var seenCelestialStatistics = new HashSet<int>();

        var processed = 0L;

        await foreach (var raw in _reader.ReadMapRegionsAsync(input, cancellationToken))
        {
            processed++;
            if (processed % 2000 == 0) _observer.OnStepProgress(Name, processed);

            var mapped = _mapper.MapRegion(raw);
            if (!seenRegion.Add(mapped.RegionId))
            {
                AddWarning(context, $"Duplicate region id {mapped.RegionId}. Keeping first occurrence.");
                continue;
            }

            var result = _regionValidator.Validate(mapped);
            if (!result.IsValid)
            {
                AddWarning(context, result.Message ?? "Invalid map region row.");
                continue;
            }

            regions.Add(mapped);

            AddDenormalizedRow(
                denormalizedRows,
                seenDenormalized,
                new MapDenormalizeRow(
                    mapped.RegionId,
                    GroupRegion,
                    GroupRegion,
                    null,
                    null,
                    null,
                    null,
                    mapped.X,
                    mapped.Y,
                    mapped.Z,
                    null,
                    mapped.RegionName,
                    null,
                    null,
                    null));
        }

        await foreach (var raw in _reader.ReadMapConstellationsAsync(input, cancellationToken))
        {
            processed++;
            if (processed % 2000 == 0) _observer.OnStepProgress(Name, processed);

            var mapped = _mapper.MapConstellation(raw);
            if (!seenConstellation.Add(mapped.ConstellationId))
            {
                AddWarning(context, $"Duplicate constellation id {mapped.ConstellationId}. Keeping first occurrence.");
                continue;
            }

            var result = _constellationValidator.Validate(mapped);
            if (!result.IsValid)
            {
                AddWarning(context, result.Message ?? "Invalid map constellation row.");
                continue;
            }

            constellations.Add(mapped);

            AddDenormalizedRow(
                denormalizedRows,
                seenDenormalized,
                new MapDenormalizeRow(
                    mapped.ConstellationId,
                    GroupConstellation,
                    GroupConstellation,
                    null,
                    null,
                    mapped.RegionId,
                    null,
                    mapped.X,
                    mapped.Y,
                    mapped.Z,
                    null,
                    mapped.ConstellationName,
                    null,
                    null,
                    null));
        }

        await foreach (var raw in _reader.ReadMapSolarSystemsAsync(input, cancellationToken))
        {
            processed++;
            if (processed % 2000 == 0) _observer.OnStepProgress(Name, processed);

            var mapped = _mapper.MapSolarSystem(raw);
            if (!seenSystem.Add(mapped.SolarSystemId))
            {
                AddWarning(context, $"Duplicate solar system id {mapped.SolarSystemId}. Keeping first occurrence.");
                continue;
            }

            var result = _solarSystemValidator.Validate(mapped);
            if (!result.IsValid)
            {
                AddWarning(context, result.Message ?? "Invalid map solar system row.");
                continue;
            }

            solarSystems.Add(mapped);

            AddDenormalizedRow(
                denormalizedRows,
                seenDenormalized,
                new MapDenormalizeRow(
                    mapped.SolarSystemId,
                    GroupSolarSystem,
                    GroupSolarSystem,
                    null,
                    mapped.ConstellationId,
                    mapped.RegionId,
                    null,
                    mapped.X,
                    mapped.Y,
                    mapped.Z,
                    mapped.Radius,
                    mapped.SolarSystemName,
                    mapped.Security,
                    null,
                    null));
        }

        var regionLookup = regions.ToDictionary(r => r.RegionId);
        var constellationLookup = constellations.ToDictionary(c => c.ConstellationId);
        var systemLookup = solarSystems.ToDictionary(s => s.SolarSystemId);

        await foreach (var gate in _reader.ReadMapStargatesAsync(input, cancellationToken))
        {
            processed++;
            if (processed % 3000 == 0) _observer.OnStepProgress(Name, processed);

            if (!gate.SolarSystemId.HasValue || !gate.DestinationSolarSystemId.HasValue)
            {
                continue;
            }

            if (!systemLookup.TryGetValue(gate.SolarSystemId.Value, out var fromSystem) ||
                !systemLookup.TryGetValue(gate.DestinationSolarSystemId.Value, out var toSystem) ||
                !fromSystem.RegionId.HasValue || !fromSystem.ConstellationId.HasValue ||
                !toSystem.RegionId.HasValue || !toSystem.ConstellationId.HasValue)
            {
                continue;
            }

            var jumpKey = $"{gate.StargateId}:{gate.DestinationStargateId}";
            if (seenJump.Add(jumpKey))
            {
                jumps.Add(new MapJump(
                    gate.StargateId,
                    gate.DestinationStargateId,
                    fromSystem.RegionId.Value,
                    fromSystem.ConstellationId.Value,
                    fromSystem.SolarSystemId,
                    toSystem.RegionId.Value,
                    toSystem.ConstellationId.Value,
                    toSystem.SolarSystemId));
            }

            var regionKey = $"{fromSystem.RegionId.Value}:{toSystem.RegionId.Value}";
            if (seenRegionJump.Add(regionKey))
            {
                regionJumps.Add(new MapRegionJump(fromSystem.RegionId.Value, toSystem.RegionId.Value));
            }

            var constellationKey = $"{fromSystem.ConstellationId.Value}:{toSystem.ConstellationId.Value}";
            if (seenConstellationJump.Add(constellationKey))
            {
                constellationJumps.Add(new MapConstellationJump(fromSystem.ConstellationId.Value, toSystem.ConstellationId.Value));
            }

            var systemKey = $"{fromSystem.SolarSystemId}:{toSystem.SolarSystemId}";
            if (seenSystemJump.Add(systemKey))
            {
                solarSystemJumps.Add(new MapSolarSystemJump(fromSystem.SolarSystemId, toSystem.SolarSystemId));
            }

            var destinationName = toSystem.SolarSystemName;
            var gateName = string.IsNullOrWhiteSpace(destinationName)
                ? "Stargate"
                : $"Stargate ({destinationName})";

            AddDenormalizedRow(
                denormalizedRows,
                seenDenormalized,
                new MapDenormalizeRow(
                    gate.StargateId,
                    null,
                    GroupStargate,
                    fromSystem.SolarSystemId,
                    fromSystem.ConstellationId,
                    fromSystem.RegionId,
                    null,
                    null,
                    null,
                    null,
                    null,
                    gateName,
                    fromSystem.Security,
                    null,
                    null));
        }

        var planetNameById = new Dictionary<int, string>();

        await foreach (var raw in _reader.ReadMapStarsAsync(input, cancellationToken))
        {
            processed++;
            if (processed % 2000 == 0) _observer.OnStepProgress(Name, processed);

            if (!raw.SolarSystemId.HasValue || !systemLookup.TryGetValue(raw.SolarSystemId.Value, out var system))
            {
                continue;
            }

            var name = string.IsNullOrWhiteSpace(system.SolarSystemName)
                ? "Star"
                : $"{system.SolarSystemName} - Star";

            AddDenormalizedRow(
                denormalizedRows,
                seenDenormalized,
                new MapDenormalizeRow(
                    raw.Key,
                    raw.TypeId,
                    GroupStar,
                    system.SolarSystemId,
                    system.ConstellationId,
                    system.RegionId,
                    null,
                    null,
                    null,
                    null,
                    raw.Radius,
                    name,
                    system.Security,
                    null,
                    null));

            AddCelestialStatistics(raw.Key, raw.Statistics, raw.Radius, celestialStatistics, seenCelestialStatistics);
        }

        await foreach (var raw in _reader.ReadMapPlanetsAsync(input, cancellationToken))
        {
            processed++;
            if (processed % 2000 == 0) _observer.OnStepProgress(Name, processed);

            if (!raw.SolarSystemId.HasValue || !systemLookup.TryGetValue(raw.SolarSystemId.Value, out var system))
            {
                continue;
            }

            var index = raw.CelestialIndex ?? 0;
            var planetName = index > 0
                ? $"{system.SolarSystemName} {ToRoman(index)}"
                : $"{system.SolarSystemName} Planet";
            planetNameById[raw.Key] = planetName;

            AddDenormalizedRow(
                denormalizedRows,
                seenDenormalized,
                new MapDenormalizeRow(
                    raw.Key,
                    raw.TypeId,
                    GroupPlanet,
                    system.SolarSystemId,
                    system.ConstellationId,
                    system.RegionId,
                    raw.OrbitId,
                    raw.Position?.X,
                    raw.Position?.Y,
                    raw.Position?.Z,
                    raw.Radius,
                    planetName,
                    system.Security,
                    raw.CelestialIndex,
                    null));

            AddCelestialGraphics(raw.Key, raw.Attributes, celestialGraphics, seenCelestialGraphics);
            AddCelestialStatistics(raw.Key, raw.Statistics, raw.Radius, celestialStatistics, seenCelestialStatistics);
        }

        await foreach (var raw in _reader.ReadMapMoonsAsync(input, cancellationToken))
        {
            processed++;
            if (processed % 2000 == 0) _observer.OnStepProgress(Name, processed);

            if (!raw.SolarSystemId.HasValue || !systemLookup.TryGetValue(raw.SolarSystemId.Value, out var system))
            {
                continue;
            }

            var parentName = raw.OrbitId.HasValue && planetNameById.TryGetValue(raw.OrbitId.Value, out var value)
                ? value
                : system.SolarSystemName;
            var moonLabel = raw.OrbitIndex ?? 0;
            var moonName = moonLabel > 0 ? $"{parentName} - Moon {moonLabel}" : $"{parentName} - Moon";

            AddDenormalizedRow(
                denormalizedRows,
                seenDenormalized,
                new MapDenormalizeRow(
                    raw.Key,
                    raw.TypeId,
                    GroupMoon,
                    system.SolarSystemId,
                    system.ConstellationId,
                    system.RegionId,
                    raw.OrbitId,
                    raw.Position?.X,
                    raw.Position?.Y,
                    raw.Position?.Z,
                    raw.Radius,
                    moonName,
                    system.Security,
                    raw.CelestialIndex,
                    raw.OrbitIndex));

            AddCelestialGraphics(raw.Key, raw.Attributes, celestialGraphics, seenCelestialGraphics);
            AddCelestialStatistics(raw.Key, raw.Statistics, raw.Radius, celestialStatistics, seenCelestialStatistics);
        }

        await foreach (var raw in _reader.ReadMapAsteroidBeltsAsync(input, cancellationToken))
        {
            processed++;
            if (processed % 3000 == 0) _observer.OnStepProgress(Name, processed);

            if (!raw.SolarSystemId.HasValue || !systemLookup.TryGetValue(raw.SolarSystemId.Value, out var system))
            {
                continue;
            }

            var parentName = raw.OrbitId.HasValue && planetNameById.TryGetValue(raw.OrbitId.Value, out var value)
                ? value
                : system.SolarSystemName;
            var beltLabel = raw.OrbitIndex ?? 0;
            var beltName = beltLabel > 0
                ? $"{parentName} - Asteroid Belt {beltLabel}"
                : $"{parentName} - Asteroid Belt";

            AddDenormalizedRow(
                denormalizedRows,
                seenDenormalized,
                new MapDenormalizeRow(
                    raw.Key,
                    raw.TypeId,
                    GroupAsteroidBelt,
                    system.SolarSystemId,
                    system.ConstellationId,
                    system.RegionId,
                    raw.OrbitId,
                    raw.Position?.X,
                    raw.Position?.Y,
                    raw.Position?.Z,
                    raw.Radius,
                    beltName,
                    system.Security,
                    raw.CelestialIndex,
                    raw.OrbitIndex));

            AddCelestialStatistics(raw.Key, raw.Statistics, raw.Radius, celestialStatistics, seenCelestialStatistics);
        }

        await foreach (var raw in _reader.ReadMapSecondarySunsAsync(input, cancellationToken))
        {
            processed++;
            if (processed % 3000 == 0) _observer.OnStepProgress(Name, processed);

            if (!raw.SolarSystemId.HasValue || !systemLookup.TryGetValue(raw.SolarSystemId.Value, out var system))
            {
                continue;
            }

            AddDenormalizedRow(
                denormalizedRows,
                seenDenormalized,
                new MapDenormalizeRow(
                    raw.Key,
                    raw.TypeId,
                    GroupSecondarySun,
                    system.SolarSystemId,
                    system.ConstellationId,
                    system.RegionId,
                    null,
                    raw.Position?.X,
                    raw.Position?.Y,
                    raw.Position?.Z,
                    null,
                    "Unknown Anomaly",
                    system.Security,
                    null,
                    null));
        }

        await foreach (var raw in _reader.ReadNpcStationsAsync(input, cancellationToken))
        {
            processed++;
            if (processed % 3000 == 0) _observer.OnStepProgress(Name, processed);

            if (!raw.SolarSystemId.HasValue || !systemLookup.TryGetValue(raw.SolarSystemId.Value, out var system))
            {
                continue;
            }

            AddDenormalizedRow(
                denormalizedRows,
                seenDenormalized,
                new MapDenormalizeRow(
                    (int)raw.Key,
                    raw.TypeId,
                    15,
                    system.SolarSystemId,
                    system.ConstellationId,
                    system.RegionId,
                    null,
                    raw.Position?.X,
                    raw.Position?.Y,
                    raw.Position?.Z,
                    null,
                    null,
                    system.Security,
                    null,
                    null));
        }

        await foreach (var raw in _reader.ReadLandmarksAsync(input, cancellationToken))
        {
            processed++;
            if (processed % 2000 == 0) _observer.OnStepProgress(Name, processed);

            var mapped = _mapper.MapLandmark(raw);
            if (!seenLandmark.Add(mapped.LandmarkId))
            {
                AddWarning(context, $"Duplicate landmark id {mapped.LandmarkId}. Keeping first occurrence.");
                continue;
            }

            var result = _landmarkValidator.Validate(mapped);
            if (!result.IsValid)
            {
                AddWarning(context, result.Message ?? "Invalid landmark row.");
                continue;
            }

            landmarks.Add(mapped);
        }

        var knownSpaceRegions = regions.Where(r => r.RegionId < 11000000).ToList();
        var wormholeRegions = regions.Where(r => r.RegionId >= 11000000 && r.RegionId < 12000000).ToList();

        AddUniverse(universes, 9, string.Empty, knownSpaceRegions);
        AddUniverse(universes, 9000001, "EVE Wormhole Universe", wormholeRegions);

        await _writer.WriteAsync(
            context.Options.OutputPath,
            regions,
            constellations,
            solarSystems,
            jumps,
            regionJumps,
            constellationJumps,
            solarSystemJumps,
            landmarks,
            denormalizedRows,
            celestialGraphics,
            celestialStatistics,
            universes,
            cancellationToken);

        context.SetRowCount("mapRegions", regions.Count);
        context.SetRowCount("mapConstellations", constellations.Count);
        context.SetRowCount("mapSolarSystems", solarSystems.Count);
        context.SetRowCount("mapJumps", jumps.Count);
        context.SetRowCount("mapRegionJumps", regionJumps.Count);
        context.SetRowCount("mapConstellationJumps", constellationJumps.Count);
        context.SetRowCount("mapSolarSystemJumps", solarSystemJumps.Count);
        context.SetRowCount("mapLandmarks", landmarks.Count);
        context.SetRowCount("mapDenormalize", denormalizedRows.Count);
        context.SetRowCount("mapCelestialGraphics", celestialGraphics.Count);
        context.SetRowCount("mapCelestialStatistics", celestialStatistics.Count);
        context.SetRowCount("mapUniverse", universes.Count);
        _observer.OnStepProgress(Name, processed);
    }

    private static void AddDenormalizedRow(List<MapDenormalizeRow> rows, HashSet<int> seenIds, MapDenormalizeRow row)
    {
        if (seenIds.Add(row.ItemId))
        {
            rows.Add(row);
        }
    }

    private static void AddCelestialGraphics(int celestialId, RawCelestialAttributes? attributes, List<MapCelestialGraphics> rows, HashSet<int> seenIds)
    {
        if (attributes is null || !seenIds.Add(celestialId))
        {
            return;
        }

        rows.Add(new MapCelestialGraphics(
            celestialId,
            attributes.HeightMap1,
            attributes.HeightMap2,
            attributes.ShaderPreset,
            attributes.Population));
    }

    private static void AddCelestialStatistics(int celestialId, RawCelestialStatistics? statistics, double? fallbackRadius, List<MapCelestialStatistics> rows, HashSet<int> seenIds)
    {
        if (statistics is null || !seenIds.Add(celestialId))
        {
            return;
        }

        rows.Add(new MapCelestialStatistics(
            celestialId,
            statistics.Temperature,
            statistics.SpectralClass,
            statistics.Luminosity,
            statistics.Age,
            statistics.Life,
            statistics.OrbitRadius,
            statistics.Eccentricity,
            statistics.MassDust,
            statistics.MassGas,
            statistics.Fragmented,
            statistics.Density,
            statistics.SurfaceGravity,
            statistics.EscapeVelocity,
            statistics.OrbitPeriod,
            statistics.RotationRate,
            statistics.Locked,
            statistics.Pressure,
            statistics.Radius ?? fallbackRadius,
            statistics.Mass));
    }

    private static void AddUniverse(List<MapUniverse> rows, int universeId, string universeName, IReadOnlyCollection<MapRegion> regions)
    {
        if (regions.Count == 0)
        {
            return;
        }

        var xMin = regions.Min(r => r.X ?? 0d);
        var xMax = regions.Max(r => r.X ?? 0d);
        var yMin = regions.Min(r => r.Y ?? 0d);
        var yMax = regions.Max(r => r.Y ?? 0d);
        var zMin = regions.Min(r => r.Z ?? 0d);
        var zMax = regions.Max(r => r.Z ?? 0d);

        var radius = Math.Max(Math.Max((xMax - xMin) / 2d, (yMax - yMin) / 2d), (zMax - zMin) / 2d);

        rows.Add(new MapUniverse(
            universeId,
            universeName,
            (xMin + xMax) / 2d,
            (yMin + yMax) / 2d,
            (zMin + zMax) / 2d,
            xMin,
            xMax,
            yMin,
            yMax,
            universeId == 9000001 ? Math.Abs(zMin) : zMin,
            universeId == 9000001 ? Math.Abs(zMax) : zMax,
            radius));
    }

    private static string ToRoman(int value)
    {
        if (value <= 0)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        var numerals = new (int Value, string Symbol)[]
        {
            (1000, "M"),
            (900, "CM"),
            (500, "D"),
            (400, "CD"),
            (100, "C"),
            (90, "XC"),
            (50, "L"),
            (40, "XL"),
            (10, "X"),
            (9, "IX"),
            (5, "V"),
            (4, "IV"),
            (1, "I")
        };

        var remainder = value;
        var result = string.Empty;
        foreach (var (number, symbol) in numerals)
        {
            while (remainder >= number)
            {
                result += symbol;
                remainder -= number;
            }
        }

        return result;
    }

    private void AddWarning(ImportContext context, string warning)
    {
        context.AddWarning(warning);
        _observer.OnWarning(warning);
    }
}
