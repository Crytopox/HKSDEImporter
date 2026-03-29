using HKSDEImporter.Core.Contracts;

namespace HKSDEImporter.Infrastructure.Json.FileSystem;

public sealed class LocalDirectorySdeSourceProvider : ISdeSourceProvider
{
    private readonly string _directoryPath;

    public LocalDirectorySdeSourceProvider(string directoryPath)
    {
        _directoryPath = directoryPath;
    }

    public Task<SdeSourceHandle> PrepareAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (!Directory.Exists(_directoryPath))
        {
            throw new DirectoryNotFoundException($"Input directory was not found: '{_directoryPath}'.");
        }

        var fullPath = Path.GetFullPath(_directoryPath);
        return Task.FromResult<SdeSourceHandle>(new LocalSdeSourceHandle(fullPath));
    }

    private sealed class LocalSdeSourceHandle(string rootDirectory) : SdeSourceHandle(rootDirectory)
    {
        public override ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }
}
