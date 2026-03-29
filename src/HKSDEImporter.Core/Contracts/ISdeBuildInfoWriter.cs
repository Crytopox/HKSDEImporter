using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Contracts;

public interface ISdeBuildInfoWriter
{
    Task WriteAsync(string outputPath, IReadOnlyCollection<SdeBuildInfo> rows, CancellationToken cancellationToken);
}
