using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Infrastructure.Sqlite.Connections;

namespace HKSDEImporter.Infrastructure.Sqlite.Writers;

public sealed class MapWriter : IMapWriter
{
    private readonly SqliteConnectionFactory _connectionFactory;

    public MapWriter(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task WriteAsync(
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
        CancellationToken cancellationToken)
    {
        await using var connection = _connectionFactory.Create(Path.GetFullPath(outputPath));
        await connection.OpenAsync(cancellationToken);
        using var transaction = connection.BeginTransaction();

        await InsertRegionsAsync(connection, transaction, regions, cancellationToken);
        await InsertConstellationsAsync(connection, transaction, constellations, cancellationToken);
        await InsertSolarSystemsAsync(connection, transaction, solarSystems, cancellationToken);
        await InsertJumpsAsync(connection, transaction, jumps, cancellationToken);
        await InsertRegionJumpsAsync(connection, transaction, regionJumps, cancellationToken);
        await InsertConstellationJumpsAsync(connection, transaction, constellationJumps, cancellationToken);
        await InsertSolarSystemJumpsAsync(connection, transaction, solarSystemJumps, cancellationToken);
        await InsertLandmarksAsync(connection, transaction, landmarks, cancellationToken);
        await InsertMapDenormalizeAsync(connection, transaction, denormalizedRows, cancellationToken);
        await InsertMapCelestialGraphicsAsync(connection, transaction, celestialGraphics, cancellationToken);
        await InsertMapCelestialStatisticsAsync(connection, transaction, celestialStatistics, cancellationToken);
        await InsertMapUniverseAsync(connection, transaction, universes, cancellationToken);

        transaction.Commit();
    }

    private static async Task InsertRegionsAsync(Microsoft.Data.Sqlite.SqliteConnection connection, Microsoft.Data.Sqlite.SqliteTransaction transaction, IReadOnlyCollection<MapRegion> rows, CancellationToken cancellationToken)
    {
        using var cmd = connection.CreateCommand();
        cmd.Transaction = transaction;
        cmd.CommandText = "INSERT INTO mapRegions (regionID, regionName, x, y, z, factionID, nebulaID, wormholeClassID) VALUES ($regionID, $regionName, $x, $y, $z, $factionID, $nebulaID, $wormholeClassID);";

        var regionId = cmd.CreateParameter(); regionId.ParameterName = "$regionID"; cmd.Parameters.Add(regionId);
        var regionName = cmd.CreateParameter(); regionName.ParameterName = "$regionName"; cmd.Parameters.Add(regionName);
        var x = cmd.CreateParameter(); x.ParameterName = "$x"; cmd.Parameters.Add(x);
        var y = cmd.CreateParameter(); y.ParameterName = "$y"; cmd.Parameters.Add(y);
        var z = cmd.CreateParameter(); z.ParameterName = "$z"; cmd.Parameters.Add(z);
        var factionId = cmd.CreateParameter(); factionId.ParameterName = "$factionID"; cmd.Parameters.Add(factionId);
        var nebulaId = cmd.CreateParameter(); nebulaId.ParameterName = "$nebulaID"; cmd.Parameters.Add(nebulaId);
        var wormholeClassId = cmd.CreateParameter(); wormholeClassId.ParameterName = "$wormholeClassID"; cmd.Parameters.Add(wormholeClassId);

        foreach (var row in rows)
        {
            regionId.Value = row.RegionId;
            regionName.Value = row.RegionName;
            x.Value = row.X.HasValue ? row.X.Value : DBNull.Value;
            y.Value = row.Y.HasValue ? row.Y.Value : DBNull.Value;
            z.Value = row.Z.HasValue ? row.Z.Value : DBNull.Value;
            factionId.Value = row.FactionId.HasValue ? row.FactionId.Value : DBNull.Value;
            nebulaId.Value = row.NebulaId.HasValue ? row.NebulaId.Value : DBNull.Value;
            wormholeClassId.Value = row.WormholeClassId.HasValue ? row.WormholeClassId.Value : DBNull.Value;
            await cmd.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    private static async Task InsertConstellationsAsync(Microsoft.Data.Sqlite.SqliteConnection connection, Microsoft.Data.Sqlite.SqliteTransaction transaction, IReadOnlyCollection<MapConstellation> rows, CancellationToken cancellationToken)
    {
        using var cmd = connection.CreateCommand();
        cmd.Transaction = transaction;
        cmd.CommandText = "INSERT INTO mapConstellations (constellationID, regionID, constellationName, x, y, z, factionID, wormholeClassID) VALUES ($constellationID, $regionID, $constellationName, $x, $y, $z, $factionID, $wormholeClassID);";

        var constellationId = cmd.CreateParameter(); constellationId.ParameterName = "$constellationID"; cmd.Parameters.Add(constellationId);
        var regionId = cmd.CreateParameter(); regionId.ParameterName = "$regionID"; cmd.Parameters.Add(regionId);
        var constellationName = cmd.CreateParameter(); constellationName.ParameterName = "$constellationName"; cmd.Parameters.Add(constellationName);
        var x = cmd.CreateParameter(); x.ParameterName = "$x"; cmd.Parameters.Add(x);
        var y = cmd.CreateParameter(); y.ParameterName = "$y"; cmd.Parameters.Add(y);
        var z = cmd.CreateParameter(); z.ParameterName = "$z"; cmd.Parameters.Add(z);
        var factionId = cmd.CreateParameter(); factionId.ParameterName = "$factionID"; cmd.Parameters.Add(factionId);
        var wormholeClassId = cmd.CreateParameter(); wormholeClassId.ParameterName = "$wormholeClassID"; cmd.Parameters.Add(wormholeClassId);

        foreach (var row in rows)
        {
            constellationId.Value = row.ConstellationId;
            regionId.Value = row.RegionId.HasValue ? row.RegionId.Value : DBNull.Value;
            constellationName.Value = row.ConstellationName;
            x.Value = row.X.HasValue ? row.X.Value : DBNull.Value;
            y.Value = row.Y.HasValue ? row.Y.Value : DBNull.Value;
            z.Value = row.Z.HasValue ? row.Z.Value : DBNull.Value;
            factionId.Value = row.FactionId.HasValue ? row.FactionId.Value : DBNull.Value;
            wormholeClassId.Value = row.WormholeClassId.HasValue ? row.WormholeClassId.Value : DBNull.Value;
            await cmd.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    private static async Task InsertSolarSystemsAsync(Microsoft.Data.Sqlite.SqliteConnection connection, Microsoft.Data.Sqlite.SqliteTransaction transaction, IReadOnlyCollection<MapSolarSystem> rows, CancellationToken cancellationToken)
    {
        using var cmd = connection.CreateCommand();
        cmd.Transaction = transaction;
        cmd.CommandText = "INSERT INTO mapSolarSystems (solarSystemID, regionID, constellationID, solarSystemName, security, securityClass, factionID, starID, x, y, z, radius, luminosity, border, fringe, corridor, hub, international, regional, wormholeClassID) VALUES ($solarSystemID, $regionID, $constellationID, $solarSystemName, $security, $securityClass, $factionID, $starID, $x, $y, $z, $radius, $luminosity, $border, $fringe, $corridor, $hub, $international, $regional, $wormholeClassID);";

        var solarSystemId = cmd.CreateParameter(); solarSystemId.ParameterName = "$solarSystemID"; cmd.Parameters.Add(solarSystemId);
        var regionId = cmd.CreateParameter(); regionId.ParameterName = "$regionID"; cmd.Parameters.Add(regionId);
        var constellationId = cmd.CreateParameter(); constellationId.ParameterName = "$constellationID"; cmd.Parameters.Add(constellationId);
        var solarSystemName = cmd.CreateParameter(); solarSystemName.ParameterName = "$solarSystemName"; cmd.Parameters.Add(solarSystemName);
        var security = cmd.CreateParameter(); security.ParameterName = "$security"; cmd.Parameters.Add(security);
        var securityClass = cmd.CreateParameter(); securityClass.ParameterName = "$securityClass"; cmd.Parameters.Add(securityClass);
        var factionId = cmd.CreateParameter(); factionId.ParameterName = "$factionID"; cmd.Parameters.Add(factionId);
        var starId = cmd.CreateParameter(); starId.ParameterName = "$starID"; cmd.Parameters.Add(starId);
        var x = cmd.CreateParameter(); x.ParameterName = "$x"; cmd.Parameters.Add(x);
        var y = cmd.CreateParameter(); y.ParameterName = "$y"; cmd.Parameters.Add(y);
        var z = cmd.CreateParameter(); z.ParameterName = "$z"; cmd.Parameters.Add(z);
        var radius = cmd.CreateParameter(); radius.ParameterName = "$radius"; cmd.Parameters.Add(radius);
        var luminosity = cmd.CreateParameter(); luminosity.ParameterName = "$luminosity"; cmd.Parameters.Add(luminosity);
        var border = cmd.CreateParameter(); border.ParameterName = "$border"; cmd.Parameters.Add(border);
        var fringe = cmd.CreateParameter(); fringe.ParameterName = "$fringe"; cmd.Parameters.Add(fringe);
        var corridor = cmd.CreateParameter(); corridor.ParameterName = "$corridor"; cmd.Parameters.Add(corridor);
        var hub = cmd.CreateParameter(); hub.ParameterName = "$hub"; cmd.Parameters.Add(hub);
        var international = cmd.CreateParameter(); international.ParameterName = "$international"; cmd.Parameters.Add(international);
        var regional = cmd.CreateParameter(); regional.ParameterName = "$regional"; cmd.Parameters.Add(regional);
        var wormholeClassId = cmd.CreateParameter(); wormholeClassId.ParameterName = "$wormholeClassID"; cmd.Parameters.Add(wormholeClassId);

        foreach (var row in rows)
        {
            solarSystemId.Value = row.SolarSystemId;
            regionId.Value = row.RegionId.HasValue ? row.RegionId.Value : DBNull.Value;
            constellationId.Value = row.ConstellationId.HasValue ? row.ConstellationId.Value : DBNull.Value;
            solarSystemName.Value = row.SolarSystemName;
            security.Value = row.Security.HasValue ? row.Security.Value : DBNull.Value;
            securityClass.Value = string.IsNullOrWhiteSpace(row.SecurityClass) ? DBNull.Value : row.SecurityClass;
            factionId.Value = row.FactionId.HasValue ? row.FactionId.Value : DBNull.Value;
            starId.Value = row.StarId.HasValue ? row.StarId.Value : DBNull.Value;
            x.Value = row.X.HasValue ? row.X.Value : DBNull.Value;
            y.Value = row.Y.HasValue ? row.Y.Value : DBNull.Value;
            z.Value = row.Z.HasValue ? row.Z.Value : DBNull.Value;
            radius.Value = row.Radius.HasValue ? row.Radius.Value : DBNull.Value;
            luminosity.Value = row.Luminosity.HasValue ? row.Luminosity.Value : DBNull.Value;
            border.Value = row.Border ? 1 : 0;
            fringe.Value = row.Fringe ? 1 : 0;
            corridor.Value = row.Corridor ? 1 : 0;
            hub.Value = row.Hub ? 1 : 0;
            international.Value = row.International ? 1 : 0;
            regional.Value = row.Regional ? 1 : 0;
            wormholeClassId.Value = row.WormholeClassId.HasValue ? row.WormholeClassId.Value : DBNull.Value;
            await cmd.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    private static async Task InsertJumpsAsync(Microsoft.Data.Sqlite.SqliteConnection connection, Microsoft.Data.Sqlite.SqliteTransaction transaction, IReadOnlyCollection<MapJump> rows, CancellationToken cancellationToken)
    {
        using var cmd = connection.CreateCommand();
        cmd.Transaction = transaction;
        cmd.CommandText = "INSERT INTO mapJumps (stargateID, destinationStargateID, fromRegionID, fromConstellationID, fromSolarSystemID, toRegionID, toConstellationID, toSolarSystemID) VALUES ($stargateID, $destinationStargateID, $fromRegionID, $fromConstellationID, $fromSolarSystemID, $toRegionID, $toConstellationID, $toSolarSystemID);";

        var stargateId = cmd.CreateParameter(); stargateId.ParameterName = "$stargateID"; cmd.Parameters.Add(stargateId);
        var destinationStargateId = cmd.CreateParameter(); destinationStargateId.ParameterName = "$destinationStargateID"; cmd.Parameters.Add(destinationStargateId);
        var fromRegionId = cmd.CreateParameter(); fromRegionId.ParameterName = "$fromRegionID"; cmd.Parameters.Add(fromRegionId);
        var fromConstellationId = cmd.CreateParameter(); fromConstellationId.ParameterName = "$fromConstellationID"; cmd.Parameters.Add(fromConstellationId);
        var fromSolarSystemId = cmd.CreateParameter(); fromSolarSystemId.ParameterName = "$fromSolarSystemID"; cmd.Parameters.Add(fromSolarSystemId);
        var toRegionId = cmd.CreateParameter(); toRegionId.ParameterName = "$toRegionID"; cmd.Parameters.Add(toRegionId);
        var toConstellationId = cmd.CreateParameter(); toConstellationId.ParameterName = "$toConstellationID"; cmd.Parameters.Add(toConstellationId);
        var toSolarSystemId = cmd.CreateParameter(); toSolarSystemId.ParameterName = "$toSolarSystemID"; cmd.Parameters.Add(toSolarSystemId);

        foreach (var row in rows)
        {
            stargateId.Value = row.StargateId.HasValue ? row.StargateId.Value : DBNull.Value;
            destinationStargateId.Value = row.DestinationStargateId.HasValue ? row.DestinationStargateId.Value : DBNull.Value;
            fromRegionId.Value = row.FromRegionId;
            fromConstellationId.Value = row.FromConstellationId;
            fromSolarSystemId.Value = row.FromSolarSystemId;
            toRegionId.Value = row.ToRegionId;
            toConstellationId.Value = row.ToConstellationId;
            toSolarSystemId.Value = row.ToSolarSystemId;
            await cmd.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    private static async Task InsertRegionJumpsAsync(Microsoft.Data.Sqlite.SqliteConnection connection, Microsoft.Data.Sqlite.SqliteTransaction transaction, IReadOnlyCollection<MapRegionJump> rows, CancellationToken cancellationToken)
    {
        using var cmd = connection.CreateCommand();
        cmd.Transaction = transaction;
        cmd.CommandText = "INSERT INTO mapRegionJumps (fromRegionID, toRegionID) VALUES ($fromRegionID, $toRegionID);";

        var fromRegionId = cmd.CreateParameter(); fromRegionId.ParameterName = "$fromRegionID"; cmd.Parameters.Add(fromRegionId);
        var toRegionId = cmd.CreateParameter(); toRegionId.ParameterName = "$toRegionID"; cmd.Parameters.Add(toRegionId);

        foreach (var row in rows)
        {
            fromRegionId.Value = row.FromRegionId;
            toRegionId.Value = row.ToRegionId;
            await cmd.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    private static async Task InsertConstellationJumpsAsync(Microsoft.Data.Sqlite.SqliteConnection connection, Microsoft.Data.Sqlite.SqliteTransaction transaction, IReadOnlyCollection<MapConstellationJump> rows, CancellationToken cancellationToken)
    {
        using var cmd = connection.CreateCommand();
        cmd.Transaction = transaction;
        cmd.CommandText = "INSERT INTO mapConstellationJumps (fromConstellationID, toConstellationID) VALUES ($fromConstellationID, $toConstellationID);";

        var fromConstellationId = cmd.CreateParameter(); fromConstellationId.ParameterName = "$fromConstellationID"; cmd.Parameters.Add(fromConstellationId);
        var toConstellationId = cmd.CreateParameter(); toConstellationId.ParameterName = "$toConstellationID"; cmd.Parameters.Add(toConstellationId);

        foreach (var row in rows)
        {
            fromConstellationId.Value = row.FromConstellationId;
            toConstellationId.Value = row.ToConstellationId;
            await cmd.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    private static async Task InsertSolarSystemJumpsAsync(Microsoft.Data.Sqlite.SqliteConnection connection, Microsoft.Data.Sqlite.SqliteTransaction transaction, IReadOnlyCollection<MapSolarSystemJump> rows, CancellationToken cancellationToken)
    {
        using var cmd = connection.CreateCommand();
        cmd.Transaction = transaction;
        cmd.CommandText = "INSERT INTO mapSolarSystemJumps (fromSolarSystemID, toSolarSystemID) VALUES ($fromSolarSystemID, $toSolarSystemID);";

        var fromSolarSystemId = cmd.CreateParameter(); fromSolarSystemId.ParameterName = "$fromSolarSystemID"; cmd.Parameters.Add(fromSolarSystemId);
        var toSolarSystemId = cmd.CreateParameter(); toSolarSystemId.ParameterName = "$toSolarSystemID"; cmd.Parameters.Add(toSolarSystemId);

        foreach (var row in rows)
        {
            fromSolarSystemId.Value = row.FromSolarSystemId;
            toSolarSystemId.Value = row.ToSolarSystemId;
            await cmd.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    private static async Task InsertLandmarksAsync(Microsoft.Data.Sqlite.SqliteConnection connection, Microsoft.Data.Sqlite.SqliteTransaction transaction, IReadOnlyCollection<MapLandmark> rows, CancellationToken cancellationToken)
    {
        using var cmd = connection.CreateCommand();
        cmd.Transaction = transaction;
        cmd.CommandText = "INSERT INTO mapLandmarks (landmarkID, landmarkName, description, iconID, x, y, z) VALUES ($landmarkID, $landmarkName, $description, $iconID, $x, $y, $z);";

        var landmarkId = cmd.CreateParameter(); landmarkId.ParameterName = "$landmarkID"; cmd.Parameters.Add(landmarkId);
        var landmarkName = cmd.CreateParameter(); landmarkName.ParameterName = "$landmarkName"; cmd.Parameters.Add(landmarkName);
        var description = cmd.CreateParameter(); description.ParameterName = "$description"; cmd.Parameters.Add(description);
        var iconId = cmd.CreateParameter(); iconId.ParameterName = "$iconID"; cmd.Parameters.Add(iconId);
        var x = cmd.CreateParameter(); x.ParameterName = "$x"; cmd.Parameters.Add(x);
        var y = cmd.CreateParameter(); y.ParameterName = "$y"; cmd.Parameters.Add(y);
        var z = cmd.CreateParameter(); z.ParameterName = "$z"; cmd.Parameters.Add(z);

        foreach (var row in rows)
        {
            landmarkId.Value = row.LandmarkId;
            landmarkName.Value = row.LandmarkName;
            description.Value = string.IsNullOrWhiteSpace(row.Description) ? DBNull.Value : row.Description;
            iconId.Value = row.IconId.HasValue ? row.IconId.Value : DBNull.Value;
            x.Value = row.X.HasValue ? row.X.Value : DBNull.Value;
            y.Value = row.Y.HasValue ? row.Y.Value : DBNull.Value;
            z.Value = row.Z.HasValue ? row.Z.Value : DBNull.Value;
            await cmd.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    private static async Task InsertMapDenormalizeAsync(Microsoft.Data.Sqlite.SqliteConnection connection, Microsoft.Data.Sqlite.SqliteTransaction transaction, IReadOnlyCollection<MapDenormalizeRow> rows, CancellationToken cancellationToken)
    {
        using var cmd = connection.CreateCommand();
        cmd.Transaction = transaction;
        cmd.CommandText = "INSERT INTO mapDenormalize (itemID, typeID, groupID, solarSystemID, constellationID, regionID, orbitID, x, y, z, radius, itemName, security, celestialIndex, orbitIndex) VALUES ($itemID, $typeID, $groupID, $solarSystemID, $constellationID, $regionID, $orbitID, $x, $y, $z, $radius, $itemName, $security, $celestialIndex, $orbitIndex);";

        var itemId = cmd.CreateParameter(); itemId.ParameterName = "$itemID"; cmd.Parameters.Add(itemId);
        var typeId = cmd.CreateParameter(); typeId.ParameterName = "$typeID"; cmd.Parameters.Add(typeId);
        var groupId = cmd.CreateParameter(); groupId.ParameterName = "$groupID"; cmd.Parameters.Add(groupId);
        var solarSystemId = cmd.CreateParameter(); solarSystemId.ParameterName = "$solarSystemID"; cmd.Parameters.Add(solarSystemId);
        var constellationId = cmd.CreateParameter(); constellationId.ParameterName = "$constellationID"; cmd.Parameters.Add(constellationId);
        var regionId = cmd.CreateParameter(); regionId.ParameterName = "$regionID"; cmd.Parameters.Add(regionId);
        var orbitId = cmd.CreateParameter(); orbitId.ParameterName = "$orbitID"; cmd.Parameters.Add(orbitId);
        var x = cmd.CreateParameter(); x.ParameterName = "$x"; cmd.Parameters.Add(x);
        var y = cmd.CreateParameter(); y.ParameterName = "$y"; cmd.Parameters.Add(y);
        var z = cmd.CreateParameter(); z.ParameterName = "$z"; cmd.Parameters.Add(z);
        var radius = cmd.CreateParameter(); radius.ParameterName = "$radius"; cmd.Parameters.Add(radius);
        var itemName = cmd.CreateParameter(); itemName.ParameterName = "$itemName"; cmd.Parameters.Add(itemName);
        var security = cmd.CreateParameter(); security.ParameterName = "$security"; cmd.Parameters.Add(security);
        var celestialIndex = cmd.CreateParameter(); celestialIndex.ParameterName = "$celestialIndex"; cmd.Parameters.Add(celestialIndex);
        var orbitIndex = cmd.CreateParameter(); orbitIndex.ParameterName = "$orbitIndex"; cmd.Parameters.Add(orbitIndex);

        foreach (var row in rows)
        {
            itemId.Value = row.ItemId;
            typeId.Value = row.TypeId.HasValue ? row.TypeId.Value : DBNull.Value;
            groupId.Value = row.GroupId.HasValue ? row.GroupId.Value : DBNull.Value;
            solarSystemId.Value = row.SolarSystemId.HasValue ? row.SolarSystemId.Value : DBNull.Value;
            constellationId.Value = row.ConstellationId.HasValue ? row.ConstellationId.Value : DBNull.Value;
            regionId.Value = row.RegionId.HasValue ? row.RegionId.Value : DBNull.Value;
            orbitId.Value = row.OrbitId.HasValue ? row.OrbitId.Value : DBNull.Value;
            x.Value = row.X.HasValue ? row.X.Value : DBNull.Value;
            y.Value = row.Y.HasValue ? row.Y.Value : DBNull.Value;
            z.Value = row.Z.HasValue ? row.Z.Value : DBNull.Value;
            radius.Value = row.Radius.HasValue ? row.Radius.Value : DBNull.Value;
            itemName.Value = string.IsNullOrWhiteSpace(row.ItemName) ? DBNull.Value : row.ItemName;
            security.Value = row.Security.HasValue ? row.Security.Value : DBNull.Value;
            celestialIndex.Value = row.CelestialIndex.HasValue ? row.CelestialIndex.Value : DBNull.Value;
            orbitIndex.Value = row.OrbitIndex.HasValue ? row.OrbitIndex.Value : DBNull.Value;
            await cmd.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    private static async Task InsertMapCelestialGraphicsAsync(Microsoft.Data.Sqlite.SqliteConnection connection, Microsoft.Data.Sqlite.SqliteTransaction transaction, IReadOnlyCollection<MapCelestialGraphics> rows, CancellationToken cancellationToken)
    {
        using var cmd = connection.CreateCommand();
        cmd.Transaction = transaction;
        cmd.CommandText = "INSERT INTO mapCelestialGraphics (celestialID, heightMap1, heightMap2, shaderPreset, population) VALUES ($celestialID, $heightMap1, $heightMap2, $shaderPreset, $population);";

        var celestialId = cmd.CreateParameter(); celestialId.ParameterName = "$celestialID"; cmd.Parameters.Add(celestialId);
        var heightMap1 = cmd.CreateParameter(); heightMap1.ParameterName = "$heightMap1"; cmd.Parameters.Add(heightMap1);
        var heightMap2 = cmd.CreateParameter(); heightMap2.ParameterName = "$heightMap2"; cmd.Parameters.Add(heightMap2);
        var shaderPreset = cmd.CreateParameter(); shaderPreset.ParameterName = "$shaderPreset"; cmd.Parameters.Add(shaderPreset);
        var population = cmd.CreateParameter(); population.ParameterName = "$population"; cmd.Parameters.Add(population);

        foreach (var row in rows)
        {
            celestialId.Value = row.CelestialId;
            heightMap1.Value = row.HeightMap1.HasValue ? row.HeightMap1.Value : DBNull.Value;
            heightMap2.Value = row.HeightMap2.HasValue ? row.HeightMap2.Value : DBNull.Value;
            shaderPreset.Value = row.ShaderPreset.HasValue ? row.ShaderPreset.Value : DBNull.Value;
            population.Value = row.Population.HasValue ? (row.Population.Value ? 1 : 0) : DBNull.Value;
            await cmd.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    private static async Task InsertMapCelestialStatisticsAsync(Microsoft.Data.Sqlite.SqliteConnection connection, Microsoft.Data.Sqlite.SqliteTransaction transaction, IReadOnlyCollection<MapCelestialStatistics> rows, CancellationToken cancellationToken)
    {
        using var cmd = connection.CreateCommand();
        cmd.Transaction = transaction;
        cmd.CommandText = "INSERT INTO mapCelestialStatistics (celestialID, temperature, spectralClass, luminosity, age, life, orbitRadius, eccentricity, massDust, massGas, fragmented, density, surfaceGravity, escapeVelocity, orbitPeriod, rotationRate, locked, pressure, radius, mass) VALUES ($celestialID, $temperature, $spectralClass, $luminosity, $age, $life, $orbitRadius, $eccentricity, $massDust, $massGas, $fragmented, $density, $surfaceGravity, $escapeVelocity, $orbitPeriod, $rotationRate, $locked, $pressure, $radius, $mass);";

        var celestialId = cmd.CreateParameter(); celestialId.ParameterName = "$celestialID"; cmd.Parameters.Add(celestialId);
        var temperature = cmd.CreateParameter(); temperature.ParameterName = "$temperature"; cmd.Parameters.Add(temperature);
        var spectralClass = cmd.CreateParameter(); spectralClass.ParameterName = "$spectralClass"; cmd.Parameters.Add(spectralClass);
        var luminosity = cmd.CreateParameter(); luminosity.ParameterName = "$luminosity"; cmd.Parameters.Add(luminosity);
        var age = cmd.CreateParameter(); age.ParameterName = "$age"; cmd.Parameters.Add(age);
        var life = cmd.CreateParameter(); life.ParameterName = "$life"; cmd.Parameters.Add(life);
        var orbitRadius = cmd.CreateParameter(); orbitRadius.ParameterName = "$orbitRadius"; cmd.Parameters.Add(orbitRadius);
        var eccentricity = cmd.CreateParameter(); eccentricity.ParameterName = "$eccentricity"; cmd.Parameters.Add(eccentricity);
        var massDust = cmd.CreateParameter(); massDust.ParameterName = "$massDust"; cmd.Parameters.Add(massDust);
        var massGas = cmd.CreateParameter(); massGas.ParameterName = "$massGas"; cmd.Parameters.Add(massGas);
        var fragmented = cmd.CreateParameter(); fragmented.ParameterName = "$fragmented"; cmd.Parameters.Add(fragmented);
        var density = cmd.CreateParameter(); density.ParameterName = "$density"; cmd.Parameters.Add(density);
        var surfaceGravity = cmd.CreateParameter(); surfaceGravity.ParameterName = "$surfaceGravity"; cmd.Parameters.Add(surfaceGravity);
        var escapeVelocity = cmd.CreateParameter(); escapeVelocity.ParameterName = "$escapeVelocity"; cmd.Parameters.Add(escapeVelocity);
        var orbitPeriod = cmd.CreateParameter(); orbitPeriod.ParameterName = "$orbitPeriod"; cmd.Parameters.Add(orbitPeriod);
        var rotationRate = cmd.CreateParameter(); rotationRate.ParameterName = "$rotationRate"; cmd.Parameters.Add(rotationRate);
        var locked = cmd.CreateParameter(); locked.ParameterName = "$locked"; cmd.Parameters.Add(locked);
        var pressure = cmd.CreateParameter(); pressure.ParameterName = "$pressure"; cmd.Parameters.Add(pressure);
        var radius = cmd.CreateParameter(); radius.ParameterName = "$radius"; cmd.Parameters.Add(radius);
        var mass = cmd.CreateParameter(); mass.ParameterName = "$mass"; cmd.Parameters.Add(mass);

        foreach (var row in rows)
        {
            celestialId.Value = row.CelestialId;
            temperature.Value = row.Temperature.HasValue ? row.Temperature.Value : DBNull.Value;
            spectralClass.Value = string.IsNullOrWhiteSpace(row.SpectralClass) ? DBNull.Value : row.SpectralClass;
            luminosity.Value = row.Luminosity.HasValue ? row.Luminosity.Value : DBNull.Value;
            age.Value = row.Age.HasValue ? row.Age.Value : DBNull.Value;
            life.Value = row.Life.HasValue ? row.Life.Value : DBNull.Value;
            orbitRadius.Value = row.OrbitRadius.HasValue ? row.OrbitRadius.Value : DBNull.Value;
            eccentricity.Value = row.Eccentricity.HasValue ? row.Eccentricity.Value : DBNull.Value;
            massDust.Value = row.MassDust.HasValue ? row.MassDust.Value : DBNull.Value;
            massGas.Value = row.MassGas.HasValue ? row.MassGas.Value : DBNull.Value;
            fragmented.Value = row.Fragmented.HasValue ? (row.Fragmented.Value ? 1 : 0) : DBNull.Value;
            density.Value = row.Density.HasValue ? row.Density.Value : DBNull.Value;
            surfaceGravity.Value = row.SurfaceGravity.HasValue ? row.SurfaceGravity.Value : DBNull.Value;
            escapeVelocity.Value = row.EscapeVelocity.HasValue ? row.EscapeVelocity.Value : DBNull.Value;
            orbitPeriod.Value = row.OrbitPeriod.HasValue ? row.OrbitPeriod.Value : DBNull.Value;
            rotationRate.Value = row.RotationRate.HasValue ? row.RotationRate.Value : DBNull.Value;
            locked.Value = row.Locked.HasValue ? (row.Locked.Value ? 1 : 0) : DBNull.Value;
            pressure.Value = row.Pressure.HasValue ? row.Pressure.Value : DBNull.Value;
            radius.Value = row.Radius.HasValue ? row.Radius.Value : DBNull.Value;
            mass.Value = row.Mass.HasValue ? row.Mass.Value : DBNull.Value;
            await cmd.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    private static async Task InsertMapUniverseAsync(Microsoft.Data.Sqlite.SqliteConnection connection, Microsoft.Data.Sqlite.SqliteTransaction transaction, IReadOnlyCollection<MapUniverse> rows, CancellationToken cancellationToken)
    {
        using var cmd = connection.CreateCommand();
        cmd.Transaction = transaction;
        cmd.CommandText = "INSERT INTO mapUniverse (universeID, universeName, x, y, z, xMin, xMax, yMin, yMax, zMin, zMax, radius) VALUES ($universeID, $universeName, $x, $y, $z, $xMin, $xMax, $yMin, $yMax, $zMin, $zMax, $radius);";

        var universeId = cmd.CreateParameter(); universeId.ParameterName = "$universeID"; cmd.Parameters.Add(universeId);
        var universeName = cmd.CreateParameter(); universeName.ParameterName = "$universeName"; cmd.Parameters.Add(universeName);
        var x = cmd.CreateParameter(); x.ParameterName = "$x"; cmd.Parameters.Add(x);
        var y = cmd.CreateParameter(); y.ParameterName = "$y"; cmd.Parameters.Add(y);
        var z = cmd.CreateParameter(); z.ParameterName = "$z"; cmd.Parameters.Add(z);
        var xMin = cmd.CreateParameter(); xMin.ParameterName = "$xMin"; cmd.Parameters.Add(xMin);
        var xMax = cmd.CreateParameter(); xMax.ParameterName = "$xMax"; cmd.Parameters.Add(xMax);
        var yMin = cmd.CreateParameter(); yMin.ParameterName = "$yMin"; cmd.Parameters.Add(yMin);
        var yMax = cmd.CreateParameter(); yMax.ParameterName = "$yMax"; cmd.Parameters.Add(yMax);
        var zMin = cmd.CreateParameter(); zMin.ParameterName = "$zMin"; cmd.Parameters.Add(zMin);
        var zMax = cmd.CreateParameter(); zMax.ParameterName = "$zMax"; cmd.Parameters.Add(zMax);
        var radius = cmd.CreateParameter(); radius.ParameterName = "$radius"; cmd.Parameters.Add(radius);

        foreach (var row in rows)
        {
            universeId.Value = row.UniverseId;
            universeName.Value = row.UniverseName;
            x.Value = row.X.HasValue ? row.X.Value : DBNull.Value;
            y.Value = row.Y.HasValue ? row.Y.Value : DBNull.Value;
            z.Value = row.Z.HasValue ? row.Z.Value : DBNull.Value;
            xMin.Value = row.XMin.HasValue ? row.XMin.Value : DBNull.Value;
            xMax.Value = row.XMax.HasValue ? row.XMax.Value : DBNull.Value;
            yMin.Value = row.YMin.HasValue ? row.YMin.Value : DBNull.Value;
            yMax.Value = row.YMax.HasValue ? row.YMax.Value : DBNull.Value;
            zMin.Value = row.ZMin.HasValue ? row.ZMin.Value : DBNull.Value;
            zMax.Value = row.ZMax.HasValue ? row.ZMax.Value : DBNull.Value;
            radius.Value = row.Radius.HasValue ? row.Radius.Value : DBNull.Value;
            await cmd.ExecuteNonQueryAsync(cancellationToken);
        }
    }
}
