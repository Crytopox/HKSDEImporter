using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Infrastructure.Sqlite.Connections;

namespace HKSDEImporter.Infrastructure.Sqlite.Writers;

public sealed class NpcAgentWriter : INpcAgentWriter
{
    private readonly SqliteConnectionFactory _connectionFactory;

    public NpcAgentWriter(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task WriteAsync(
        string outputPath,
        IReadOnlyCollection<Agent> agents,
        IReadOnlyCollection<ResearchAgent> researchAgents,
        IReadOnlyCollection<NpcCorporationResearchField> researchFields,
        IReadOnlyCollection<NpcCorporationTrade> corporationTrades,
        CancellationToken cancellationToken)
    {
        await using var connection = _connectionFactory.Create(Path.GetFullPath(outputPath));
        await connection.OpenAsync(cancellationToken);
        using var transaction = connection.BeginTransaction();

        await InsertAgentsAsync(connection, transaction, agents, cancellationToken);
        await InsertResearchAgentsAsync(connection, transaction, researchAgents, cancellationToken);
        await InsertResearchFieldsAsync(connection, transaction, researchFields, cancellationToken);
        await InsertCorporationTradesAsync(connection, transaction, corporationTrades, cancellationToken);

        transaction.Commit();
    }

    private static async Task InsertAgentsAsync(Microsoft.Data.Sqlite.SqliteConnection connection, Microsoft.Data.Sqlite.SqliteTransaction transaction, IReadOnlyCollection<Agent> rows, CancellationToken cancellationToken)
    {
        using var cmd = connection.CreateCommand();
        cmd.Transaction = transaction;
        cmd.CommandText = "INSERT INTO agtAgents (agentID, divisionID, corporationID, locationID, level, quality, agentTypeID, isLocator) VALUES ($agentID, $divisionID, $corporationID, $locationID, $level, $quality, $agentTypeID, $isLocator);";

        var agentId = cmd.CreateParameter(); agentId.ParameterName = "$agentID"; cmd.Parameters.Add(agentId);
        var divisionId = cmd.CreateParameter(); divisionId.ParameterName = "$divisionID"; cmd.Parameters.Add(divisionId);
        var corporationId = cmd.CreateParameter(); corporationId.ParameterName = "$corporationID"; cmd.Parameters.Add(corporationId);
        var locationId = cmd.CreateParameter(); locationId.ParameterName = "$locationID"; cmd.Parameters.Add(locationId);
        var level = cmd.CreateParameter(); level.ParameterName = "$level"; cmd.Parameters.Add(level);
        var quality = cmd.CreateParameter(); quality.ParameterName = "$quality"; cmd.Parameters.Add(quality);
        var agentTypeId = cmd.CreateParameter(); agentTypeId.ParameterName = "$agentTypeID"; cmd.Parameters.Add(agentTypeId);
        var isLocator = cmd.CreateParameter(); isLocator.ParameterName = "$isLocator"; cmd.Parameters.Add(isLocator);

        foreach (var row in rows)
        {
            agentId.Value = row.AgentId;
            divisionId.Value = row.DivisionId.HasValue ? row.DivisionId.Value : DBNull.Value;
            corporationId.Value = row.CorporationId.HasValue ? row.CorporationId.Value : DBNull.Value;
            locationId.Value = row.LocationId.HasValue ? row.LocationId.Value : DBNull.Value;
            level.Value = row.Level.HasValue ? row.Level.Value : DBNull.Value;
            quality.Value = row.Quality.HasValue ? row.Quality.Value : DBNull.Value;
            agentTypeId.Value = row.AgentTypeId.HasValue ? row.AgentTypeId.Value : DBNull.Value;
            isLocator.Value = row.IsLocator.HasValue ? (row.IsLocator.Value ? 1 : 0) : DBNull.Value;
            await cmd.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    private static async Task InsertResearchAgentsAsync(Microsoft.Data.Sqlite.SqliteConnection connection, Microsoft.Data.Sqlite.SqliteTransaction transaction, IReadOnlyCollection<ResearchAgent> rows, CancellationToken cancellationToken)
    {
        using var cmd = connection.CreateCommand();
        cmd.Transaction = transaction;
        cmd.CommandText = "INSERT INTO agtResearchAgents (agentID, typeID) VALUES ($agentID, $typeID);";

        var agentId = cmd.CreateParameter(); agentId.ParameterName = "$agentID"; cmd.Parameters.Add(agentId);
        var typeId = cmd.CreateParameter(); typeId.ParameterName = "$typeID"; cmd.Parameters.Add(typeId);

        foreach (var row in rows)
        {
            agentId.Value = row.AgentId;
            typeId.Value = row.TypeId;
            await cmd.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    private static async Task InsertResearchFieldsAsync(Microsoft.Data.Sqlite.SqliteConnection connection, Microsoft.Data.Sqlite.SqliteTransaction transaction, IReadOnlyCollection<NpcCorporationResearchField> rows, CancellationToken cancellationToken)
    {
        using var cmd = connection.CreateCommand();
        cmd.Transaction = transaction;
        cmd.CommandText = "INSERT INTO crpNPCCorporationResearchFields (skillID, corporationID) VALUES ($skillID, $corporationID);";

        var skillId = cmd.CreateParameter(); skillId.ParameterName = "$skillID"; cmd.Parameters.Add(skillId);
        var corporationId = cmd.CreateParameter(); corporationId.ParameterName = "$corporationID"; cmd.Parameters.Add(corporationId);

        foreach (var row in rows)
        {
            skillId.Value = row.SkillId;
            corporationId.Value = row.CorporationId;
            await cmd.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    private static async Task InsertCorporationTradesAsync(Microsoft.Data.Sqlite.SqliteConnection connection, Microsoft.Data.Sqlite.SqliteTransaction transaction, IReadOnlyCollection<NpcCorporationTrade> rows, CancellationToken cancellationToken)
    {
        using var cmd = connection.CreateCommand();
        cmd.Transaction = transaction;
        cmd.CommandText = "INSERT INTO crpNPCCorporationTrades (corporationID, typeID) VALUES ($corporationID, $typeID);";

        var corporationId = cmd.CreateParameter(); corporationId.ParameterName = "$corporationID"; cmd.Parameters.Add(corporationId);
        var typeId = cmd.CreateParameter(); typeId.ParameterName = "$typeID"; cmd.Parameters.Add(typeId);

        foreach (var row in rows)
        {
            corporationId.Value = row.CorporationId;
            typeId.Value = row.TypeId;
            await cmd.ExecuteNonQueryAsync(cancellationToken);
        }
    }
}
