using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Mapping;
using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Models.Raw;
using HKSDEImporter.Core.Validation;

namespace HKSDEImporter.Core.ImportSteps;

public sealed class ImportCategoriesStep : IImportStep
{
    private readonly IRawSdeReader _reader;
    private readonly IMapper<RawCategory, Category> _mapper;
    private readonly IValidator<Category> _validator;
    private readonly ICategoryWriter _writer;
    private readonly IImportObserver _observer;

    public ImportCategoriesStep(
        IRawSdeReader reader,
        IMapper<RawCategory, Category> mapper,
        IValidator<Category> validator,
        ICategoryWriter writer,
        IImportObserver observer)
    {
        _reader = reader;
        _mapper = mapper;
        _validator = validator;
        _writer = writer;
        _observer = observer;
    }

    public string Name => "Import Categories";

    public async Task ExecuteAsync(ImportContext context, CancellationToken cancellationToken)
    {
        var totalCount = await _reader.CountCategoriesAsync(context.ResolvedInputDirectory!, cancellationToken);
        _observer.OnStepStarted(Name, totalCount);

        var categories = new List<Category>();
        var seenIds = new HashSet<int>();
        var processed = 0L;

        await foreach (var raw in _reader.ReadCategoriesAsync(context.ResolvedInputDirectory!, cancellationToken))
        {
            processed++;
            if (processed % 500 == 0)
            {
                _observer.OnStepProgress(Name, processed);
            }

            var mapped = _mapper.Map(raw);
            if (!seenIds.Add(mapped.CategoryId))
            {
                var warning = $"Duplicate category id {mapped.CategoryId}. Keeping first occurrence.";
                context.AddWarning(warning);
                _observer.OnWarning(warning);
                continue;
            }

            var result = _validator.Validate(mapped);
            if (!result.IsValid)
            {
                var warning = result.Message ?? "Invalid category row.";
                context.AddWarning(warning);
                _observer.OnWarning(warning);
                continue;
            }

            categories.Add(mapped);
        }

        await _writer.WriteAsync(context.Options.OutputPath, categories, cancellationToken);
        context.SetRowCount("categories", categories.Count);
        _observer.OnStepProgress(Name, processed);
    }
}
