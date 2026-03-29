using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Contracts;

public interface IDogmaAttributeWriter
{
    Task WriteAsync(string outputPath, IReadOnlyCollection<DogmaAttributeType> attributes, CancellationToken cancellationToken);
}
