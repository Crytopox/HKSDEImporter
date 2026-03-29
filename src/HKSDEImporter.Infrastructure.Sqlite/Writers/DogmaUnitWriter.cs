using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Infrastructure.Sqlite.Connections;

namespace HKSDEImporter.Infrastructure.Sqlite.Writers;

public sealed class DogmaUnitWriter : IDogmaUnitWriter
{
    private readonly SqliteConnectionFactory _connectionFactory;

    public DogmaUnitWriter(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task WriteAsync(string outputPath, IReadOnlyCollection<DogmaUnit> units, CancellationToken cancellationToken)
    {
        await using var connection = _connectionFactory.Create(Path.GetFullPath(outputPath));
        await connection.OpenAsync(cancellationToken);
        using var transaction = connection.BeginTransaction();

        using var command = connection.CreateCommand();
        command.CommandText = "INSERT INTO eveUnits (unitID, unitName, displayName, description) VALUES ($unitID, $unitName, $displayName, $description);";
        command.Transaction = transaction;

        var unitId = command.CreateParameter(); unitId.ParameterName = "$unitID"; command.Parameters.Add(unitId);
        var unitName = command.CreateParameter(); unitName.ParameterName = "$unitName"; command.Parameters.Add(unitName);
        var displayName = command.CreateParameter(); displayName.ParameterName = "$displayName"; command.Parameters.Add(displayName);
        var description = command.CreateParameter(); description.ParameterName = "$description"; command.Parameters.Add(description);

        foreach (var item in units)
        {
            unitId.Value = item.UnitId;
            unitName.Value = item.UnitName;
            displayName.Value = string.IsNullOrWhiteSpace(item.DisplayName) ? DBNull.Value : item.DisplayName;
            description.Value = string.IsNullOrWhiteSpace(item.Description) ? DBNull.Value : item.Description;
            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        transaction.Commit();
    }
}
