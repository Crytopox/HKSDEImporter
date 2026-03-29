using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Mapping;
using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Validation;

namespace HKSDEImporter.Core.ImportSteps;

public sealed class ImportCertificatesStep : IImportStep
{
    private readonly IRawSdeReader _reader;
    private readonly CertificateMapper _mapper;
    private readonly IValidator<Cert> _certValidator;
    private readonly IValidator<CertMastery> _masteryValidator;
    private readonly IValidator<CertSkill> _skillValidator;
    private readonly ICertificateWriter _writer;
    private readonly IImportObserver _observer;

    public ImportCertificatesStep(IRawSdeReader reader, CertificateMapper mapper, IValidator<Cert> certValidator, IValidator<CertMastery> masteryValidator, IValidator<CertSkill> skillValidator, ICertificateWriter writer, IImportObserver observer)
    {
        _reader = reader;
        _mapper = mapper;
        _certValidator = certValidator;
        _masteryValidator = masteryValidator;
        _skillValidator = skillValidator;
        _writer = writer;
        _observer = observer;
    }

    public string Name => "Import Certificates";

    public async Task ExecuteAsync(ImportContext context, CancellationToken cancellationToken)
    {
        var total = await _reader.CountCertificatesAsync(context.ResolvedInputDirectory!, cancellationToken)
                    + await _reader.CountMasteriesAsync(context.ResolvedInputDirectory!, cancellationToken);
        _observer.OnStepStarted(Name, total);

        var certs = new List<Cert>();
        var masteries = new List<CertMastery>();
        var skills = new List<CertSkill>();

        var seenCert = new HashSet<int>();
        var seenMastery = new HashSet<string>(StringComparer.Ordinal);
        var seenSkill = new HashSet<string>(StringComparer.Ordinal);

        var processed = 0L;

        await foreach (var raw in _reader.ReadCertificatesAsync(context.ResolvedInputDirectory!, cancellationToken))
        {
            processed++;
            if (processed % 1000 == 0) _observer.OnStepProgress(Name, processed);

            var cert = _mapper.MapCert(raw);
            if (seenCert.Add(cert.CertId))
            {
                var certResult = _certValidator.Validate(cert);
                if (certResult.IsValid) certs.Add(cert);
                else AddWarning(context, certResult.Message ?? "Invalid certificate row.");
            }

            foreach (var skill in _mapper.MapSkills(raw))
            {
                var key = $"{skill.CertId}:{skill.SkillId}:{skill.CertLevelInt}";
                if (!seenSkill.Add(key)) continue;

                var skillResult = _skillValidator.Validate(skill);
                if (skillResult.IsValid) skills.Add(skill);
                else AddWarning(context, skillResult.Message ?? "Invalid certificate skill row.");
            }
        }

        await foreach (var raw in _reader.ReadMasteriesAsync(context.ResolvedInputDirectory!, cancellationToken))
        {
            processed++;
            if (processed % 1000 == 0) _observer.OnStepProgress(Name, processed);

            foreach (var mastery in _mapper.MapMasteries(raw))
            {
                var key = $"{mastery.TypeId}:{mastery.MasteryLevel}:{mastery.CertId}";
                if (!seenMastery.Add(key)) continue;

                var masteryResult = _masteryValidator.Validate(mastery);
                if (masteryResult.IsValid) masteries.Add(mastery);
                else AddWarning(context, masteryResult.Message ?? "Invalid mastery row.");
            }
        }

        await _writer.WriteAsync(context.Options.OutputPath, certs, masteries, skills, cancellationToken);

        context.SetRowCount("certCerts", certs.Count);
        context.SetRowCount("certMasteries", masteries.Count);
        context.SetRowCount("certSkills", skills.Count);
        _observer.OnStepProgress(Name, processed);
    }

    private void AddWarning(ImportContext context, string warning)
    {
        context.AddWarning(warning);
        _observer.OnWarning(warning);
    }
}
