using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Validation;

public sealed class MapLandmarkValidator : IValidator<MapLandmark>
{
    public ValidationResult Validate(MapLandmark value)
    {
        if (value.LandmarkId <= 0)
        {
            return ValidationResult.Invalid($"LandmarkId must be greater than 0, got {value.LandmarkId}.");
        }

        if (string.IsNullOrWhiteSpace(value.LandmarkName))
        {
            return ValidationResult.Invalid($"Landmark {value.LandmarkId} has an empty English name.");
        }

        return ValidationResult.Valid();
    }
}
