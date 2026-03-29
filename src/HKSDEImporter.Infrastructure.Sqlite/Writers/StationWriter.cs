using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Infrastructure.Sqlite.Connections;

namespace HKSDEImporter.Infrastructure.Sqlite.Writers;

public sealed class StationWriter : IStationWriter
{
    private readonly SqliteConnectionFactory _connectionFactory;

    public StationWriter(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task WriteAsync(
        string outputPath,
        IReadOnlyCollection<StationOperation> operations,
        IReadOnlyCollection<StationOperationService> operationServices,
        IReadOnlyCollection<StationService> services,
        IReadOnlyCollection<Station> stations,
        CancellationToken cancellationToken)
    {
        await using var connection = _connectionFactory.Create(Path.GetFullPath(outputPath));
        await connection.OpenAsync(cancellationToken);
        using var transaction = connection.BeginTransaction();

        await InsertOperationsAsync(connection, transaction, operations, cancellationToken);
        await InsertServicesAsync(connection, transaction, services, cancellationToken);
        await InsertOperationServicesAsync(connection, transaction, operationServices, cancellationToken);
        await InsertStationsAsync(connection, transaction, stations, cancellationToken);

        transaction.Commit();
    }

    private static async Task InsertOperationsAsync(Microsoft.Data.Sqlite.SqliteConnection connection, Microsoft.Data.Sqlite.SqliteTransaction transaction, IReadOnlyCollection<StationOperation> rows, CancellationToken cancellationToken)
    {
        using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = "INSERT INTO staOperations (activityID, operationID, operationName, description, fringe, corridor, hub, border, ratio, caldariStationTypeID, minmatarStationTypeID, amarrStationTypeID, gallenteStationTypeID, joveStationTypeID) VALUES ($activityID, $operationID, $operationName, $description, $fringe, $corridor, $hub, $border, $ratio, $caldariStationTypeID, $minmatarStationTypeID, $amarrStationTypeID, $gallenteStationTypeID, $joveStationTypeID);";

        var activityId = command.CreateParameter(); activityId.ParameterName = "$activityID"; command.Parameters.Add(activityId);
        var operationId = command.CreateParameter(); operationId.ParameterName = "$operationID"; command.Parameters.Add(operationId);
        var operationName = command.CreateParameter(); operationName.ParameterName = "$operationName"; command.Parameters.Add(operationName);
        var description = command.CreateParameter(); description.ParameterName = "$description"; command.Parameters.Add(description);
        var fringe = command.CreateParameter(); fringe.ParameterName = "$fringe"; command.Parameters.Add(fringe);
        var corridor = command.CreateParameter(); corridor.ParameterName = "$corridor"; command.Parameters.Add(corridor);
        var hub = command.CreateParameter(); hub.ParameterName = "$hub"; command.Parameters.Add(hub);
        var border = command.CreateParameter(); border.ParameterName = "$border"; command.Parameters.Add(border);
        var ratio = command.CreateParameter(); ratio.ParameterName = "$ratio"; command.Parameters.Add(ratio);
        var caldariStationTypeId = command.CreateParameter(); caldariStationTypeId.ParameterName = "$caldariStationTypeID"; command.Parameters.Add(caldariStationTypeId);
        var minmatarStationTypeId = command.CreateParameter(); minmatarStationTypeId.ParameterName = "$minmatarStationTypeID"; command.Parameters.Add(minmatarStationTypeId);
        var amarrStationTypeId = command.CreateParameter(); amarrStationTypeId.ParameterName = "$amarrStationTypeID"; command.Parameters.Add(amarrStationTypeId);
        var gallenteStationTypeId = command.CreateParameter(); gallenteStationTypeId.ParameterName = "$gallenteStationTypeID"; command.Parameters.Add(gallenteStationTypeId);
        var joveStationTypeId = command.CreateParameter(); joveStationTypeId.ParameterName = "$joveStationTypeID"; command.Parameters.Add(joveStationTypeId);

        foreach (var row in rows)
        {
            activityId.Value = row.ActivityId.HasValue ? row.ActivityId.Value : DBNull.Value;
            operationId.Value = row.OperationId;
            operationName.Value = string.IsNullOrWhiteSpace(row.OperationName) ? DBNull.Value : row.OperationName;
            description.Value = string.IsNullOrWhiteSpace(row.Description) ? DBNull.Value : row.Description;
            fringe.Value = row.Fringe.HasValue ? row.Fringe.Value : DBNull.Value;
            corridor.Value = row.Corridor.HasValue ? row.Corridor.Value : DBNull.Value;
            hub.Value = row.Hub.HasValue ? row.Hub.Value : DBNull.Value;
            border.Value = row.Border.HasValue ? row.Border.Value : DBNull.Value;
            ratio.Value = row.Ratio.HasValue ? row.Ratio.Value : DBNull.Value;
            caldariStationTypeId.Value = row.CaldariStationTypeId.HasValue ? row.CaldariStationTypeId.Value : DBNull.Value;
            minmatarStationTypeId.Value = row.MinmatarStationTypeId.HasValue ? row.MinmatarStationTypeId.Value : DBNull.Value;
            amarrStationTypeId.Value = row.AmarrStationTypeId.HasValue ? row.AmarrStationTypeId.Value : DBNull.Value;
            gallenteStationTypeId.Value = row.GallenteStationTypeId.HasValue ? row.GallenteStationTypeId.Value : DBNull.Value;
            joveStationTypeId.Value = row.JoveStationTypeId.HasValue ? row.JoveStationTypeId.Value : DBNull.Value;
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    private static async Task InsertServicesAsync(Microsoft.Data.Sqlite.SqliteConnection connection, Microsoft.Data.Sqlite.SqliteTransaction transaction, IReadOnlyCollection<StationService> rows, CancellationToken cancellationToken)
    {
        using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = "INSERT INTO staServices (serviceID, serviceName, description) VALUES ($serviceID, $serviceName, $description);";

        var serviceId = command.CreateParameter(); serviceId.ParameterName = "$serviceID"; command.Parameters.Add(serviceId);
        var serviceName = command.CreateParameter(); serviceName.ParameterName = "$serviceName"; command.Parameters.Add(serviceName);
        var description = command.CreateParameter(); description.ParameterName = "$description"; command.Parameters.Add(description);

        foreach (var row in rows)
        {
            serviceId.Value = row.ServiceId;
            serviceName.Value = string.IsNullOrWhiteSpace(row.ServiceName) ? DBNull.Value : row.ServiceName;
            description.Value = string.IsNullOrWhiteSpace(row.Description) ? DBNull.Value : row.Description;
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    private static async Task InsertOperationServicesAsync(Microsoft.Data.Sqlite.SqliteConnection connection, Microsoft.Data.Sqlite.SqliteTransaction transaction, IReadOnlyCollection<StationOperationService> rows, CancellationToken cancellationToken)
    {
        using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = "INSERT INTO staOperationServices (operationID, serviceID) VALUES ($operationID, $serviceID);";

        var operationId = command.CreateParameter(); operationId.ParameterName = "$operationID"; command.Parameters.Add(operationId);
        var serviceId = command.CreateParameter(); serviceId.ParameterName = "$serviceID"; command.Parameters.Add(serviceId);

        foreach (var row in rows)
        {
            operationId.Value = row.OperationId;
            serviceId.Value = row.ServiceId;
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    private static async Task InsertStationsAsync(Microsoft.Data.Sqlite.SqliteConnection connection, Microsoft.Data.Sqlite.SqliteTransaction transaction, IReadOnlyCollection<Station> rows, CancellationToken cancellationToken)
    {
        using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = "INSERT INTO staStations (stationID, security, dockingCostPerVolume, maxShipVolumeDockable, officeRentalCost, operationID, stationTypeID, corporationID, solarSystemID, constellationID, regionID, stationName, x, y, z, reprocessingEfficiency, reprocessingStationsTake, reprocessingHangarFlag) VALUES ($stationID, $security, $dockingCostPerVolume, $maxShipVolumeDockable, $officeRentalCost, $operationID, $stationTypeID, $corporationID, $solarSystemID, $constellationID, $regionID, $stationName, $x, $y, $z, $reprocessingEfficiency, $reprocessingStationsTake, $reprocessingHangarFlag);";

        var stationId = command.CreateParameter(); stationId.ParameterName = "$stationID"; command.Parameters.Add(stationId);
        var security = command.CreateParameter(); security.ParameterName = "$security"; command.Parameters.Add(security);
        var dockingCostPerVolume = command.CreateParameter(); dockingCostPerVolume.ParameterName = "$dockingCostPerVolume"; command.Parameters.Add(dockingCostPerVolume);
        var maxShipVolumeDockable = command.CreateParameter(); maxShipVolumeDockable.ParameterName = "$maxShipVolumeDockable"; command.Parameters.Add(maxShipVolumeDockable);
        var officeRentalCost = command.CreateParameter(); officeRentalCost.ParameterName = "$officeRentalCost"; command.Parameters.Add(officeRentalCost);
        var operationId = command.CreateParameter(); operationId.ParameterName = "$operationID"; command.Parameters.Add(operationId);
        var stationTypeId = command.CreateParameter(); stationTypeId.ParameterName = "$stationTypeID"; command.Parameters.Add(stationTypeId);
        var corporationId = command.CreateParameter(); corporationId.ParameterName = "$corporationID"; command.Parameters.Add(corporationId);
        var solarSystemId = command.CreateParameter(); solarSystemId.ParameterName = "$solarSystemID"; command.Parameters.Add(solarSystemId);
        var constellationId = command.CreateParameter(); constellationId.ParameterName = "$constellationID"; command.Parameters.Add(constellationId);
        var regionId = command.CreateParameter(); regionId.ParameterName = "$regionID"; command.Parameters.Add(regionId);
        var stationName = command.CreateParameter(); stationName.ParameterName = "$stationName"; command.Parameters.Add(stationName);
        var x = command.CreateParameter(); x.ParameterName = "$x"; command.Parameters.Add(x);
        var y = command.CreateParameter(); y.ParameterName = "$y"; command.Parameters.Add(y);
        var z = command.CreateParameter(); z.ParameterName = "$z"; command.Parameters.Add(z);
        var reprocessingEfficiency = command.CreateParameter(); reprocessingEfficiency.ParameterName = "$reprocessingEfficiency"; command.Parameters.Add(reprocessingEfficiency);
        var reprocessingStationsTake = command.CreateParameter(); reprocessingStationsTake.ParameterName = "$reprocessingStationsTake"; command.Parameters.Add(reprocessingStationsTake);
        var reprocessingHangarFlag = command.CreateParameter(); reprocessingHangarFlag.ParameterName = "$reprocessingHangarFlag"; command.Parameters.Add(reprocessingHangarFlag);

        foreach (var row in rows)
        {
            stationId.Value = row.StationId;
            security.Value = row.Security.HasValue ? row.Security.Value : DBNull.Value;
            dockingCostPerVolume.Value = row.DockingCostPerVolume.HasValue ? row.DockingCostPerVolume.Value : DBNull.Value;
            maxShipVolumeDockable.Value = row.MaxShipVolumeDockable.HasValue ? row.MaxShipVolumeDockable.Value : DBNull.Value;
            officeRentalCost.Value = row.OfficeRentalCost.HasValue ? row.OfficeRentalCost.Value : DBNull.Value;
            operationId.Value = row.OperationId.HasValue ? row.OperationId.Value : DBNull.Value;
            stationTypeId.Value = row.StationTypeId.HasValue ? row.StationTypeId.Value : DBNull.Value;
            corporationId.Value = row.CorporationId.HasValue ? row.CorporationId.Value : DBNull.Value;
            solarSystemId.Value = row.SolarSystemId.HasValue ? row.SolarSystemId.Value : DBNull.Value;
            constellationId.Value = row.ConstellationId.HasValue ? row.ConstellationId.Value : DBNull.Value;
            regionId.Value = row.RegionId.HasValue ? row.RegionId.Value : DBNull.Value;
            stationName.Value = string.IsNullOrWhiteSpace(row.StationName) ? DBNull.Value : row.StationName;
            x.Value = row.X.HasValue ? row.X.Value : DBNull.Value;
            y.Value = row.Y.HasValue ? row.Y.Value : DBNull.Value;
            z.Value = row.Z.HasValue ? row.Z.Value : DBNull.Value;
            reprocessingEfficiency.Value = row.ReprocessingEfficiency.HasValue ? row.ReprocessingEfficiency.Value : DBNull.Value;
            reprocessingStationsTake.Value = row.ReprocessingStationsTake.HasValue ? row.ReprocessingStationsTake.Value : DBNull.Value;
            reprocessingHangarFlag.Value = row.ReprocessingHangarFlag.HasValue ? row.ReprocessingHangarFlag.Value : DBNull.Value;
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }
}
