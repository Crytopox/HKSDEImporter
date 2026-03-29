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
        command.CommandText =
            "INSERT INTO invGroups (groupID, categoryID, groupName, iconID, useBasePrice, anchored, anchorable, fittableNonSingleton, published) " +
            "VALUES ($groupId, $categoryId, $groupName, $iconId, $useBasePrice, $anchored, $anchorable, $fittableNonSingleton, $published);";
        command.Transaction = transaction;

        var groupId = command.CreateParameter();
        groupId.ParameterName = "$groupId";
        command.Parameters.Add(groupId);

        var categoryId = command.CreateParameter();
        categoryId.ParameterName = "$categoryId";
        command.Parameters.Add(categoryId);

        var groupName = command.CreateParameter();
        groupName.ParameterName = "$groupName";
        command.Parameters.Add(groupName);

        var iconId = command.CreateParameter();
        iconId.ParameterName = "$iconId";
        command.Parameters.Add(iconId);

        var useBasePrice = command.CreateParameter();
        useBasePrice.ParameterName = "$useBasePrice";
        command.Parameters.Add(useBasePrice);

        var anchored = command.CreateParameter();
        anchored.ParameterName = "$anchored";
        command.Parameters.Add(anchored);

        var anchorable = command.CreateParameter();
        anchorable.ParameterName = "$anchorable";
        command.Parameters.Add(anchorable);

        var fittableNonSingleton = command.CreateParameter();
        fittableNonSingleton.ParameterName = "$fittableNonSingleton";
        command.Parameters.Add(fittableNonSingleton);

        var published = command.CreateParameter();
        published.ParameterName = "$published";
        command.Parameters.Add(published);

        foreach (var group in groups)
        {
            cancellationToken.ThrowIfCancellationRequested();

            groupId.Value = group.GroupId;
            categoryId.Value = group.CategoryId;
            groupName.Value = group.Name;
            iconId.Value = group.IconId.HasValue ? group.IconId.Value : DBNull.Value;
            useBasePrice.Value = group.UseBasePrice ? 1 : 0;
            anchored.Value = group.Anchored ? 1 : 0;
            anchorable.Value = group.Anchorable ? 1 : 0;
            fittableNonSingleton.Value = group.FittableNonSingleton ? 1 : 0;
            published.Value = group.Published ? 1 : 0;

            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        transaction.Commit();
    }
}
