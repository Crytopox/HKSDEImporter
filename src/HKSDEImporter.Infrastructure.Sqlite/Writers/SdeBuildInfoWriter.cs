using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Infrastructure.Sqlite.Connections;

namespace HKSDEImporter.Infrastructure.Sqlite.Writers;

public sealed class SdeBuildInfoWriter : ISdeBuildInfoWriter
{
    private readonly SqliteConnectionFactory _connectionFactory;

    public SdeBuildInfoWriter(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task WriteAsync(string outputPath, IReadOnlyCollection<SdeBuildInfo> rows, CancellationToken cancellationToken)
    {
        await using var connection = _connectionFactory.Create(Path.GetFullPath(outputPath));
        await connection.OpenAsync(cancellationToken);
        using var transaction = connection.BeginTransaction();

        using (var delete = connection.CreateCommand())
        {
            delete.Transaction = transaction;
            delete.CommandText = "DELETE FROM _hki_sde_build;";
            await delete.ExecuteNonQueryAsync(cancellationToken);
        }

        using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = "INSERT INTO _hki_sde_build (source_key, build_number, release_date_utc) VALUES ($sourceKey, $buildNumber, $releaseDateUtc);";
        var sourceKey = command.CreateParameter(); sourceKey.ParameterName = "$sourceKey"; command.Parameters.Add(sourceKey);
        var buildNumber = command.CreateParameter(); buildNumber.ParameterName = "$buildNumber"; command.Parameters.Add(buildNumber);
        var releaseDateUtc = command.CreateParameter(); releaseDateUtc.ParameterName = "$releaseDateUtc"; command.Parameters.Add(releaseDateUtc);

        foreach (var row in rows)
        {
            sourceKey.Value = row.SourceKey;
            buildNumber.Value = row.BuildNumber;
            releaseDateUtc.Value = row.ReleaseDateUtc.ToUniversalTime().ToString("O");
            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        transaction.Commit();
    }
}
