namespace HKSDEImporter.Core.Models.Domain;

public sealed class ValidationResult
{
    private ValidationResult(bool isValid, string? message)
    {
        IsValid = isValid;
        Message = message;
    }

    public bool IsValid { get; }
    public string? Message { get; }

    public static ValidationResult Valid() => new(true, null);

    public static ValidationResult Invalid(string message) => new(false, message);
}
