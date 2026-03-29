using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Validation;

public sealed class TranslationLanguageValidator : IValidator<TranslationLanguage>
{
    public ValidationResult Validate(TranslationLanguage value)
    {
        if (string.IsNullOrWhiteSpace(value.LanguageId))
        {
            return ValidationResult.Invalid("Translation language id cannot be empty.");
        }

        return value.NumericLanguageId > 0
            ? ValidationResult.Valid()
            : ValidationResult.Invalid($"Numeric language id must be greater than 0 for {value.LanguageId}.");
    }
}
