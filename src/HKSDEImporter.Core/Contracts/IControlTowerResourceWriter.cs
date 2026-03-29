using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Contracts;

public interface IControlTowerResourceWriter
{
    Task WriteAsync(string outputPath, IReadOnlyCollection<ControlTowerResource> resources, CancellationToken cancellationToken);
}
