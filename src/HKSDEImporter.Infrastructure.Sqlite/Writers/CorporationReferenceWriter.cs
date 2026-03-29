using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Infrastructure.Sqlite.Connections;

namespace HKSDEImporter.Infrastructure.Sqlite.Writers;

public sealed class CorporationReferenceWriter : ICorporationReferenceWriter
{
    private readonly SqliteConnectionFactory _connectionFactory;

    public CorporationReferenceWriter(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task WriteAsync(
        string outputPath,
        IReadOnlyCollection<CorporationActivity> activities,
        IReadOnlyCollection<NpcCorporationDivision> divisions,
        IReadOnlyCollection<NpcCorporation> corporations,
        CancellationToken cancellationToken)
    {
        await using var connection = _connectionFactory.Create(Path.GetFullPath(outputPath));
        await connection.OpenAsync(cancellationToken);
        using var transaction = connection.BeginTransaction();

        await InsertActivitiesAsync(connection, transaction, activities, cancellationToken);
        await InsertDivisionsAsync(connection, transaction, divisions, cancellationToken);
        await InsertCorporationsAsync(connection, transaction, corporations, cancellationToken);

        transaction.Commit();
    }

    private static async Task InsertActivitiesAsync(Microsoft.Data.Sqlite.SqliteConnection connection, Microsoft.Data.Sqlite.SqliteTransaction transaction, IReadOnlyCollection<CorporationActivity> rows, CancellationToken cancellationToken)
    {
        using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = "INSERT INTO crpActivities (activityID, activityName, description) VALUES ($activityID, $activityName, $description);";

        var activityId = command.CreateParameter(); activityId.ParameterName = "$activityID"; command.Parameters.Add(activityId);
        var activityName = command.CreateParameter(); activityName.ParameterName = "$activityName"; command.Parameters.Add(activityName);
        var description = command.CreateParameter(); description.ParameterName = "$description"; command.Parameters.Add(description);

        foreach (var row in rows)
        {
            activityId.Value = row.ActivityId;
            activityName.Value = string.IsNullOrWhiteSpace(row.ActivityName) ? DBNull.Value : row.ActivityName;
            description.Value = string.IsNullOrWhiteSpace(row.Description) ? DBNull.Value : row.Description;
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    private static async Task InsertDivisionsAsync(Microsoft.Data.Sqlite.SqliteConnection connection, Microsoft.Data.Sqlite.SqliteTransaction transaction, IReadOnlyCollection<NpcCorporationDivision> rows, CancellationToken cancellationToken)
    {
        using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = "INSERT INTO crpNPCDivisions (divisionID, divisionName, description, leaderType) VALUES ($divisionID, $divisionName, $description, $leaderType);";

        var divisionId = command.CreateParameter(); divisionId.ParameterName = "$divisionID"; command.Parameters.Add(divisionId);
        var divisionName = command.CreateParameter(); divisionName.ParameterName = "$divisionName"; command.Parameters.Add(divisionName);
        var description = command.CreateParameter(); description.ParameterName = "$description"; command.Parameters.Add(description);
        var leaderType = command.CreateParameter(); leaderType.ParameterName = "$leaderType"; command.Parameters.Add(leaderType);

        foreach (var row in rows)
        {
            divisionId.Value = row.DivisionId;
            divisionName.Value = string.IsNullOrWhiteSpace(row.DivisionName) ? DBNull.Value : row.DivisionName;
            description.Value = string.IsNullOrWhiteSpace(row.Description) ? DBNull.Value : row.Description;
            leaderType.Value = string.IsNullOrWhiteSpace(row.LeaderType) ? DBNull.Value : row.LeaderType;
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    private static async Task InsertCorporationsAsync(Microsoft.Data.Sqlite.SqliteConnection connection, Microsoft.Data.Sqlite.SqliteTransaction transaction, IReadOnlyCollection<NpcCorporation> rows, CancellationToken cancellationToken)
    {
        using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = "INSERT INTO crpNPCCorporations (corporationID, size, extent, solarSystemID, investorID1, investorShares1, investorID2, investorShares2, investorID3, investorShares3, investorID4, investorShares4, friendID, enemyID, publicShares, initialPrice, minSecurity, scattered, fringe, corridor, hub, border, factionID, sizeFactor, stationCount, stationSystemCount, description, iconID) VALUES ($corporationID, $size, $extent, $solarSystemID, $investorID1, $investorShares1, $investorID2, $investorShares2, $investorID3, $investorShares3, $investorID4, $investorShares4, $friendID, $enemyID, $publicShares, $initialPrice, $minSecurity, $scattered, $fringe, $corridor, $hub, $border, $factionID, $sizeFactor, $stationCount, $stationSystemCount, $description, $iconID);";

        var corporationId = command.CreateParameter(); corporationId.ParameterName = "$corporationID"; command.Parameters.Add(corporationId);
        var size = command.CreateParameter(); size.ParameterName = "$size"; command.Parameters.Add(size);
        var extent = command.CreateParameter(); extent.ParameterName = "$extent"; command.Parameters.Add(extent);
        var solarSystemId = command.CreateParameter(); solarSystemId.ParameterName = "$solarSystemID"; command.Parameters.Add(solarSystemId);
        var investorId1 = command.CreateParameter(); investorId1.ParameterName = "$investorID1"; command.Parameters.Add(investorId1);
        var investorShares1 = command.CreateParameter(); investorShares1.ParameterName = "$investorShares1"; command.Parameters.Add(investorShares1);
        var investorId2 = command.CreateParameter(); investorId2.ParameterName = "$investorID2"; command.Parameters.Add(investorId2);
        var investorShares2 = command.CreateParameter(); investorShares2.ParameterName = "$investorShares2"; command.Parameters.Add(investorShares2);
        var investorId3 = command.CreateParameter(); investorId3.ParameterName = "$investorID3"; command.Parameters.Add(investorId3);
        var investorShares3 = command.CreateParameter(); investorShares3.ParameterName = "$investorShares3"; command.Parameters.Add(investorShares3);
        var investorId4 = command.CreateParameter(); investorId4.ParameterName = "$investorID4"; command.Parameters.Add(investorId4);
        var investorShares4 = command.CreateParameter(); investorShares4.ParameterName = "$investorShares4"; command.Parameters.Add(investorShares4);
        var friendId = command.CreateParameter(); friendId.ParameterName = "$friendID"; command.Parameters.Add(friendId);
        var enemyId = command.CreateParameter(); enemyId.ParameterName = "$enemyID"; command.Parameters.Add(enemyId);
        var publicShares = command.CreateParameter(); publicShares.ParameterName = "$publicShares"; command.Parameters.Add(publicShares);
        var initialPrice = command.CreateParameter(); initialPrice.ParameterName = "$initialPrice"; command.Parameters.Add(initialPrice);
        var minSecurity = command.CreateParameter(); minSecurity.ParameterName = "$minSecurity"; command.Parameters.Add(minSecurity);
        var scattered = command.CreateParameter(); scattered.ParameterName = "$scattered"; command.Parameters.Add(scattered);
        var fringe = command.CreateParameter(); fringe.ParameterName = "$fringe"; command.Parameters.Add(fringe);
        var corridor = command.CreateParameter(); corridor.ParameterName = "$corridor"; command.Parameters.Add(corridor);
        var hub = command.CreateParameter(); hub.ParameterName = "$hub"; command.Parameters.Add(hub);
        var border = command.CreateParameter(); border.ParameterName = "$border"; command.Parameters.Add(border);
        var factionId = command.CreateParameter(); factionId.ParameterName = "$factionID"; command.Parameters.Add(factionId);
        var sizeFactor = command.CreateParameter(); sizeFactor.ParameterName = "$sizeFactor"; command.Parameters.Add(sizeFactor);
        var stationCount = command.CreateParameter(); stationCount.ParameterName = "$stationCount"; command.Parameters.Add(stationCount);
        var stationSystemCount = command.CreateParameter(); stationSystemCount.ParameterName = "$stationSystemCount"; command.Parameters.Add(stationSystemCount);
        var description = command.CreateParameter(); description.ParameterName = "$description"; command.Parameters.Add(description);
        var iconId = command.CreateParameter(); iconId.ParameterName = "$iconID"; command.Parameters.Add(iconId);

        foreach (var row in rows)
        {
            corporationId.Value = row.CorporationId;
            size.Value = string.IsNullOrWhiteSpace(row.Size) ? DBNull.Value : row.Size;
            extent.Value = string.IsNullOrWhiteSpace(row.Extent) ? DBNull.Value : row.Extent;
            solarSystemId.Value = row.SolarSystemId.HasValue ? row.SolarSystemId.Value : DBNull.Value;
            investorId1.Value = row.InvestorId1.HasValue ? row.InvestorId1.Value : DBNull.Value;
            investorShares1.Value = row.InvestorShares1.HasValue ? row.InvestorShares1.Value : DBNull.Value;
            investorId2.Value = row.InvestorId2.HasValue ? row.InvestorId2.Value : DBNull.Value;
            investorShares2.Value = row.InvestorShares2.HasValue ? row.InvestorShares2.Value : DBNull.Value;
            investorId3.Value = row.InvestorId3.HasValue ? row.InvestorId3.Value : DBNull.Value;
            investorShares3.Value = row.InvestorShares3.HasValue ? row.InvestorShares3.Value : DBNull.Value;
            investorId4.Value = row.InvestorId4.HasValue ? row.InvestorId4.Value : DBNull.Value;
            investorShares4.Value = row.InvestorShares4.HasValue ? row.InvestorShares4.Value : DBNull.Value;
            friendId.Value = row.FriendId.HasValue ? row.FriendId.Value : DBNull.Value;
            enemyId.Value = row.EnemyId.HasValue ? row.EnemyId.Value : DBNull.Value;
            publicShares.Value = row.PublicShares.HasValue ? row.PublicShares.Value : DBNull.Value;
            initialPrice.Value = row.InitialPrice.HasValue ? row.InitialPrice.Value : DBNull.Value;
            minSecurity.Value = row.MinSecurity.HasValue ? row.MinSecurity.Value : DBNull.Value;
            scattered.Value = row.Scattered.HasValue ? row.Scattered.Value : DBNull.Value;
            fringe.Value = row.Fringe.HasValue ? row.Fringe.Value : DBNull.Value;
            corridor.Value = row.Corridor.HasValue ? row.Corridor.Value : DBNull.Value;
            hub.Value = row.Hub.HasValue ? row.Hub.Value : DBNull.Value;
            border.Value = row.Border.HasValue ? row.Border.Value : DBNull.Value;
            factionId.Value = row.FactionId.HasValue ? row.FactionId.Value : DBNull.Value;
            sizeFactor.Value = row.SizeFactor.HasValue ? row.SizeFactor.Value : DBNull.Value;
            stationCount.Value = row.StationCount.HasValue ? row.StationCount.Value : DBNull.Value;
            stationSystemCount.Value = row.StationSystemCount.HasValue ? row.StationSystemCount.Value : DBNull.Value;
            description.Value = string.IsNullOrWhiteSpace(row.Description) ? DBNull.Value : row.Description;
            iconId.Value = row.IconId.HasValue ? row.IconId.Value : DBNull.Value;
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }
}
