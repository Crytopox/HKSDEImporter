using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Infrastructure.Sqlite.Connections;

namespace HKSDEImporter.Infrastructure.Sqlite.Writers;

public sealed class CompatibilityWriter : ICompatibilityWriter
{
    private readonly SqliteConnectionFactory _connectionFactory;

    public CompatibilityWriter(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task WriteAsync(
        string outputPath,
        IReadOnlyCollection<MetaType> metaTypes,
        IReadOnlyCollection<NpcCorporationDivisionLink> corporationDivisionLinks,
        CancellationToken cancellationToken)
    {
        await using var connection = _connectionFactory.Create(Path.GetFullPath(outputPath));
        await connection.OpenAsync(cancellationToken);
        using var transaction = connection.BeginTransaction();

        await InsertMetaTypesAsync(connection, transaction, metaTypes, cancellationToken);
        await InsertNpcCorporationDivisionsAsync(connection, transaction, corporationDivisionLinks, cancellationToken);
        await BuildDerivedTablesAsync(connection, transaction, cancellationToken);

        transaction.Commit();
    }

    private static async Task InsertMetaTypesAsync(Microsoft.Data.Sqlite.SqliteConnection connection, Microsoft.Data.Sqlite.SqliteTransaction transaction, IReadOnlyCollection<MetaType> rows, CancellationToken cancellationToken)
    {
        using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = "INSERT OR REPLACE INTO invMetaTypes (typeID, parentTypeID, metaGroupID) VALUES ($typeId, $parentTypeId, $metaGroupId);";
        var typeId = command.CreateParameter(); typeId.ParameterName = "$typeId"; command.Parameters.Add(typeId);
        var parentTypeId = command.CreateParameter(); parentTypeId.ParameterName = "$parentTypeId"; command.Parameters.Add(parentTypeId);
        var metaGroupId = command.CreateParameter(); metaGroupId.ParameterName = "$metaGroupId"; command.Parameters.Add(metaGroupId);

        foreach (var row in rows)
        {
            typeId.Value = row.TypeId;
            parentTypeId.Value = row.ParentTypeId.HasValue ? row.ParentTypeId.Value : DBNull.Value;
            metaGroupId.Value = row.MetaGroupId.HasValue ? row.MetaGroupId.Value : DBNull.Value;
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    private static async Task InsertNpcCorporationDivisionsAsync(Microsoft.Data.Sqlite.SqliteConnection connection, Microsoft.Data.Sqlite.SqliteTransaction transaction, IReadOnlyCollection<NpcCorporationDivisionLink> rows, CancellationToken cancellationToken)
    {
        using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = "INSERT OR REPLACE INTO crpNPCCorporationDivisions (corporationID, divisionID, size) VALUES ($corporationId, $divisionId, $size);";
        var corporationId = command.CreateParameter(); corporationId.ParameterName = "$corporationId"; command.Parameters.Add(corporationId);
        var divisionId = command.CreateParameter(); divisionId.ParameterName = "$divisionId"; command.Parameters.Add(divisionId);
        var size = command.CreateParameter(); size.ParameterName = "$size"; command.Parameters.Add(size);

        foreach (var row in rows)
        {
            corporationId.Value = row.CorporationId;
            divisionId.Value = row.DivisionId;
            size.Value = row.Size.HasValue ? row.Size.Value : DBNull.Value;
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    private static async Task BuildDerivedTablesAsync(Microsoft.Data.Sqlite.SqliteConnection connection, Microsoft.Data.Sqlite.SqliteTransaction transaction, CancellationToken cancellationToken)
    {
        var sql = new[]
        {
            "INSERT OR REPLACE INTO invNames (itemID, itemName) SELECT itemID, itemName FROM mapDenormalize WHERE itemName IS NOT NULL AND TRIM(itemName) <> '';",
            "INSERT OR REPLACE INTO invUniqueNames (itemID, itemName, groupID) SELECT itemID, itemName, groupID FROM mapDenormalize WHERE itemName IS NOT NULL AND TRIM(itemName) <> '' AND groupID IS NOT NULL;",
            "INSERT OR REPLACE INTO invPositions (itemID, x, y, z, yaw, pitch, roll) SELECT itemID, x, y, z, NULL, NULL, NULL FROM mapDenormalize WHERE x IS NOT NULL AND y IS NOT NULL AND z IS NOT NULL;",
            "INSERT OR REPLACE INTO invVolumes (typeID, volume) SELECT typeID, CAST(volume AS INTEGER) FROM invTypes WHERE volume IS NOT NULL AND volume > 0;",
            "INSERT OR REPLACE INTO mapLocationScenes (locationID, graphicID) SELECT regionID, nebulaID FROM mapRegions WHERE nebulaID IS NOT NULL;",
            "INSERT OR REPLACE INTO mapLocationWormholeClasses (locationID, wormholeClassID) SELECT regionID, wormholeClassID FROM mapRegions WHERE wormholeClassID IS NOT NULL;",
            "INSERT OR REPLACE INTO mapLocationWormholeClasses (locationID, wormholeClassID) SELECT constellationID, wormholeClassID FROM mapConstellations WHERE wormholeClassID IS NOT NULL;",
            "INSERT OR REPLACE INTO mapLocationWormholeClasses (locationID, wormholeClassID) SELECT solarSystemID, wormholeClassID FROM mapSolarSystems WHERE wormholeClassID IS NOT NULL;",
            "INSERT OR REPLACE INTO invTypeReactions (reactionTypeID, input, typeID, quantity) SELECT typeID, 1, materialTypeID, quantity FROM industryActivityMaterials WHERE activityID = 11;",
            "INSERT OR REPLACE INTO invTypeReactions (reactionTypeID, input, typeID, quantity) SELECT typeID, 0, productTypeID, quantity FROM industryActivityProducts WHERE activityID = 11;"
        };

        foreach (var statement in sql)
        {
            using var command = connection.CreateCommand();
            command.Transaction = transaction;
            command.CommandText = statement;
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }
}
