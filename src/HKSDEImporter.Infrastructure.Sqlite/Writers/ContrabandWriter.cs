using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Infrastructure.Sqlite.Connections;

namespace HKSDEImporter.Infrastructure.Sqlite.Writers;

public sealed class ContrabandWriter : IContrabandWriter
{
    private readonly SqliteConnectionFactory _connectionFactory;

    public ContrabandWriter(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task WriteAsync(string outputPath, IReadOnlyCollection<ContrabandTypeRule> rules, CancellationToken cancellationToken)
    {
        await using var connection = _connectionFactory.Create(Path.GetFullPath(outputPath));
        await connection.OpenAsync(cancellationToken);
        using var transaction = connection.BeginTransaction();

        using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = "INSERT INTO invContrabandTypes (factionID, typeID, standingLoss, confiscateMinSec, fineByValue, attackMinSec) VALUES ($factionID, $typeID, $standingLoss, $confiscateMinSec, $fineByValue, $attackMinSec);";

        var factionId = command.CreateParameter(); factionId.ParameterName = "$factionID"; command.Parameters.Add(factionId);
        var typeId = command.CreateParameter(); typeId.ParameterName = "$typeID"; command.Parameters.Add(typeId);
        var standingLoss = command.CreateParameter(); standingLoss.ParameterName = "$standingLoss"; command.Parameters.Add(standingLoss);
        var confiscateMinSec = command.CreateParameter(); confiscateMinSec.ParameterName = "$confiscateMinSec"; command.Parameters.Add(confiscateMinSec);
        var fineByValue = command.CreateParameter(); fineByValue.ParameterName = "$fineByValue"; command.Parameters.Add(fineByValue);
        var attackMinSec = command.CreateParameter(); attackMinSec.ParameterName = "$attackMinSec"; command.Parameters.Add(attackMinSec);

        foreach (var row in rules)
        {
            factionId.Value = row.FactionId;
            typeId.Value = row.TypeId;
            standingLoss.Value = row.StandingLoss.HasValue ? row.StandingLoss.Value : DBNull.Value;
            confiscateMinSec.Value = row.ConfiscateMinSec.HasValue ? row.ConfiscateMinSec.Value : DBNull.Value;
            fineByValue.Value = row.FineByValue.HasValue ? row.FineByValue.Value : DBNull.Value;
            attackMinSec.Value = row.AttackMinSec.HasValue ? row.AttackMinSec.Value : DBNull.Value;
            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        transaction.Commit();
    }
}
