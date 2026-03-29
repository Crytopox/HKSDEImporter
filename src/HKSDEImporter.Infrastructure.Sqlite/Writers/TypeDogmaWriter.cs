using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Infrastructure.Sqlite.Connections;

namespace HKSDEImporter.Infrastructure.Sqlite.Writers;

public sealed class TypeDogmaWriter : ITypeDogmaWriter
{
    private readonly SqliteConnectionFactory _connectionFactory;

    public TypeDogmaWriter(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task WriteAsync(string outputPath, IReadOnlyCollection<TypeDogmaAttribute> attributes, IReadOnlyCollection<TypeDogmaEffect> effects, CancellationToken cancellationToken)
    {
        await using var connection = _connectionFactory.Create(Path.GetFullPath(outputPath));
        await connection.OpenAsync(cancellationToken);
        using var transaction = connection.BeginTransaction();

        using (var command = connection.CreateCommand())
        {
            command.CommandText = "INSERT INTO dgmTypeAttributes (typeID, attributeID, valueInt, valueFloat) VALUES ($typeID, $attributeID, $valueInt, $valueFloat);";
            command.Transaction = transaction;

            var typeId = command.CreateParameter(); typeId.ParameterName = "$typeID"; command.Parameters.Add(typeId);
            var attributeId = command.CreateParameter(); attributeId.ParameterName = "$attributeID"; command.Parameters.Add(attributeId);
            var valueInt = command.CreateParameter(); valueInt.ParameterName = "$valueInt"; command.Parameters.Add(valueInt);
            var valueFloat = command.CreateParameter(); valueFloat.ParameterName = "$valueFloat"; command.Parameters.Add(valueFloat);

            foreach (var item in attributes)
            {
                typeId.Value = item.TypeId;
                attributeId.Value = item.AttributeId;
                valueInt.Value = item.ValueInt.HasValue ? item.ValueInt.Value : DBNull.Value;
                valueFloat.Value = item.ValueFloat.HasValue ? item.ValueFloat.Value : DBNull.Value;
                await command.ExecuteNonQueryAsync(cancellationToken);
            }
        }

        using (var command = connection.CreateCommand())
        {
            command.CommandText = "INSERT INTO dgmTypeEffects (typeID, effectID, isDefault) VALUES ($typeID, $effectID, $isDefault);";
            command.Transaction = transaction;

            var typeId = command.CreateParameter(); typeId.ParameterName = "$typeID"; command.Parameters.Add(typeId);
            var effectId = command.CreateParameter(); effectId.ParameterName = "$effectID"; command.Parameters.Add(effectId);
            var isDefault = command.CreateParameter(); isDefault.ParameterName = "$isDefault"; command.Parameters.Add(isDefault);

            foreach (var item in effects)
            {
                typeId.Value = item.TypeId;
                effectId.Value = item.EffectId;
                isDefault.Value = item.IsDefault ? 1 : 0;
                await command.ExecuteNonQueryAsync(cancellationToken);
            }
        }

        transaction.Commit();
    }
}
