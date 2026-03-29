using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.ImportSteps;
using HKSDEImporter.Core.Mapping;
using HKSDEImporter.Core.Options;
using HKSDEImporter.Core.Services;
using HKSDEImporter.Core.Validation;
using HKSDEImporter.Infrastructure.Json.FileSystem;
using HKSDEImporter.Infrastructure.Json.Readers;
using HKSDEImporter.Infrastructure.Sqlite;
using HKSDEImporter.Infrastructure.Sqlite.Connections;
using HKSDEImporter.Infrastructure.Sqlite.Schema;
using HKSDEImporter.Infrastructure.Sqlite.Writers;
using Microsoft.Data.Sqlite;

namespace HKSDEImporter.Tests;

public sealed class IntegrationImportTests
{
    [Fact]
    public async Task FullImport_WritesExpectedRowsToSqlite()
    {
        var inputDir = CreateFixtureDirectory();
        var outputDb = Path.Combine(Path.GetTempPath(), "hksde-tests", $"{Guid.NewGuid():N}.db");

        try
        {
            var options = new ImportOptions
            {
                InputMode = SdeInputMode.Local,
                InputPath = inputDir,
                OutputPath = outputDb,
                Overwrite = true,
                Verbose = false
            };

            var connectionFactory = new SqliteConnectionFactory();
            var reader = new JsonLinesSdeReader();
            var observer = new NullImportObserver();
            ISdeSourceProvider source = new LocalDirectorySdeSourceProvider(inputDir);

            var steps = new List<IImportStep>
            {
                new ImportCategoriesStep(reader, new CategoryMapper(), new CategoryValidator(), new CategoryWriter(connectionFactory), observer),
                new ImportGroupsStep(reader, new GroupMapper(), new GroupValidator(), new GroupWriter(connectionFactory), observer),
                new ImportTypesStep(reader, new TypeMapper(), new TypeValidator(), new TypeWriter(connectionFactory), observer)
            };

            var coordinator = new ImportCoordinator(
                source,
                new SqliteDatabaseInitializer(connectionFactory, new SqliteSchemaBuilder()),
                new ImportMetadataWriter(connectionFactory),
                steps,
                observer);

            var context = await coordinator.RunAsync(options, CancellationToken.None);

            Assert.Equal(2, context.RowCounts["categories"]);
            Assert.Equal(2, context.RowCounts["groups"]);
            Assert.Equal(2, context.RowCounts["types"]);

            await using var conn = new SqliteConnection($"Data Source={outputDb};Pooling=False");
            await conn.OpenAsync();

            Assert.Equal(2, await CountAsync(conn, "categories"));
            Assert.Equal(2, await CountAsync(conn, "groups"));
            Assert.Equal(2, await CountAsync(conn, "types"));
            Assert.Equal(1, await CountAsync(conn, "import_metadata"));
        }
        finally
        {
            if (Directory.Exists(inputDir))
            {
                Directory.Delete(inputDir, recursive: true);
            }

            DeleteFileWithRetry(outputDb);
        }
    }

    private static async Task<long> CountAsync(SqliteConnection connection, string table)
    {
        using var command = connection.CreateCommand();
        command.CommandText = $"SELECT COUNT(1) FROM {table};";
        return (long)(await command.ExecuteScalarAsync() ?? 0L);
    }

    private static void DeleteFileWithRetry(string path)
    {
        if (!File.Exists(path))
        {
            return;
        }

        for (var attempt = 0; attempt < 10; attempt++)
        {
            try
            {
                File.Delete(path);
                return;
            }
            catch (IOException)
            {
                Thread.Sleep(100);
            }
            catch (UnauthorizedAccessException)
            {
                Thread.Sleep(100);
            }
        }
    }

    private static string CreateFixtureDirectory()
    {
        var dir = Path.Combine(Path.GetTempPath(), "hksde-tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(dir);

        File.WriteAllLines(Path.Combine(dir, "categories.jsonl"),
        [
            "{\"_key\":10,\"name\":{\"en\":\"Ships\"},\"published\":true}",
            "{\"_key\":20,\"name\":{\"en\":\"Modules\"},\"published\":true}"
        ]);

        File.WriteAllLines(Path.Combine(dir, "groups.jsonl"),
        [
            "{\"_key\":100,\"categoryID\":10,\"name\":{\"en\":\"Frigate\"},\"published\":true}",
            "{\"_key\":200,\"categoryID\":20,\"name\":{\"en\":\"Weapon\"},\"published\":true}"
        ]);

        File.WriteAllLines(Path.Combine(dir, "types.jsonl"),
        [
            "{\"_key\":1000,\"groupID\":100,\"name\":{\"en\":\"Rifter\"},\"description\":{\"en\":\"Frigate hull\"},\"portionSize\":1,\"published\":true}",
            "{\"_key\":2000,\"groupID\":200,\"name\":{\"en\":\"Autocannon\"},\"description\":{\"en\":\"Projectile weapon\"},\"portionSize\":1,\"published\":true}"
        ]);

        return dir;
    }
}
