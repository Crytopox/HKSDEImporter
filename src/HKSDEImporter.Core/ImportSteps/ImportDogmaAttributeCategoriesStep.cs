using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Mapping;
using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Models.Raw;
using HKSDEImporter.Core.Validation;

namespace HKSDEImporter.Core.ImportSteps;

public sealed class ImportDogmaAttributeCategoriesStep : IImportStep
{
    private readonly IRawSdeReader _reader;
    private readonly IMapper<RawDogmaAttributeCategory, DogmaAttributeCategory> _mapper;
    private readonly IValidator<DogmaAttributeCategory> _validator;
    private readonly IDogmaAttributeCategoryWriter _writer;
    private readonly IImportObserver _observer;

    public ImportDogmaAttributeCategoriesStep(IRawSdeReader reader, IMapper<RawDogmaAttributeCategory, DogmaAttributeCategory> mapper, IValidator<DogmaAttributeCategory> validator, IDogmaAttributeCategoryWriter writer, IImportObserver observer)
    {
        _reader = reader;
        _mapper = mapper;
        _validator = validator;
        _writer = writer;
        _observer = observer;
    }

    public string Name => "Import Dogma Attribute Categories";

    public async Task ExecuteAsync(ImportContext context, CancellationToken cancellationToken)
    {
        var total = await _reader.CountDogmaAttributeCategoriesAsync(context.ResolvedInputDirectory!, cancellationToken);
        _observer.OnStepStarted(Name, total);

        var items = new List<DogmaAttributeCategory>();
        var seen = new HashSet<int>();
        var processed = 0L;

        await foreach (var raw in _reader.ReadDogmaAttributeCategoriesAsync(context.ResolvedInputDirectory!, cancellationToken))
        {
            processed++;
            if (processed % 200 == 0) _observer.OnStepProgress(Name, processed);
            var mapped = _mapper.Map(raw);
            if (!seen.Add(mapped.CategoryId))
            {
                var warning = $"Duplicate dogma attribute category id {mapped.CategoryId}. Keeping first occurrence.";
                context.AddWarning(warning);
                _observer.OnWarning(warning);
                continue;
            }

            var result = _validator.Validate(mapped);
            if (!result.IsValid)
            {
                var warning = result.Message ?? "Invalid dogma attribute category row.";
                context.AddWarning(warning);
                _observer.OnWarning(warning);
                continue;
            }

            items.Add(mapped);
        }

        await _writer.WriteAsync(context.Options.OutputPath, items, cancellationToken);
        context.SetRowCount("dgmAttributeCategories", items.Count);
        _observer.OnStepProgress(Name, processed);
    }
}
