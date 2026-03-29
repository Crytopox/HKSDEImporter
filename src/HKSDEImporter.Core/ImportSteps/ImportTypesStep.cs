using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Mapping;
using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Models.Raw;
using HKSDEImporter.Core.Validation;

namespace HKSDEImporter.Core.ImportSteps;

public sealed class ImportTypesStep : IImportStep
{
    private readonly IRawSdeReader _reader;
    private readonly IMapper<RawType, TypeItem> _mapper;
    private readonly IValidator<TypeItem> _validator;
    private readonly ITypeWriter _writer;
    private readonly IImportObserver _observer;

    public ImportTypesStep(
        IRawSdeReader reader,
        IMapper<RawType, TypeItem> mapper,
        IValidator<TypeItem> validator,
        ITypeWriter writer,
        IImportObserver observer)
    {
        _reader = reader;
        _mapper = mapper;
        _validator = validator;
        _writer = writer;
        _observer = observer;
    }

    public string Name => "Import Types";

    public async Task ExecuteAsync(ImportContext context, CancellationToken cancellationToken)
    {
        var totalCount = await _reader.CountTypesAsync(context.ResolvedInputDirectory!, cancellationToken);
        _observer.OnStepStarted(Name, totalCount);

        var types = new List<TypeItem>();
        var seenIds = new HashSet<int>();
        var processed = 0L;

        await foreach (var raw in _reader.ReadTypesAsync(context.ResolvedInputDirectory!, cancellationToken))
        {
            processed++;
            if (processed % 5000 == 0)
            {
                _observer.OnStepProgress(Name, processed);
            }

            var mapped = _mapper.Map(raw);
            if (!seenIds.Add(mapped.TypeId))
            {
                var warning = $"Duplicate type id {mapped.TypeId}. Keeping first occurrence.";
                context.AddWarning(warning);
                _observer.OnWarning(warning);
                continue;
            }

            var result = _validator.Validate(mapped);
            if (!result.IsValid)
            {
                var warning = result.Message ?? "Invalid type row.";
                context.AddWarning(warning);
                _observer.OnWarning(warning);
                continue;
            }

            types.Add(mapped);
        }

        await _writer.WriteAsync(context.Options.OutputPath, types, cancellationToken);
        context.SetRowCount("types", types.Count);
        _observer.OnStepProgress(Name, processed);
    }
}
