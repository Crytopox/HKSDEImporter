using Microsoft.Data.Sqlite;

namespace HKSDEImporter.Infrastructure.Sqlite.Connections;

public sealed class SqliteConnectionFactory
{
    public SqliteConnection Create(string databasePath)
    {
        var builder = new SqliteConnectionStringBuilder
        {
            DataSource = databasePath,
            Mode = SqliteOpenMode.ReadWriteCreate,
            Cache = SqliteCacheMode.Private,
            ForeignKeys = true,
            Pooling = false
        };

        return new SqliteConnection(builder.ConnectionString);
    }
}
