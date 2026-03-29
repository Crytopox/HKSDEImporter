using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Contracts;

public interface IMetaGroupWriter
{
    Task WriteAsync(string outputPath, IReadOnlyCollection<MetaGroup> metaGroups, CancellationToken cancellationToken);
}
