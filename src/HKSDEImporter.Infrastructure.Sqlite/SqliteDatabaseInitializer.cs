using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Infrastructure.Sqlite.Connections;
using HKSDEImporter.Infrastructure.Sqlite.Schema;

namespace HKSDEImporter.Infrastructure.Sqlite;

public sealed class SqliteDatabaseInitializer : IDatabaseInitializer
{
    private readonly SqliteConnectionFactory _connectionFactory;
    private readonly SqliteSchemaBuilder _schemaBuilder;

    public SqliteDatabaseInitializer(SqliteConnectionFactory connectionFactory, SqliteSchemaBuilder schemaBuilder)
    {
        _connectionFactory = connectionFactory;
        _schemaBuilder = schemaBuilder;
    }

    public async Task InitializeAsync(string outputPath, bool overwrite, CancellationToken cancellationToken)
    {
        var fullPath = Path.GetFullPath(outputPath);
        var directory = Path.GetDirectoryName(fullPath);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        if (File.Exists(fullPath))
        {
            if (!overwrite)
            {
                throw new InvalidOperationException($"Output database already exists at '{fullPath}'. Use --overwrite to replace it.");
            }

            File.Delete(fullPath);
        }

        await using var connection = _connectionFactory.Create(fullPath);
        await connection.OpenAsync(cancellationToken);

        await _schemaBuilder.CreateSchemaAsync(connection, cancellationToken);
        await _schemaBuilder.CreateIndexesAsync(connection, cancellationToken);
    }
}
