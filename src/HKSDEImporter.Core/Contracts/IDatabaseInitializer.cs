using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Contracts;

public interface IDatabaseInitializer
{
    Task InitializeAsync(string outputPath, bool overwrite, CancellationToken cancellationToken);
}
