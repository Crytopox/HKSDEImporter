using System.Runtime.CompilerServices;
using System.Text.Json;

namespace HKSDEImporter.Infrastructure.Json.Readers;

internal sealed class JsonLinesFileReader
{
    public async Task<long> CountLinesAsync(string filePath, CancellationToken cancellationToken)
    {
        await using var stream = File.OpenRead(filePath);
        using var reader = new StreamReader(stream);

        var count = 0L;
        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var line = await reader.ReadLineAsync(cancellationToken);
            if (line is null)
            {
                break;
            }

            if (!string.IsNullOrWhiteSpace(line))
            {
                count++;
            }
        }

        return count;
    }

    public async IAsyncEnumerable<T> ReadAsync<T>(
        string filePath,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await using var stream = File.OpenRead(filePath);
        using var reader = new StreamReader(stream);

        var lineNumber = 0;
        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var line = await reader.ReadLineAsync(cancellationToken);
            if (line is null)
            {
                break;
            }

            lineNumber++;
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            var item = JsonSerializer.Deserialize<T>(line, JsonDefaults.Options);
            if (item is null)
            {
                throw new InvalidDataException($"Unable to deserialize line {lineNumber} in '{filePath}'.");
            }

            yield return item;
        }
    }
}
