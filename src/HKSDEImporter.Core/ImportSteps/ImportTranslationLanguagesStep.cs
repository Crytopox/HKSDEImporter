using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Mapping;
using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Models.Raw;
using HKSDEImporter.Core.Validation;

namespace HKSDEImporter.Core.ImportSteps;

public sealed class ImportTranslationLanguagesStep : IImportStep
{
    private readonly IRawSdeReader _reader;
    private readonly IMapper<RawTranslationLanguage, TranslationLanguage> _mapper;
    private readonly IValidator<TranslationLanguage> _validator;
    private readonly ITranslationLanguageWriter _writer;
    private readonly IImportObserver _observer;

    public ImportTranslationLanguagesStep(IRawSdeReader reader, IMapper<RawTranslationLanguage, TranslationLanguage> mapper, IValidator<TranslationLanguage> validator, ITranslationLanguageWriter writer, IImportObserver observer)
    {
        _reader = reader;
        _mapper = mapper;
        _validator = validator;
        _writer = writer;
        _observer = observer;
    }

    public string Name => "Import Translation Languages";

    public async Task ExecuteAsync(ImportContext context, CancellationToken cancellationToken)
    {
        var total = await _reader.CountTranslationLanguagesAsync(context.ResolvedInputDirectory!, cancellationToken);
        _observer.OnStepStarted(Name, total);

        var rows = new List<TranslationLanguage>();
        var seen = new HashSet<int>();
        var processed = 0L;

        await foreach (var raw in _reader.ReadTranslationLanguagesAsync(context.ResolvedInputDirectory!, cancellationToken))
        {
            processed++;
            var mapped = _mapper.Map(raw);
            if (!seen.Add(mapped.NumericLanguageId))
            {
                continue;
            }

            var result = _validator.Validate(mapped);
            if (result.IsValid)
            {
                rows.Add(mapped);
            }
            else
            {
                var warning = result.Message ?? "Invalid translation language row.";
                context.AddWarning(warning);
                _observer.OnWarning(warning);
            }
        }

        await _writer.WriteAsync(context.Options.OutputPath, rows, cancellationToken);
        context.SetRowCount("trnTranslationLanguages", rows.Count);
        _observer.OnStepProgress(Name, processed);
    }
}
