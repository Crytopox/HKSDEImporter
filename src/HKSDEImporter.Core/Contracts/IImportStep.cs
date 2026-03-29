using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Contracts;

public interface IImportStep
{
    string Name { get; }
    Task ExecuteAsync(ImportContext context, CancellationToken cancellationToken);
}
