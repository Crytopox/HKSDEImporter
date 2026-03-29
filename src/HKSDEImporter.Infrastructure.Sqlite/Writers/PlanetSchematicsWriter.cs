using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Infrastructure.Sqlite.Connections;

namespace HKSDEImporter.Infrastructure.Sqlite.Writers;

public sealed class PlanetSchematicsWriter : IPlanetSchematicsWriter
{
    private readonly SqliteConnectionFactory _connectionFactory;

    public PlanetSchematicsWriter(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task WriteAsync(string outputPath, IReadOnlyCollection<PlanetSchematic> schematics, IReadOnlyCollection<PlanetSchematicPinMap> pinMaps, IReadOnlyCollection<PlanetSchematicTypeMap> typeMaps, CancellationToken cancellationToken)
    {
        await using var connection = _connectionFactory.Create(Path.GetFullPath(outputPath));
        await connection.OpenAsync(cancellationToken);
        using var transaction = connection.BeginTransaction();

        using (var command = connection.CreateCommand())
        {
            command.Transaction = transaction;
            command.CommandText = "INSERT INTO planetSchematics (schematicID, schematicName, cycleTime) VALUES ($schematicID, $schematicName, $cycleTime);";
            var schematicId = command.CreateParameter(); schematicId.ParameterName = "$schematicID"; command.Parameters.Add(schematicId);
            var schematicName = command.CreateParameter(); schematicName.ParameterName = "$schematicName"; command.Parameters.Add(schematicName);
            var cycleTime = command.CreateParameter(); cycleTime.ParameterName = "$cycleTime"; command.Parameters.Add(cycleTime);

            foreach (var row in schematics)
            {
                schematicId.Value = row.SchematicId;
                schematicName.Value = row.SchematicName;
                cycleTime.Value = row.CycleTime;
                await command.ExecuteNonQueryAsync(cancellationToken);
            }
        }

        using (var command = connection.CreateCommand())
        {
            command.Transaction = transaction;
            command.CommandText = "INSERT INTO planetSchematicsPinMap (schematicID, pinTypeID) VALUES ($schematicID, $pinTypeID);";
            var schematicId = command.CreateParameter(); schematicId.ParameterName = "$schematicID"; command.Parameters.Add(schematicId);
            var pinTypeId = command.CreateParameter(); pinTypeId.ParameterName = "$pinTypeID"; command.Parameters.Add(pinTypeId);

            foreach (var row in pinMaps)
            {
                schematicId.Value = row.SchematicId;
                pinTypeId.Value = row.PinTypeId;
                await command.ExecuteNonQueryAsync(cancellationToken);
            }
        }

        using (var command = connection.CreateCommand())
        {
            command.Transaction = transaction;
            command.CommandText = "INSERT INTO planetSchematicsTypeMap (schematicID, typeID, quantity, isInput) VALUES ($schematicID, $typeID, $quantity, $isInput);";
            var schematicId = command.CreateParameter(); schematicId.ParameterName = "$schematicID"; command.Parameters.Add(schematicId);
            var typeId = command.CreateParameter(); typeId.ParameterName = "$typeID"; command.Parameters.Add(typeId);
            var quantity = command.CreateParameter(); quantity.ParameterName = "$quantity"; command.Parameters.Add(quantity);
            var isInput = command.CreateParameter(); isInput.ParameterName = "$isInput"; command.Parameters.Add(isInput);

            foreach (var row in typeMaps)
            {
                schematicId.Value = row.SchematicId;
                typeId.Value = row.TypeId;
                quantity.Value = row.Quantity;
                isInput.Value = row.IsInput ? 1 : 0;
                await command.ExecuteNonQueryAsync(cancellationToken);
            }
        }

        transaction.Commit();
    }
}
