using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Infrastructure.Sqlite.Connections;

namespace HKSDEImporter.Infrastructure.Sqlite.Writers;

public sealed class GroupWriter : IGroupWriter
{
    private readonly SqliteConnectionFactory _connectionFactory;

    public GroupWriter(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task WriteAsync(string outputPath, IReadOnlyCollection<Group> groups, CancellationToken cancellationToken)
    {
        await using var connection = _connectionFactory.Create(Path.GetFullPath(outputPath));
        await connection.OpenAsync(cancellationToken);
        using var transaction = connection.BeginTransaction();

        using var command = connection.CreateCommand();
        command.CommandText = "INSERT INTO groups (group_id, category_id, name, published) VALUES ($groupId, $categoryId, $name, $published);";
        command.Transaction = transaction;

        var groupId = command.CreateParameter();
        groupId.ParameterName = "$groupId";
        command.Parameters.Add(groupId);

        var categoryId = command.CreateParameter();
        categoryId.ParameterName = "$categoryId";
        command.Parameters.Add(categoryId);

        var name = command.CreateParameter();
        name.ParameterName = "$name";
        command.Parameters.Add(name);

        var published = command.CreateParameter();
        published.ParameterName = "$published";
        command.Parameters.Add(published);

        foreach (var group in groups)
        {
            cancellationToken.ThrowIfCancellationRequested();

            groupId.Value = group.GroupId;
            categoryId.Value = group.CategoryId;
            name.Value = group.Name;
            published.Value = group.Published ? 1 : 0;

            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        transaction.Commit();
    }
}
