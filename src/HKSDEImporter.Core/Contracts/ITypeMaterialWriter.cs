using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Contracts;

public interface ITypeMaterialWriter
{
    Task WriteAsync(string outputPath, IReadOnlyCollection<TypeMaterial> materials, CancellationToken cancellationToken);
}
