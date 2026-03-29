namespace HKSDEImporter.Core.Contracts;

public abstract class SdeSourceHandle : IAsyncDisposable
{
    protected SdeSourceHandle(string rootDirectory)
    {
        RootDirectory = rootDirectory;
    }

    public string RootDirectory { get; }

    public abstract ValueTask DisposeAsync();
}
