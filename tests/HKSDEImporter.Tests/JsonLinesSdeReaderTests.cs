using HKSDEImporter.Infrastructure.Json.Readers;

namespace HKSDEImporter.Tests;

public sealed class JsonLinesSdeReaderTests
{
    [Fact]
    public async Task Reader_ParsesThreeEntityFiles()
    {
        var dir = CreateFixtureDirectory();

        try
        {
            var reader = new JsonLinesSdeReader();
            var categories = await ToListAsync(reader.ReadCategoriesAsync(dir, CancellationToken.None));
            var groups = await ToListAsync(reader.ReadGroupsAsync(dir, CancellationToken.None));
            var types = await ToListAsync(reader.ReadTypesAsync(dir, CancellationToken.None));

            Assert.Single(categories);
            Assert.Single(groups);
            Assert.Single(types);
            Assert.Equal("Ships", categories[0].Name?.En);
            Assert.Equal("Frigate", groups[0].Name?.En);
            Assert.Equal("Rifter", types[0].Name?.En);
        }
        finally
        {
            Directory.Delete(dir, recursive: true);
        }
    }

    private static string CreateFixtureDirectory()
    {
        var dir = Path.Combine(Path.GetTempPath(), "hksde-tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(dir);

        File.WriteAllLines(Path.Combine(dir, "categories.jsonl"),
        [
            "{\"_key\":25,\"name\":{\"en\":\"Ships\"},\"published\":true}"
        ]);

        File.WriteAllLines(Path.Combine(dir, "groups.jsonl"),
        [
            "{\"_key\":26,\"categoryID\":25,\"name\":{\"en\":\"Frigate\"},\"published\":true}"
        ]);

        File.WriteAllLines(Path.Combine(dir, "types.jsonl"),
        [
            "{\"_key\":27,\"groupID\":26,\"name\":{\"en\":\"Rifter\"},\"description\":{\"en\":\"Fast attack frigate\"},\"portionSize\":1,\"published\":true}"
        ]);

        return dir;
    }

    private static async Task<List<T>> ToListAsync<T>(IAsyncEnumerable<T> source)
    {
        var items = new List<T>();
        await foreach (var item in source)
        {
            items.Add(item);
        }

        return items;
    }
}
