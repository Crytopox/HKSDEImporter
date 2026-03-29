using System.Text.Json;
using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Infrastructure.Sqlite.Connections;

namespace HKSDEImporter.Infrastructure.Sqlite.Writers;

public sealed class ImportMetadataWriter : IImportMetadataWriter
{
    private readonly SqliteConnectionFactory _connectionFactory;

    public ImportMetadataWriter(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task WriteAsync(
        string outputPath,
        DateTime startedAtUtc,
        DateTime completedAtUtc,
        IReadOnlyDictionary<string, long> rowCounts,
        IReadOnlyList<string> warnings,
        IReadOnlyList<string> errors,
        CancellationToken cancellationToken)
    {
        await using var connection = _connectionFactory.Create(Path.GetFullPath(outputPath));
        await connection.OpenAsync(cancellationToken);

        using var command = connection.CreateCommand();
        command.CommandText =
            "INSERT INTO import_metadata (started_at_utc, completed_at_utc, duration_ms, row_counts_json, warning_count, error_count, warnings_json, errors_json) " +
            "VALUES ($startedAt, $completedAt, $durationMs, $rowCountsJson, $warningCount, $errorCount, $warningsJson, $errorsJson);";

        var duration = completedAtUtc - startedAtUtc;

        command.Parameters.AddWithValue("$startedAt", startedAtUtc.ToString("O"));
        command.Parameters.AddWithValue("$completedAt", completedAtUtc.ToString("O"));
        command.Parameters.AddWithValue("$durationMs", (long)duration.TotalMilliseconds);
        command.Parameters.AddWithValue("$rowCountsJson", JsonSerializer.Serialize(rowCounts));
        command.Parameters.AddWithValue("$warningCount", warnings.Count);
        command.Parameters.AddWithValue("$errorCount", errors.Count);
        command.Parameters.AddWithValue("$warningsJson", warnings.Count == 0 ? DBNull.Value : JsonSerializer.Serialize(warnings));
        command.Parameters.AddWithValue("$errorsJson", errors.Count == 0 ? DBNull.Value : JsonSerializer.Serialize(errors));

        await command.ExecuteNonQueryAsync(cancellationToken);
    }
}
