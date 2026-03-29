using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Contracts;

public interface IContrabandWriter
{
    Task WriteAsync(string outputPath, IReadOnlyCollection<ContrabandTypeRule> rules, CancellationToken cancellationToken);
}
