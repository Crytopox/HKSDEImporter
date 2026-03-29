using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Infrastructure.Sqlite.Connections;

namespace HKSDEImporter.Infrastructure.Sqlite.Writers;

public sealed class MarketGroupWriter : IMarketGroupWriter
{
    private readonly SqliteConnectionFactory _connectionFactory;

    public MarketGroupWriter(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task WriteAsync(string outputPath, IReadOnlyCollection<MarketGroup> marketGroups, CancellationToken cancellationToken)
    {
        await using var connection = _connectionFactory.Create(Path.GetFullPath(outputPath));
        await connection.OpenAsync(cancellationToken);
        using var transaction = connection.BeginTransaction();

        using var command = connection.CreateCommand();
        command.CommandText = "INSERT INTO invMarketGroups (marketGroupID, parentGroupID, marketGroupName, description, iconID, hasTypes) VALUES ($marketGroupID, $parentGroupID, $marketGroupName, $description, $iconID, $hasTypes);";
        command.Transaction = transaction;

        var marketGroupId = command.CreateParameter(); marketGroupId.ParameterName = "$marketGroupID"; command.Parameters.Add(marketGroupId);
        var parentGroupId = command.CreateParameter(); parentGroupId.ParameterName = "$parentGroupID"; command.Parameters.Add(parentGroupId);
        var marketGroupName = command.CreateParameter(); marketGroupName.ParameterName = "$marketGroupName"; command.Parameters.Add(marketGroupName);
        var description = command.CreateParameter(); description.ParameterName = "$description"; command.Parameters.Add(description);
        var iconId = command.CreateParameter(); iconId.ParameterName = "$iconID"; command.Parameters.Add(iconId);
        var hasTypes = command.CreateParameter(); hasTypes.ParameterName = "$hasTypes"; command.Parameters.Add(hasTypes);

        foreach (var item in marketGroups)
        {
            marketGroupId.Value = item.MarketGroupId;
            parentGroupId.Value = item.ParentGroupId.HasValue ? item.ParentGroupId.Value : DBNull.Value;
            marketGroupName.Value = item.Name;
            description.Value = string.IsNullOrWhiteSpace(item.Description) ? DBNull.Value : item.Description;
            iconId.Value = item.IconId.HasValue ? item.IconId.Value : DBNull.Value;
            hasTypes.Value = item.HasTypes ? 1 : 0;
            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        transaction.Commit();
    }
}
