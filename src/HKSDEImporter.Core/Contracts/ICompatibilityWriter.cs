using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Contracts;

public interface ICompatibilityWriter
{
    Task WriteAsync(
        string outputPath,
        IReadOnlyCollection<MetaType> metaTypes,
        IReadOnlyCollection<NpcCorporationDivisionLink> corporationDivisionLinks,
        CancellationToken cancellationToken);
}
