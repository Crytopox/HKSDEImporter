using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Mapping;
using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Models.Raw;
using HKSDEImporter.Core.Validation;

namespace HKSDEImporter.Core.ImportSteps;

public sealed class ImportCharacterReferenceStep : IImportStep
{
    private readonly IRawSdeReader _reader;
    private readonly IMapper<RawFaction, Faction> _factionMapper;
    private readonly IMapper<RawRace, Race> _raceMapper;
    private readonly IMapper<RawBloodline, Bloodline> _bloodlineMapper;
    private readonly IMapper<RawAncestry, Ancestry> _ancestryMapper;
    private readonly IMapper<RawCharacterAttribute, CharacterAttribute> _attributeMapper;
    private readonly IValidator<Faction> _factionValidator;
    private readonly IValidator<Race> _raceValidator;
    private readonly IValidator<Bloodline> _bloodlineValidator;
    private readonly IValidator<Ancestry> _ancestryValidator;
    private readonly IValidator<CharacterAttribute> _attributeValidator;
    private readonly ICharacterReferenceWriter _writer;
    private readonly IImportObserver _observer;

    public ImportCharacterReferenceStep(
        IRawSdeReader reader,
        IMapper<RawFaction, Faction> factionMapper,
        IMapper<RawRace, Race> raceMapper,
        IMapper<RawBloodline, Bloodline> bloodlineMapper,
        IMapper<RawAncestry, Ancestry> ancestryMapper,
        IMapper<RawCharacterAttribute, CharacterAttribute> attributeMapper,
        IValidator<Faction> factionValidator,
        IValidator<Race> raceValidator,
        IValidator<Bloodline> bloodlineValidator,
        IValidator<Ancestry> ancestryValidator,
        IValidator<CharacterAttribute> attributeValidator,
        ICharacterReferenceWriter writer,
        IImportObserver observer)
    {
        _reader = reader;
        _factionMapper = factionMapper;
        _raceMapper = raceMapper;
        _bloodlineMapper = bloodlineMapper;
        _ancestryMapper = ancestryMapper;
        _attributeMapper = attributeMapper;
        _factionValidator = factionValidator;
        _raceValidator = raceValidator;
        _bloodlineValidator = bloodlineValidator;
        _ancestryValidator = ancestryValidator;
        _attributeValidator = attributeValidator;
        _writer = writer;
        _observer = observer;
    }

    public string Name => "Import Character Reference";

    public async Task ExecuteAsync(ImportContext context, CancellationToken cancellationToken)
    {
        var total =
            await _reader.CountFactionsAsync(context.ResolvedInputDirectory!, cancellationToken) +
            await _reader.CountRacesAsync(context.ResolvedInputDirectory!, cancellationToken) +
            await _reader.CountBloodlinesAsync(context.ResolvedInputDirectory!, cancellationToken) +
            await _reader.CountAncestriesAsync(context.ResolvedInputDirectory!, cancellationToken) +
            await _reader.CountCharacterAttributesAsync(context.ResolvedInputDirectory!, cancellationToken);

        _observer.OnStepStarted(Name, total);

        var factions = new List<Faction>();
        var races = new List<Race>();
        var bloodlines = new List<Bloodline>();
        var ancestries = new List<Ancestry>();
        var attributes = new List<CharacterAttribute>();

        var seenFactions = new HashSet<int>();
        var seenRaces = new HashSet<int>();
        var seenBloodlines = new HashSet<int>();
        var seenAncestries = new HashSet<int>();
        var seenAttributes = new HashSet<int>();

        var processed = 0L;

        await foreach (var raw in _reader.ReadFactionsAsync(context.ResolvedInputDirectory!, cancellationToken))
        {
            processed++;
            if (processed % 500 == 0) _observer.OnStepProgress(Name, processed);

            var mapped = _factionMapper.Map(raw);
            if (!seenFactions.Add(mapped.FactionId))
            {
                AddWarning(context, $"Duplicate faction id {mapped.FactionId}. Keeping first occurrence.");
                continue;
            }

            var result = _factionValidator.Validate(mapped);
            if (!result.IsValid)
            {
                AddWarning(context, result.Message ?? "Invalid faction row.");
                continue;
            }

            factions.Add(mapped);
        }

        await foreach (var raw in _reader.ReadRacesAsync(context.ResolvedInputDirectory!, cancellationToken))
        {
            processed++;
            if (processed % 500 == 0) _observer.OnStepProgress(Name, processed);

            var mapped = _raceMapper.Map(raw);
            if (!seenRaces.Add(mapped.RaceId))
            {
                AddWarning(context, $"Duplicate race id {mapped.RaceId}. Keeping first occurrence.");
                continue;
            }

            var result = _raceValidator.Validate(mapped);
            if (!result.IsValid)
            {
                AddWarning(context, result.Message ?? "Invalid race row.");
                continue;
            }

            races.Add(mapped);
        }

        await foreach (var raw in _reader.ReadBloodlinesAsync(context.ResolvedInputDirectory!, cancellationToken))
        {
            processed++;
            if (processed % 500 == 0) _observer.OnStepProgress(Name, processed);

            var mapped = _bloodlineMapper.Map(raw);
            if (!seenBloodlines.Add(mapped.BloodlineId))
            {
                AddWarning(context, $"Duplicate bloodline id {mapped.BloodlineId}. Keeping first occurrence.");
                continue;
            }

            var result = _bloodlineValidator.Validate(mapped);
            if (!result.IsValid)
            {
                AddWarning(context, result.Message ?? "Invalid bloodline row.");
                continue;
            }

            bloodlines.Add(mapped);
        }

        await foreach (var raw in _reader.ReadAncestriesAsync(context.ResolvedInputDirectory!, cancellationToken))
        {
            processed++;
            if (processed % 500 == 0) _observer.OnStepProgress(Name, processed);

            var mapped = _ancestryMapper.Map(raw);
            if (!seenAncestries.Add(mapped.AncestryId))
            {
                AddWarning(context, $"Duplicate ancestry id {mapped.AncestryId}. Keeping first occurrence.");
                continue;
            }

            var result = _ancestryValidator.Validate(mapped);
            if (!result.IsValid)
            {
                AddWarning(context, result.Message ?? "Invalid ancestry row.");
                continue;
            }

            ancestries.Add(mapped);
        }

        await foreach (var raw in _reader.ReadCharacterAttributesAsync(context.ResolvedInputDirectory!, cancellationToken))
        {
            processed++;
            if (processed % 500 == 0) _observer.OnStepProgress(Name, processed);

            var mapped = _attributeMapper.Map(raw);
            if (!seenAttributes.Add(mapped.AttributeId))
            {
                AddWarning(context, $"Duplicate character attribute id {mapped.AttributeId}. Keeping first occurrence.");
                continue;
            }

            var result = _attributeValidator.Validate(mapped);
            if (!result.IsValid)
            {
                AddWarning(context, result.Message ?? "Invalid character attribute row.");
                continue;
            }

            attributes.Add(mapped);
        }

        await _writer.WriteAsync(context.Options.OutputPath, factions, races, bloodlines, ancestries, attributes, cancellationToken);

        context.SetRowCount("chrFactions", factions.Count);
        context.SetRowCount("chrRaces", races.Count);
        context.SetRowCount("chrBloodlines", bloodlines.Count);
        context.SetRowCount("chrAncestries", ancestries.Count);
        context.SetRowCount("chrAttributes", attributes.Count);
        _observer.OnStepProgress(Name, processed);
    }

    private void AddWarning(ImportContext context, string warning)
    {
        context.AddWarning(warning);
        _observer.OnWarning(warning);
    }
}
