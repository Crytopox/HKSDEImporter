using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Infrastructure.Sqlite.Connections;

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
        command.CommandText = "INSERT INTO invCategories (categoryID, categoryName, iconID, published) VALUES ($categoryId, $categoryName, $iconId, $published);";
        command.Transaction = transaction;

        var categoryId = command.CreateParameter();
        categoryId.ParameterName = "$categoryId";
        command.Parameters.Add(categoryId);

        var categoryName = command.CreateParameter();
        categoryName.ParameterName = "$categoryName";
        command.Parameters.Add(categoryName);

        var iconId = command.CreateParameter();
        iconId.ParameterName = "$iconId";
        command.Parameters.Add(iconId);

        var published = command.CreateParameter();
        published.ParameterName = "$published";
        command.Parameters.Add(published);

        foreach (var category in categories)
        {
            cancellationToken.ThrowIfCancellationRequested();

            categoryId.Value = category.CategoryId;
            categoryName.Value = category.Name;
            iconId.Value = category.IconId.HasValue ? category.IconId.Value : DBNull.Value;
            published.Value = category.Published ? 1 : 0;

            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        transaction.Commit();
    }
}
