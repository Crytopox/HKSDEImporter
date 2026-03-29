using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Contracts;

public interface ICategoryWriter
{
    Task WriteAsync(string outputPath, IReadOnlyCollection<Category> categories, CancellationToken cancellationToken);
}
