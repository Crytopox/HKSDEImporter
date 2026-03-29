using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Mapping;
using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Models.Raw;
using HKSDEImporter.Core.Validation;

namespace HKSDEImporter.Core.ImportSteps;

public sealed class ImportDogmaEffectsStep : IImportStep
{
    private readonly IRawSdeReader _reader;
    private readonly IMapper<RawDogmaEffect, DogmaEffect> _mapper;
    private readonly IValidator<DogmaEffect> _validator;
    private readonly IDogmaEffectWriter _writer;
    private readonly IImportObserver _observer;

    public ImportDogmaEffectsStep(IRawSdeReader reader, IMapper<RawDogmaEffect, DogmaEffect> mapper, IValidator<DogmaEffect> validator, IDogmaEffectWriter writer, IImportObserver observer)
    {
        _reader = reader;
        _mapper = mapper;
        _validator = validator;
        _writer = writer;
        _observer = observer;
    }

    public string Name => "Import Dogma Effects";

    public async Task ExecuteAsync(ImportContext context, CancellationToken cancellationToken)
    {
        var total = await _reader.CountDogmaEffectsAsync(context.ResolvedInputDirectory!, cancellationToken);
        _observer.OnStepStarted(Name, total);

        var items = new List<DogmaEffect>();
        var seen = new HashSet<int>();
        var processed = 0L;

        await foreach (var raw in _reader.ReadDogmaEffectsAsync(context.ResolvedInputDirectory!, cancellationToken))
        {
            processed++;
            if (processed % 1000 == 0) _observer.OnStepProgress(Name, processed);
            var mapped = _mapper.Map(raw);
            if (!seen.Add(mapped.EffectId))
            {
                var warning = $"Duplicate dogma effect id {mapped.EffectId}. Keeping first occurrence.";
                context.AddWarning(warning);
                _observer.OnWarning(warning);
                continue;
            }

            var result = _validator.Validate(mapped);
            if (!result.IsValid)
            {
                var warning = result.Message ?? "Invalid dogma effect row.";
                context.AddWarning(warning);
                _observer.OnWarning(warning);
                continue;
            }

            items.Add(mapped);
        }

        await _writer.WriteAsync(context.Options.OutputPath, items, cancellationToken);
        context.SetRowCount("dgmEffects", items.Count);
        _observer.OnStepProgress(Name, processed);
    }
}
