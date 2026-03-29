using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Infrastructure.Sqlite.Connections;

namespace HKSDEImporter.Infrastructure.Sqlite.Writers;

public sealed class AgentReferenceWriter : IAgentReferenceWriter
{
    private readonly SqliteConnectionFactory _connectionFactory;

    public AgentReferenceWriter(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task WriteAsync(string outputPath, IReadOnlyCollection<AgentType> agentTypes, IReadOnlyCollection<AgentInSpace> agentsInSpace, CancellationToken cancellationToken)
    {
        await using var connection = _connectionFactory.Create(Path.GetFullPath(outputPath));
        await connection.OpenAsync(cancellationToken);
        using var transaction = connection.BeginTransaction();

        using (var typeCommand = connection.CreateCommand())
        {
            typeCommand.Transaction = transaction;
            typeCommand.CommandText = "INSERT INTO agtAgentTypes (agentTypeID, agentType) VALUES ($agentTypeID, $agentType);";

            var agentTypeId = typeCommand.CreateParameter(); agentTypeId.ParameterName = "$agentTypeID"; typeCommand.Parameters.Add(agentTypeId);
            var agentType = typeCommand.CreateParameter(); agentType.ParameterName = "$agentType"; typeCommand.Parameters.Add(agentType);

            foreach (var row in agentTypes)
            {
                agentTypeId.Value = row.AgentTypeId;
                agentType.Value = string.IsNullOrWhiteSpace(row.AgentTypeName) ? DBNull.Value : row.AgentTypeName;
                await typeCommand.ExecuteNonQueryAsync(cancellationToken);
            }
        }

        using (var agentCommand = connection.CreateCommand())
        {
            agentCommand.Transaction = transaction;
            agentCommand.CommandText = "INSERT INTO agtAgentsInSpace (agentID, dungeonID, solarSystemID, spawnPointID, typeID) VALUES ($agentID, $dungeonID, $solarSystemID, $spawnPointID, $typeID);";

            var agentId = agentCommand.CreateParameter(); agentId.ParameterName = "$agentID"; agentCommand.Parameters.Add(agentId);
            var dungeonId = agentCommand.CreateParameter(); dungeonId.ParameterName = "$dungeonID"; agentCommand.Parameters.Add(dungeonId);
            var solarSystemId = agentCommand.CreateParameter(); solarSystemId.ParameterName = "$solarSystemID"; agentCommand.Parameters.Add(solarSystemId);
            var spawnPointId = agentCommand.CreateParameter(); spawnPointId.ParameterName = "$spawnPointID"; agentCommand.Parameters.Add(spawnPointId);
            var typeId = agentCommand.CreateParameter(); typeId.ParameterName = "$typeID"; agentCommand.Parameters.Add(typeId);

            foreach (var row in agentsInSpace)
            {
                agentId.Value = row.AgentId;
                dungeonId.Value = row.DungeonId.HasValue ? row.DungeonId.Value : DBNull.Value;
                solarSystemId.Value = row.SolarSystemId.HasValue ? row.SolarSystemId.Value : DBNull.Value;
                spawnPointId.Value = row.SpawnPointId.HasValue ? row.SpawnPointId.Value : DBNull.Value;
                typeId.Value = row.TypeId.HasValue ? row.TypeId.Value : DBNull.Value;
                await agentCommand.ExecuteNonQueryAsync(cancellationToken);
            }
        }

        transaction.Commit();
    }
}
