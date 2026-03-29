using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Models.Raw;

namespace HKSDEImporter.Infrastructure.Json.Readers;

public sealed class JsonLinesSdeReader : IRawSdeReader
{
    private readonly JsonLinesFileReader _fileReader = new();

    public Task<long> CountCategoriesAsync(string rootDirectory, CancellationToken cancellationToken)
    {
        var path = FileSystem.SdeFileLocator.LocateRequiredFile(rootDirectory, "categories.jsonl");
        return _fileReader.CountLinesAsync(path, cancellationToken);
    }

    public Task<long> CountGroupsAsync(string rootDirectory, CancellationToken cancellationToken)
    {
        var path = FileSystem.SdeFileLocator.LocateRequiredFile(rootDirectory, "groups.jsonl");
        return _fileReader.CountLinesAsync(path, cancellationToken);
    }

    public Task<long> CountTypesAsync(string rootDirectory, CancellationToken cancellationToken)
    {
        var path = FileSystem.SdeFileLocator.LocateRequiredFile(rootDirectory, "types.jsonl");
        return _fileReader.CountLinesAsync(path, cancellationToken);
    }

    public async IAsyncEnumerable<RawCategory> ReadCategoriesAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = FileSystem.SdeFileLocator.LocateRequiredFile(rootDirectory, "categories.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonlCategoryDto>(path, cancellationToken))
        {
            yield return new RawCategory
            {
                Key = item.Key,
                IconId = item.IconId,
                Name = item.Name is null ? null : new RawLocalizedText { En = item.Name.En },
                Published = item.Published
            };
        }
    }

    public async IAsyncEnumerable<RawGroup> ReadGroupsAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = FileSystem.SdeFileLocator.LocateRequiredFile(rootDirectory, "groups.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonlGroupDto>(path, cancellationToken))
        {
            yield return new RawGroup
            {
                Key = item.Key,
                CategoryId = item.CategoryId,
                Name = item.Name is null ? null : new RawLocalizedText { En = item.Name.En },
                Published = item.Published
            };
        }
    }

    public async IAsyncEnumerable<RawType> ReadTypesAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = FileSystem.SdeFileLocator.LocateRequiredFile(rootDirectory, "types.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonlTypeDto>(path, cancellationToken))
        {
            yield return new RawType
            {
                Key = item.Key,
                GroupId = item.GroupId,
                Description = item.Description is null ? null : new RawLocalizedText { En = item.Description.En },
                IconId = item.IconId,
                Mass = item.Mass,
                Name = item.Name is null ? null : new RawLocalizedText { En = item.Name.En },
                PortionSize = item.PortionSize,
                Published = item.Published,
                Radius = item.Radius,
                Volume = item.Volume
            };
        }
    }
}
