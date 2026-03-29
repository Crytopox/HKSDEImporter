using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Contracts;

public interface ITranslationLanguageWriter
{
    Task WriteAsync(string outputPath, IReadOnlyCollection<TranslationLanguage> languages, CancellationToken cancellationToken);
}
