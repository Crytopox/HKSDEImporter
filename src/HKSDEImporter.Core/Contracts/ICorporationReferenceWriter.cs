using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Contracts;

public interface ICorporationReferenceWriter
{
    Task WriteAsync(
        string outputPath,
        IReadOnlyCollection<CorporationActivity> activities,
        IReadOnlyCollection<NpcCorporationDivision> divisions,
        IReadOnlyCollection<NpcCorporation> corporations,
        CancellationToken cancellationToken);
}
