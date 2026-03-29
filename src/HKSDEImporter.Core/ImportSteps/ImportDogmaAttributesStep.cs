using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Mapping;
using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Models.Raw;
using HKSDEImporter.Core.Validation;

namespace HKSDEImporter.Core.ImportSteps;

public sealed class ImportDogmaAttributesStep : IImportStep
{
    private readonly IRawSdeReader _reader;
    private readonly IMapper<RawDogmaAttribute, DogmaAttributeType> _mapper;
    private readonly IValidator<DogmaAttributeType> _validator;
    private readonly IDogmaAttributeWriter _writer;
    private readonly IImportObserver _observer;

    public ImportDogmaAttributesStep(IRawSdeReader reader, IMapper<RawDogmaAttribute, DogmaAttributeType> mapper, IValidator<DogmaAttributeType> validator, IDogmaAttributeWriter writer, IImportObserver observer)
    {
        _reader = reader;
        _mapper = mapper;
        _validator = validator;
        _writer = writer;
        _observer = observer;
    }

    public string Name => "Import Dogma Attributes";

    public async Task ExecuteAsync(ImportContext context, CancellationToken cancellationToken)
    {
        var total = await _reader.CountDogmaAttributesAsync(context.ResolvedInputDirectory!, cancellationToken);
        _observer.OnStepStarted(Name, total);

        var items = new List<DogmaAttributeType>();
        var seen = new HashSet<int>();
        var processed = 0L;

        await foreach (var raw in _reader.ReadDogmaAttributesAsync(context.ResolvedInputDirectory!, cancellationToken))
        {
            processed++;
            if (processed % 1000 == 0) _observer.OnStepProgress(Name, processed);
            var mapped = _mapper.Map(raw);
            if (!seen.Add(mapped.AttributeId))
            {
                var warning = $"Duplicate dogma attribute id {mapped.AttributeId}. Keeping first occurrence.";
                context.AddWarning(warning);
                _observer.OnWarning(warning);
                continue;
            }

            var result = _validator.Validate(mapped);
            if (!result.IsValid)
            {
                var warning = result.Message ?? "Invalid dogma attribute row.";
                context.AddWarning(warning);
                _observer.OnWarning(warning);
                continue;
            }

            items.Add(mapped);
        }

        await _writer.WriteAsync(context.Options.OutputPath, items, cancellationToken);
        context.SetRowCount("dgmAttributeTypes", items.Count);
        _observer.OnStepProgress(Name, processed);
    }
}
