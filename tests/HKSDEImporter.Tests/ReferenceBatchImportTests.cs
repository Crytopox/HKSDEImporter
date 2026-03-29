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

public sealed class ReferenceBatchImportTests
{
    [Fact]
    public async Task ReferenceBatch_ImportsExpectedRows()
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
                new ImportStaticVisualsStep(reader, new GraphicMapper(), new IconMapper(), new GraphicValidator(), new IconValidator(), new StaticVisualWriter(connectionFactory), observer),
                new ImportCharacterReferenceStep(reader, new FactionMapper(), new RaceMapper(), new BloodlineMapper(), new AncestryMapper(), new CharacterAttributeMapper(), new FactionValidator(), new RaceValidator(), new BloodlineValidator(), new AncestryValidator(), new CharacterAttributeValidator(), new CharacterReferenceWriter(connectionFactory), observer),
                new ImportCorporationReferenceStep(reader, new CorporationActivityMapper(), new NpcCorporationDivisionMapper(), new NpcCorporationMapper(), new CorporationActivityValidator(), new NpcCorporationDivisionValidator(), new NpcCorporationValidator(), new CorporationReferenceWriter(connectionFactory), observer)
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

            Assert.Equal(1, await CountAsync(conn, "eveGraphics"));
            Assert.Equal(1, await CountAsync(conn, "eveIcons"));
            Assert.Equal(1, await CountAsync(conn, "chrFactions"));
            Assert.Equal(1, await CountAsync(conn, "chrRaces"));
            Assert.Equal(1, await CountAsync(conn, "chrBloodlines"));
            Assert.Equal(1, await CountAsync(conn, "chrAncestries"));
            Assert.Equal(1, await CountAsync(conn, "chrAttributes"));
            Assert.Equal(1, await CountAsync(conn, "crpActivities"));
            Assert.Equal(1, await CountAsync(conn, "crpNPCDivisions"));
            Assert.Equal(1, await CountAsync(conn, "crpNPCCorporations"));
        }
        finally
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

    private static async Task<long> CountAsync(SqliteConnection connection, string table)
    {
        using var command = connection.CreateCommand();
        command.CommandText = $"SELECT COUNT(1) FROM {table};";
        return (long)(await command.ExecuteScalarAsync() ?? 0L);
    }

    private static string CreateFixtureDirectory()
    {
        var dir = Path.Combine(Path.GetTempPath(), "hksde-tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(dir);

        File.WriteAllLines(Path.Combine(dir, "graphics.jsonl"),
        [
            "{\"_key\":10,\"graphicFile\":\"res:/dx9/model/worldobject/planet/moon.red\",\"sofFactionName\":\"testfaction\",\"sofHullName\":\"testhull\",\"sofRaceName\":\"testrace\"}"
        ]);

        File.WriteAllLines(Path.Combine(dir, "icons.jsonl"),
        [
            "{\"_key\":0,\"iconFile\":\"res:/ui/texture/icons/7_64_15.png\"}"
        ]);

        File.WriteAllLines(Path.Combine(dir, "factions.jsonl"),
        [
            "{\"_key\":500001,\"corporationID\":1000035,\"description\":{\"en\":\"Faction desc\"},\"iconID\":1439,\"memberRaces\":[1],\"militiaCorporationID\":1000180,\"name\":{\"en\":\"Faction Name\"},\"sizeFactor\":5.0,\"solarSystemID\":30000145}"
        ]);

        File.WriteAllLines(Path.Combine(dir, "races.jsonl"),
        [
            "{\"_key\":1,\"description\":{\"en\":\"Race desc\"},\"iconID\":1439,\"name\":{\"en\":\"Race Name\"},\"shortDescription\":{\"en\":\"Race short\"}}"
        ]);

        File.WriteAllLines(Path.Combine(dir, "bloodlines.jsonl"),
        [
            "{\"_key\":1,\"charisma\":6,\"corporationID\":1000006,\"description\":{\"en\":\"Bloodline desc\"},\"iconID\":1633,\"intelligence\":7,\"memory\":7,\"name\":{\"en\":\"Bloodline Name\"},\"perception\":5,\"raceID\":1,\"willpower\":5,\"shortDescription\":{\"en\":\"Bloodline short\"}}"
        ]);

        File.WriteAllLines(Path.Combine(dir, "ancestries.jsonl"),
        [
            "{\"_key\":1,\"bloodlineID\":1,\"charisma\":3,\"description\":{\"en\":\"Ancestry desc\"},\"iconID\":1641,\"intelligence\":0,\"memory\":0,\"name\":{\"en\":\"Ancestry Name\"},\"perception\":0,\"shortDescription\":\"Ancestry short\",\"willpower\":1}"
        ]);

        File.WriteAllLines(Path.Combine(dir, "characterAttributes.jsonl"),
        [
            "{\"_key\":1,\"description\":\"Attr desc\",\"iconID\":1380,\"name\":{\"en\":\"Intelligence\"},\"notes\":\"note\",\"shortDescription\":\"short\"}"
        ]);

        File.WriteAllLines(Path.Combine(dir, "corporationActivities.jsonl"),
        [
            "{\"_key\":1,\"name\":{\"en\":\"Agriculture\"}}"
        ]);

        File.WriteAllLines(Path.Combine(dir, "npcCorporationDivisions.jsonl"),
        [
            "{\"_key\":18,\"displayName\":\"Research and development division\",\"internalName\":\"R&D\",\"leaderTypeName\":{\"en\":\"Chief Researcher\"},\"name\":{\"en\":\"R&D\"}}"
        ]);

        File.WriteAllLines(Path.Combine(dir, "npcCorporations.jsonl"),
        [
            "{\"_key\":1000002,\"description\":{\"en\":\"Corp desc\"},\"enemyID\":1000005,\"extent\":\"G\",\"factionID\":500001,\"friendID\":1000006,\"iconID\":1465,\"initialPrice\":47,\"investors\":[{\"_key\":1000002,\"_value\":42}],\"minSecurity\":0.0,\"shares\":30515077373,\"size\":\"H\",\"sizeFactor\":1.75,\"solarSystemID\":30002780}"
        ]);

        return dir;
    }
}
