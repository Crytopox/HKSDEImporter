using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Infrastructure.Sqlite.Connections;

namespace HKSDEImporter.Infrastructure.Sqlite.Writers;

public sealed class MetaGroupWriter : IMetaGroupWriter
{
    private readonly SqliteConnectionFactory _connectionFactory;

    public MetaGroupWriter(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task WriteAsync(string outputPath, IReadOnlyCollection<MetaGroup> metaGroups, CancellationToken cancellationToken)
    {
        await using var connection = _connectionFactory.Create(Path.GetFullPath(outputPath));
        await connection.OpenAsync(cancellationToken);
        using var transaction = connection.BeginTransaction();

        using var command = connection.CreateCommand();
        command.CommandText = "INSERT INTO invMetaGroups (metaGroupID, metaGroupName, description, iconID) VALUES ($metaGroupID, $metaGroupName, $description, $iconID);";
        command.Transaction = transaction;

        var metaGroupId = command.CreateParameter(); metaGroupId.ParameterName = "$metaGroupID"; command.Parameters.Add(metaGroupId);
        var metaGroupName = command.CreateParameter(); metaGroupName.ParameterName = "$metaGroupName"; command.Parameters.Add(metaGroupName);
        var description = command.CreateParameter(); description.ParameterName = "$description"; command.Parameters.Add(description);
        var iconId = command.CreateParameter(); iconId.ParameterName = "$iconID"; command.Parameters.Add(iconId);

        foreach (var item in metaGroups)
        {
            metaGroupId.Value = item.MetaGroupId;
            metaGroupName.Value = item.Name;
            description.Value = string.IsNullOrWhiteSpace(item.Description) ? DBNull.Value : item.Description;
            iconId.Value = item.IconId.HasValue ? item.IconId.Value : DBNull.Value;
            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        transaction.Commit();
    }
}
