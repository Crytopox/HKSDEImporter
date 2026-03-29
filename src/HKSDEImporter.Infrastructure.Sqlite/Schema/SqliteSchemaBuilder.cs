using Microsoft.Data.Sqlite;

namespace HKSDEImporter.Infrastructure.Sqlite.Schema;

public sealed class SqliteSchemaBuilder
{
    public async Task CreateSchemaAsync(SqliteConnection connection, CancellationToken cancellationToken)
    {
        var commands = new[]
        {
            """
            CREATE TABLE IF NOT EXISTS categories (
                category_id INTEGER PRIMARY KEY,
                name TEXT NOT NULL,
                published INTEGER NOT NULL,
                icon_id INTEGER NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS groups (
                group_id INTEGER PRIMARY KEY,
                category_id INTEGER NOT NULL,
                name TEXT NOT NULL,
                published INTEGER NOT NULL,
                FOREIGN KEY (category_id) REFERENCES categories(category_id)
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS types (
                type_id INTEGER PRIMARY KEY,
                group_id INTEGER NOT NULL,
                name TEXT NOT NULL,
                description TEXT NULL,
                published INTEGER NOT NULL,
                portion_size INTEGER NOT NULL,
                icon_id INTEGER NULL,
                mass REAL NULL,
                radius REAL NULL,
                volume REAL NULL,
                FOREIGN KEY (group_id) REFERENCES groups(group_id)
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS import_metadata (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                started_at_utc TEXT NOT NULL,
                completed_at_utc TEXT NOT NULL,
                duration_ms INTEGER NOT NULL,
                row_counts_json TEXT NOT NULL,
                warning_count INTEGER NOT NULL,
                error_count INTEGER NOT NULL,
                warnings_json TEXT NULL,
                errors_json TEXT NULL
            );
            """
        };

        foreach (var sql in commands)
        {
            using var command = connection.CreateCommand();
            command.CommandText = sql;
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    public async Task CreateIndexesAsync(SqliteConnection connection, CancellationToken cancellationToken)
    {
        var commands = new[]
        {
            "CREATE INDEX IF NOT EXISTS idx_groups_category_id ON groups(category_id);",
            "CREATE INDEX IF NOT EXISTS idx_types_group_id ON types(group_id);",
            "CREATE INDEX IF NOT EXISTS idx_types_name ON types(name);"
        };

        foreach (var sql in commands)
        {
            using var command = connection.CreateCommand();
            command.CommandText = sql;
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }
}
