using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Contracts;

public interface ICertificateWriter
{
    Task WriteAsync(string outputPath, IReadOnlyCollection<Cert> certs, IReadOnlyCollection<CertMastery> masteries, IReadOnlyCollection<CertSkill> skills, CancellationToken cancellationToken);
}
