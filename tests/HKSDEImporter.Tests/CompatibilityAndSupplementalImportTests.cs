using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.ImportSteps;
using HKSDEImporter.Core.Mapping;
using HKSDEImporter.Core.Options;
using HKSDEImporter.Core.Services;
using HKSDEImporter.Infrastructure.Json.FileSystem;
using HKSDEImporter.Infrastructure.Json.Readers;
using HKSDEImporter.Infrastructure.Sqlite;
using HKSDEImporter.Infrastructure.Sqlite.Connections;
using HKSDEImporter.Infrastructure.Sqlite.Schema;
using HKSDEImporter.Infrastructure.Sqlite.Writers;
using HKSDEImporter.Core.Validation;
using Microsoft.Data.Sqlite;

namespace HKSDEImporter.Tests;

public sealed class CompatibilityAndSupplementalImportTests
{
    [Fact]
    public async Task CompatibilityStep_PopulatesMetaAndNpcDivisionTables()
    {
        var inputDir = CreateCompatibilityFixtureDirectory();
        var outputDb = Path.Combine(Path.GetTempPath(), "hksde-tests", $"{Guid.NewGuid():N}.db");

        try
        {
            var options = new ImportOptions
            {
                InputMode = SdeInputMode.Local,
                InputPath = inputDir,
                OutputPath = outputDb,
                Overwrite = true
            };

            var reader = new JsonLinesSdeReader();
            var connectionFactory = new SqliteConnectionFactory();
            var observer = new NullImportObserver();
            ISdeSourceProvider source = new LocalDirectorySdeSourceProvider(inputDir);

            var steps = new List<IImportStep>
            {
                new ImportCategoriesStep(reader, new CategoryMapper(), new CategoryValidator(), new CategoryWriter(connectionFactory), observer),
                new ImportGroupsStep(reader, new GroupMapper(), new GroupValidator(), new GroupWriter(connectionFactory), observer),
                new ImportTypesStep(reader, new TypeMapper(), new TypeValidator(), new TypeWriter(connectionFactory), observer),
                new ImportSdeBuildInfoStep(reader, new SdeBuildInfoWriter(connectionFactory), observer),
                new ImportCompatibilityTablesStep(reader, new CompatibilityWriter(connectionFactory), observer)
            };

            var coordinator = new ImportCoordinator(
                source,
                new SqliteDatabaseInitializer(connectionFactory, new SqliteSchemaBuilder()),
                new ImportMetadataWriter(connectionFactory),
                steps,
                observer);

            await coordinator.RunAsync(options, CancellationToken.None);

            await using var conn = new SqliteConnection($"Data Source={outputDb};Pooling=False");
            await conn.OpenAsync();

            Assert.Equal(2, await CountAsync(conn, "invMetaTypes"));
            Assert.Equal(2, await CountAsync(conn, "crpNPCCorporationDivisions"));
            Assert.Equal(2, await CountAsync(conn, "invVolumes"));
            Assert.Equal(1, await CountAsync(conn, "_hki_sde_build"));
        }
        finally
        {
            Cleanup(inputDir, outputDb);
        }
    }

    [Fact]
    public async Task SupplementalStep_ImportsRemainingJsonFiles()
    {
        var inputDir = CreateSupplementalFixtureDirectory();
        var outputDb = Path.Combine(Path.GetTempPath(), "hksde-tests", $"{Guid.NewGuid():N}.db");

        try
        {
            var options = new ImportOptions
            {
                InputMode = SdeInputMode.Local,
                InputPath = inputDir,
                OutputPath = outputDb,
                Overwrite = true
            };

            var reader = new JsonLinesSdeReader();
            var connectionFactory = new SqliteConnectionFactory();
            var observer = new NullImportObserver();
            ISdeSourceProvider source = new LocalDirectorySdeSourceProvider(inputDir);

            var steps = new List<IImportStep>
            {
                new ImportControlTowerResourcesStep(reader, new ControlTowerResourceMapper(), new ControlTowerResourceValidator(), new ControlTowerResourceWriter(connectionFactory), observer),
                new ImportSupplementalDataStep(reader, new SupplementalDataWriter(connectionFactory), observer)
            };

            var coordinator = new ImportCoordinator(
                source,
                new SqliteDatabaseInitializer(connectionFactory, new SqliteSchemaBuilder()),
                new ImportMetadataWriter(connectionFactory),
                steps,
                observer);

            await coordinator.RunAsync(options, CancellationToken.None);

            await using var conn = new SqliteConnection($"Data Source={outputDb};Pooling=False");
            await conn.OpenAsync();

            Assert.Equal(1, await CountAsync(conn, "chrCloneGrades"));
            Assert.Equal(1, await CountAsync(conn, "chrCloneGradeSkills"));
            Assert.Equal(1, await CountAsync(conn, "invCompressibleTypes"));
            Assert.Equal(1, await CountAsync(conn, "dgmBuffCollections"));
            Assert.Equal(1, await CountAsync(conn, "dgmDynamicItemAttributes"));
            Assert.Equal(1, await CountAsync(conn, "frtFreelanceJobSchemas"));
            Assert.Equal(1, await CountAsync(conn, "mercenaryTacticalOperations"));
            Assert.Equal(1, await CountAsync(conn, "planetResources"));
            Assert.Equal(1, await CountAsync(conn, "sovSovereigntyUpgrades"));
            Assert.Equal(1, await CountAsync(conn, "invTraits"));
            Assert.Equal(1, await CountAsync(conn, "invControlTowerResourcePurposes"));
        }
        finally
        {
            Cleanup(inputDir, outputDb);
        }
    }

