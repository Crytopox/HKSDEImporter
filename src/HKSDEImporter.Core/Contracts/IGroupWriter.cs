using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Contracts;

public interface IGroupWriter
{
    Task WriteAsync(string outputPath, IReadOnlyCollection<Group> groups, CancellationToken cancellationToken);
}
