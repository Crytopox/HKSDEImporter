using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Mapping;
using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Models.Raw;
using HKSDEImporter.Core.Validation;

namespace HKSDEImporter.Core.ImportSteps;

public sealed class ImportCorporationReferenceStep : IImportStep
{
    private readonly IRawSdeReader _reader;
    private readonly IMapper<RawCorporationActivity, CorporationActivity> _activityMapper;
    private readonly IMapper<RawNpcCorporationDivision, NpcCorporationDivision> _divisionMapper;
    private readonly IMapper<RawNpcCorporation, NpcCorporation> _corporationMapper;
    private readonly IValidator<CorporationActivity> _activityValidator;
    private readonly IValidator<NpcCorporationDivision> _divisionValidator;
    private readonly IValidator<NpcCorporation> _corporationValidator;
    private readonly ICorporationReferenceWriter _writer;
    private readonly IImportObserver _observer;

    public ImportCorporationReferenceStep(
        IRawSdeReader reader,
        IMapper<RawCorporationActivity, CorporationActivity> activityMapper,
        IMapper<RawNpcCorporationDivision, NpcCorporationDivision> divisionMapper,
        IMapper<RawNpcCorporation, NpcCorporation> corporationMapper,
        IValidator<CorporationActivity> activityValidator,
        IValidator<NpcCorporationDivision> divisionValidator,
        IValidator<NpcCorporation> corporationValidator,
        ICorporationReferenceWriter writer,
        IImportObserver observer)
    {
        _reader = reader;
        _activityMapper = activityMapper;
        _divisionMapper = divisionMapper;
        _corporationMapper = corporationMapper;
        _activityValidator = activityValidator;
        _divisionValidator = divisionValidator;
        _corporationValidator = corporationValidator;
        _writer = writer;
        _observer = observer;
    }

    public string Name => "Import Corporation Reference";

    public async Task ExecuteAsync(ImportContext context, CancellationToken cancellationToken)
    {
        var total =
            await _reader.CountCorporationActivitiesAsync(context.ResolvedInputDirectory!, cancellationToken) +
            await _reader.CountNpcCorporationDivisionsAsync(context.ResolvedInputDirectory!, cancellationToken) +
            await _reader.CountNpcCorporationsAsync(context.ResolvedInputDirectory!, cancellationToken);

        _observer.OnStepStarted(Name, total);

        var activities = new List<CorporationActivity>();
        var divisions = new List<NpcCorporationDivision>();
        var corporations = new List<NpcCorporation>();

        var seenActivities = new HashSet<int>();
        var seenDivisions = new HashSet<int>();
        var seenCorporations = new HashSet<int>();

        var processed = 0L;

        await foreach (var raw in _reader.ReadCorporationActivitiesAsync(context.ResolvedInputDirectory!, cancellationToken))
        {
            processed++;
            if (processed % 1000 == 0) _observer.OnStepProgress(Name, processed);

            var mapped = _activityMapper.Map(raw);
            if (!seenActivities.Add(mapped.ActivityId))
            {
                AddWarning(context, $"Duplicate corporation activity id {mapped.ActivityId}. Keeping first occurrence.");
                continue;
            }

            var result = _activityValidator.Validate(mapped);
            if (!result.IsValid)
            {
                AddWarning(context, result.Message ?? "Invalid corporation activity row.");
                continue;
            }

            activities.Add(mapped);
        }

        await foreach (var raw in _reader.ReadNpcCorporationDivisionsAsync(context.ResolvedInputDirectory!, cancellationToken))
        {
            processed++;
            if (processed % 1000 == 0) _observer.OnStepProgress(Name, processed);

            var mapped = _divisionMapper.Map(raw);
            if (!seenDivisions.Add(mapped.DivisionId))
            {
                AddWarning(context, $"Duplicate NPC division id {mapped.DivisionId}. Keeping first occurrence.");
                continue;
            }

            var result = _divisionValidator.Validate(mapped);
            if (!result.IsValid)
            {
                AddWarning(context, result.Message ?? "Invalid NPC division row.");
                continue;
            }

            divisions.Add(mapped);
        }

        await foreach (var raw in _reader.ReadNpcCorporationsAsync(context.ResolvedInputDirectory!, cancellationToken))
        {
            processed++;
            if (processed % 1000 == 0) _observer.OnStepProgress(Name, processed);

            var mapped = _corporationMapper.Map(raw);
            if (!seenCorporations.Add(mapped.CorporationId))
            {
                AddWarning(context, $"Duplicate NPC corporation id {mapped.CorporationId}. Keeping first occurrence.");
                continue;
            }

            var result = _corporationValidator.Validate(mapped);
            if (!result.IsValid)
            {
                AddWarning(context, result.Message ?? "Invalid NPC corporation row.");
                continue;
            }

            corporations.Add(mapped);
        }

        await _writer.WriteAsync(context.Options.OutputPath, activities, divisions, corporations, cancellationToken);

        context.SetRowCount("crpActivities", activities.Count);
        context.SetRowCount("crpNPCDivisions", divisions.Count);
        context.SetRowCount("crpNPCCorporations", corporations.Count);
        _observer.OnStepProgress(Name, processed);
    }

    private void AddWarning(ImportContext context, string warning)
    {
        context.AddWarning(warning);
        _observer.OnWarning(warning);
    }
}
