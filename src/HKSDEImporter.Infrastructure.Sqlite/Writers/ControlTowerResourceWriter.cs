using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Infrastructure.Sqlite.Connections;

namespace HKSDEImporter.Infrastructure.Sqlite.Writers;

public sealed class ControlTowerResourceWriter : IControlTowerResourceWriter
{
    private readonly SqliteConnectionFactory _connectionFactory;

    public ControlTowerResourceWriter(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task WriteAsync(string outputPath, IReadOnlyCollection<ControlTowerResource> resources, CancellationToken cancellationToken)
    {
        await using var connection = _connectionFactory.Create(Path.GetFullPath(outputPath));
        await connection.OpenAsync(cancellationToken);
        using var transaction = connection.BeginTransaction();

        using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = "INSERT INTO invControlTowerResources (controlTowerTypeID, resourceTypeID, purpose, quantity, minSecurityLevel, factionID) VALUES ($controlTowerTypeID, $resourceTypeID, $purpose, $quantity, $minSecurityLevel, $factionID);";

        var controlTowerTypeId = command.CreateParameter(); controlTowerTypeId.ParameterName = "$controlTowerTypeID"; command.Parameters.Add(controlTowerTypeId);
        var resourceTypeId = command.CreateParameter(); resourceTypeId.ParameterName = "$resourceTypeID"; command.Parameters.Add(resourceTypeId);
        var purpose = command.CreateParameter(); purpose.ParameterName = "$purpose"; command.Parameters.Add(purpose);
        var quantity = command.CreateParameter(); quantity.ParameterName = "$quantity"; command.Parameters.Add(quantity);
        var minSecurityLevel = command.CreateParameter(); minSecurityLevel.ParameterName = "$minSecurityLevel"; command.Parameters.Add(minSecurityLevel);
        var factionId = command.CreateParameter(); factionId.ParameterName = "$factionID"; command.Parameters.Add(factionId);

        foreach (var row in resources)
        {
            controlTowerTypeId.Value = row.ControlTowerTypeId;
            resourceTypeId.Value = row.ResourceTypeId;
            purpose.Value = row.Purpose.HasValue ? row.Purpose.Value : DBNull.Value;
            quantity.Value = row.Quantity.HasValue ? row.Quantity.Value : DBNull.Value;
            minSecurityLevel.Value = row.MinSecurityLevel.HasValue ? row.MinSecurityLevel.Value : DBNull.Value;
            factionId.Value = row.FactionId.HasValue ? row.FactionId.Value : DBNull.Value;
            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        transaction.Commit();
    }
}
