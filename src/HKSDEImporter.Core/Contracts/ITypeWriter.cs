using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Contracts;

public interface ITypeWriter
{
    Task WriteAsync(string outputPath, IReadOnlyCollection<TypeItem> types, CancellationToken cancellationToken);
}
