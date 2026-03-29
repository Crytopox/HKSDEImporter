using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Infrastructure.Sqlite.Connections;

namespace HKSDEImporter.Infrastructure.Sqlite.Writers;

public sealed class DogmaAttributeCategoryWriter : IDogmaAttributeCategoryWriter
{
    private readonly SqliteConnectionFactory _connectionFactory;

    public DogmaAttributeCategoryWriter(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task WriteAsync(string outputPath, IReadOnlyCollection<DogmaAttributeCategory> categories, CancellationToken cancellationToken)
    {
        await using var connection = _connectionFactory.Create(Path.GetFullPath(outputPath));
        await connection.OpenAsync(cancellationToken);
        using var transaction = connection.BeginTransaction();

        using var command = connection.CreateCommand();
        command.CommandText = "INSERT INTO dgmAttributeCategories (categoryID, categoryName, categoryDescription) VALUES ($categoryID, $categoryName, $categoryDescription);";
        command.Transaction = transaction;

        var categoryId = command.CreateParameter(); categoryId.ParameterName = "$categoryID"; command.Parameters.Add(categoryId);
        var categoryName = command.CreateParameter(); categoryName.ParameterName = "$categoryName"; command.Parameters.Add(categoryName);
        var categoryDescription = command.CreateParameter(); categoryDescription.ParameterName = "$categoryDescription"; command.Parameters.Add(categoryDescription);

        foreach (var item in categories)
        {
            categoryId.Value = item.CategoryId;
            categoryName.Value = item.CategoryName;
            categoryDescription.Value = string.IsNullOrWhiteSpace(item.CategoryDescription) ? DBNull.Value : item.CategoryDescription;
            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        transaction.Commit();
    }
}
