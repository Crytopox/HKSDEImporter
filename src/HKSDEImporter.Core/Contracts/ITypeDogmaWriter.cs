using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Contracts;

public interface ITypeDogmaWriter
{
    Task WriteAsync(
        string outputPath,
        IReadOnlyCollection<TypeDogmaAttribute> attributes,
        IReadOnlyCollection<TypeDogmaEffect> effects,
        CancellationToken cancellationToken);
}
