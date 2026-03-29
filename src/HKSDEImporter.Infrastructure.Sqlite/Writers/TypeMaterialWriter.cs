using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Infrastructure.Sqlite.Connections;

namespace HKSDEImporter.Infrastructure.Sqlite.Writers;

public sealed class TypeMaterialWriter : ITypeMaterialWriter
{
    private readonly SqliteConnectionFactory _connectionFactory;

    public TypeMaterialWriter(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task WriteAsync(string outputPath, IReadOnlyCollection<TypeMaterial> materials, CancellationToken cancellationToken)
    {
        await using var connection = _connectionFactory.Create(Path.GetFullPath(outputPath));
        await connection.OpenAsync(cancellationToken);
        using var transaction = connection.BeginTransaction();

        using var command = connection.CreateCommand();
        command.CommandText = "INSERT INTO invTypeMaterials (typeID, materialTypeID, quantity) VALUES ($typeID, $materialTypeID, $quantity);";
        command.Transaction = transaction;

        var typeId = command.CreateParameter(); typeId.ParameterName = "$typeID"; command.Parameters.Add(typeId);
        var materialTypeId = command.CreateParameter(); materialTypeId.ParameterName = "$materialTypeID"; command.Parameters.Add(materialTypeId);
        var quantity = command.CreateParameter(); quantity.ParameterName = "$quantity"; command.Parameters.Add(quantity);

        foreach (var item in materials)
        {
            typeId.Value = item.TypeId;
            materialTypeId.Value = item.MaterialTypeId;
            quantity.Value = item.Quantity;
            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        transaction.Commit();
    }
}
