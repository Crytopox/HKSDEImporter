using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Contracts;

public interface IDogmaAttributeCategoryWriter
{
    Task WriteAsync(string outputPath, IReadOnlyCollection<DogmaAttributeCategory> categories, CancellationToken cancellationToken);
}
