using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Models.Raw;

namespace HKSDEImporter.Core.Mapping;

public sealed class CertificateMapper
{
    private static readonly (string Name, int LevelInt, Func<RawCertificateSkill, int?> Selector)[] Levels =
    [
        ("Basic", 0, s => s.Basic),
        ("Standard", 1, s => s.Standard),
        ("Improved", 2, s => s.Improved),
        ("Advanced", 3, s => s.Advanced),
        ("Elite", 4, s => s.Elite)
    ];

    public Cert MapCert(RawCertificate raw)
        => new(raw.CertId, raw.Description?.Trim(), raw.GroupId, raw.Name?.Trim());

    public IEnumerable<CertSkill> MapSkills(RawCertificate raw)
    {
        foreach (var skill in raw.Skills ?? [])
        {
            foreach (var level in Levels)
            {
                var skillLevel = level.Selector(skill);
                if (!skillLevel.HasValue)
                {
                    continue;
                }

                yield return new CertSkill(raw.CertId, skill.SkillId, level.LevelInt, skillLevel.Value, level.Name);
            }
        }
    }

    public IEnumerable<CertMastery> MapMasteries(RawMastery raw)
    {
        foreach (var level in raw.Levels ?? [])
        {
            foreach (var certId in level.CertIds ?? [])
            {
                yield return new CertMastery(raw.TypeId, level.MasteryLevel, certId);
            }
        }
    }
}
