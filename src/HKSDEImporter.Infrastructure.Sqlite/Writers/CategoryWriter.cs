using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Infrastructure.Sqlite.Connections;
using Microsoft.Data.Sqlite;

namespace HKSDEImporter.Infrastructure.Sqlite.Writers;

public sealed class CategoryWriter : ICategoryWriter
{
    private readonly SqliteConnectionFactory _connectionFactory;

    public CategoryWriter(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task WriteAsync(string outputPath, IReadOnlyCollection<Category> categories, CancellationToken cancellationToken)
    {
        await using var connection = _connectionFactory.Create(Path.GetFullPath(outputPath));
        await connection.OpenAsync(cancellationToken);
        using var transaction = connection.BeginTransaction();

        using var command = connection.CreateCommand();
        command.CommandText = "INSERT INTO categories (category_id, name, published, icon_id) VALUES ($categoryId, $name, $published, $iconId);";
        command.Transaction = transaction;

        var categoryId = command.CreateParameter();
        categoryId.ParameterName = "$categoryId";
        command.Parameters.Add(categoryId);

        var name = command.CreateParameter();
        name.ParameterName = "$name";
        command.Parameters.Add(name);

        var published = command.CreateParameter();
        published.ParameterName = "$published";
        command.Parameters.Add(published);

        var iconId = command.CreateParameter();
        iconId.ParameterName = "$iconId";
        command.Parameters.Add(iconId);

        foreach (var category in categories)
        {
            cancellationToken.ThrowIfCancellationRequested();

            categoryId.Value = category.CategoryId;
            name.Value = category.Name;
            published.Value = category.Published ? 1 : 0;
            iconId.Value = category.IconId.HasValue ? category.IconId.Value : DBNull.Value;

            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        transaction.Commit();
    }
}