    private static async Task<long> CountAsync(SqliteConnection connection, string table)
    {
        using var command = connection.CreateCommand();
        command.CommandText = $"SELECT COUNT(1) FROM {table};";
        return (long)(await command.ExecuteScalarAsync() ?? 0L);
    }

    private static string CreateCompatibilityFixtureDirectory()
    {
        var dir = Path.Combine(Path.GetTempPath(), "hksde-tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(dir);

        File.WriteAllLines(Path.Combine(dir, "categories.jsonl"), ["{\"_key\":1,\"name\":{\"en\":\"Test\"},\"published\":true}"]);
        File.WriteAllLines(Path.Combine(dir, "groups.jsonl"), ["{\"_key\":2,\"categoryID\":1,\"name\":{\"en\":\"Group\"},\"published\":true}"]);
        File.WriteAllLines(Path.Combine(dir, "types.jsonl"),
        [
            "{\"_key\":10,\"groupID\":2,\"name\":{\"en\":\"TypeA\"},\"portionSize\":1,\"published\":true,\"volume\":5.0,\"metaGroupID\":2,\"variationParentTypeID\":5}",
            "{\"_key\":11,\"groupID\":2,\"name\":{\"en\":\"TypeB\"},\"portionSize\":1,\"published\":true,\"volume\":2.0,\"metaGroupID\":1,\"variationParentTypeID\":6}"
        ]);
        File.WriteAllLines(Path.Combine(dir, "npcCorporations.jsonl"),
        [
            "{\"_key\":100,\"description\":{\"en\":\"Corp\"},\"divisions\":[{\"_key\":22,\"size\":5},{\"_key\":23,\"size\":9}]}"
        ]);
        File.WriteAllLines(Path.Combine(dir, "_sde.jsonl"), ["{\"_key\":\"sde\",\"buildNumber\":12345,\"releaseDate\":\"2026-03-27T11:25:23Z\"}"]);

        return dir;
    }

    private static string CreateSupplementalFixtureDirectory()
    {
        var dir = Path.Combine(Path.GetTempPath(), "hksde-tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(dir);

        File.WriteAllLines(Path.Combine(dir, "cloneGrades.jsonl"), ["{\"_key\":1,\"name\":\"Alpha\",\"skills\":[{\"typeID\":3300,\"level\":5}]}"]);
        File.WriteAllLines(Path.Combine(dir, "compressibleTypes.jsonl"), ["{\"_key\":18,\"compressedTypeID\":62528}"]);
        File.WriteAllLines(Path.Combine(dir, "dbuffCollections.jsonl"), ["{\"_key\":1,\"aggregateMode\":\"Maximum\",\"operationName\":\"PostMul\",\"showOutputValueInUI\":\"ShowNormal\",\"developerDescription\":\"Test\"}"]);
        File.WriteAllLines(Path.Combine(dir, "dynamicItemAttributes.jsonl"), ["{\"_key\":47297,\"attributeIDs\":[{\"_key\":6,\"max\":1.4,\"min\":0.6}]}"]);
        File.WriteAllLines(Path.Combine(dir, "freelanceJobSchemas.jsonl"), ["{\"_key\":1,\"_value\":[{\"_key\":\"BoostShield\"}]}"]);
        File.WriteAllLines(Path.Combine(dir, "mercenaryTacticalOperations.jsonl"), ["{\"_key\":12367,\"anarchy_impact\":-20,\"development_impact\":10,\"infomorph_bonus\":350,\"name\":{\"en\":\"Op\"},\"description\":{\"en\":\"Desc\"}}"]);
        File.WriteAllLines(Path.Combine(dir, "planetResources.jsonl"), ["{\"_key\":40013180,\"power\":740}"]);
        File.WriteAllLines(Path.Combine(dir, "sovereigntyUpgrades.jsonl"), ["{\"_key\":81615,\"fuel\":{\"hourly_upkeep\":205,\"startup_cost\":62000,\"type_id\":81143},\"mutually_exclusive_group\":\"Infrastructure_5\",\"power_allocation\":250,\"workforce_allocation\":1500}"]);
        File.WriteAllLines(Path.Combine(dir, "typeBonus.jsonl"), ["{\"_key\":582,\"roleBonuses\":[{\"bonus\":300.0}]}"]);
        File.WriteAllLines(Path.Combine(dir, "controlTowerResources.jsonl"), ["{\"_key\":200,\"resources\":[{\"resourceTypeID\":300,\"purpose\":1,\"quantity\":5}]}"]);

        return dir;
    }

    private static void Cleanup(string inputDir, string outputDb)
    {
        if (Directory.Exists(inputDir))
        {
            Directory.Delete(inputDir, recursive: true);
        }

        if (File.Exists(outputDb))
        {
            File.Delete(outputDb);
        }
    }
}
