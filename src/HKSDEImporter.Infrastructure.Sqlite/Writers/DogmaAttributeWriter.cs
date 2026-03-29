using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Infrastructure.Sqlite.Connections;

namespace HKSDEImporter.Infrastructure.Sqlite.Writers;

public sealed class DogmaAttributeWriter : IDogmaAttributeWriter
{
    private readonly SqliteConnectionFactory _connectionFactory;

    public DogmaAttributeWriter(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task WriteAsync(string outputPath, IReadOnlyCollection<DogmaAttributeType> attributes, CancellationToken cancellationToken)
    {
        await using var connection = _connectionFactory.Create(Path.GetFullPath(outputPath));
        await connection.OpenAsync(cancellationToken);
        using var transaction = connection.BeginTransaction();

        using var command = connection.CreateCommand();
        command.CommandText =
            "INSERT INTO dgmAttributeTypes (attributeID, attributeName, description, iconID, defaultValue, published, displayName, unitID, stackable, highIsGood, categoryID) " +
            "VALUES ($attributeID, $attributeName, $description, $iconID, $defaultValue, $published, $displayName, $unitID, $stackable, $highIsGood, $categoryID);";
        command.Transaction = transaction;

        var attributeId = command.CreateParameter(); attributeId.ParameterName = "$attributeID"; command.Parameters.Add(attributeId);
        var attributeName = command.CreateParameter(); attributeName.ParameterName = "$attributeName"; command.Parameters.Add(attributeName);
        var description = command.CreateParameter(); description.ParameterName = "$description"; command.Parameters.Add(description);
        var iconId = command.CreateParameter(); iconId.ParameterName = "$iconID"; command.Parameters.Add(iconId);
        var defaultValue = command.CreateParameter(); defaultValue.ParameterName = "$defaultValue"; command.Parameters.Add(defaultValue);
        var published = command.CreateParameter(); published.ParameterName = "$published"; command.Parameters.Add(published);
        var displayName = command.CreateParameter(); displayName.ParameterName = "$displayName"; command.Parameters.Add(displayName);
        var unitId = command.CreateParameter(); unitId.ParameterName = "$unitID"; command.Parameters.Add(unitId);
        var stackable = command.CreateParameter(); stackable.ParameterName = "$stackable"; command.Parameters.Add(stackable);
        var highIsGood = command.CreateParameter(); highIsGood.ParameterName = "$highIsGood"; command.Parameters.Add(highIsGood);
        var categoryId = command.CreateParameter(); categoryId.ParameterName = "$categoryID"; command.Parameters.Add(categoryId);

        foreach (var item in attributes)
        {
            attributeId.Value = item.AttributeId;
            attributeName.Value = item.AttributeName;
            description.Value = string.IsNullOrWhiteSpace(item.Description) ? DBNull.Value : item.Description;
            iconId.Value = item.IconId.HasValue ? item.IconId.Value : DBNull.Value;
            defaultValue.Value = item.DefaultValue.HasValue ? item.DefaultValue.Value : DBNull.Value;
            published.Value = item.Published ? 1 : 0;
            displayName.Value = string.IsNullOrWhiteSpace(item.DisplayName) ? DBNull.Value : item.DisplayName;
            unitId.Value = item.UnitId.HasValue ? item.UnitId.Value : DBNull.Value;
            stackable.Value = item.Stackable ? 1 : 0;
            highIsGood.Value = item.HighIsGood ? 1 : 0;
            categoryId.Value = item.CategoryId.HasValue ? item.CategoryId.Value : DBNull.Value;

            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        transaction.Commit();
    }
}
