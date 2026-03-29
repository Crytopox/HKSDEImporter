namespace HKSDEImporter.Core.Contracts;

public interface ISdeSourceProvider
{
    Task<SdeSourceHandle> PrepareAsync(CancellationToken cancellationToken);
}
