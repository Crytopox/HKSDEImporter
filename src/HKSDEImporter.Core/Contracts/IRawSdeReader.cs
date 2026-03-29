using HKSDEImporter.Core.Models.Raw;

namespace HKSDEImporter.Core.Contracts;

public interface IRawSdeReader
{
    Task<long> CountCategoriesAsync(string rootDirectory, CancellationToken cancellationToken);
    Task<long> CountGroupsAsync(string rootDirectory, CancellationToken cancellationToken);
    Task<long> CountTypesAsync(string rootDirectory, CancellationToken cancellationToken);

    IAsyncEnumerable<RawCategory> ReadCategoriesAsync(string rootDirectory, CancellationToken cancellationToken);
    IAsyncEnumerable<RawGroup> ReadGroupsAsync(string rootDirectory, CancellationToken cancellationToken);
    IAsyncEnumerable<RawType> ReadTypesAsync(string rootDirectory, CancellationToken cancellationToken);
}
