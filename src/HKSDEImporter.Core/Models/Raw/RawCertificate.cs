namespace HKSDEImporter.Core.Models.Raw;

public sealed class RawCertificate
{
    public int CertId { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public int? GroupId { get; init; }
    public List<RawCertificateSkill>? Skills { get; init; }
}

public sealed class RawCertificateSkill
{
    public int SkillId { get; init; }
    public int? Basic { get; init; }
    public int? Standard { get; init; }
    public int? Improved { get; init; }
    public int? Advanced { get; init; }
    public int? Elite { get; init; }
}
