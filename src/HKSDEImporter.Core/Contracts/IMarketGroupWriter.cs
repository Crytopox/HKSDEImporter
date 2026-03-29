using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Contracts;

public interface IMarketGroupWriter
{
    Task WriteAsync(string outputPath, IReadOnlyCollection<MarketGroup> marketGroups, CancellationToken cancellationToken);
}
