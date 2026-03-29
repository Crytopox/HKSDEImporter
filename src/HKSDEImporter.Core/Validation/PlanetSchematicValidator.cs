using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Validation;

public sealed class PlanetSchematicValidator : IValidator<PlanetSchematic>
{
    public ValidationResult Validate(PlanetSchematic value)
    {
        if (value.SchematicId <= 0)
        {
            return ValidationResult.Invalid($"Schematic id must be > 0, got {value.SchematicId}.");
        }

        if (string.IsNullOrWhiteSpace(value.SchematicName))
        {
            return ValidationResult.Invalid($"Schematic {value.SchematicId} has empty name.");
        }

        if (value.CycleTime < 0)
        {
            return ValidationResult.Invalid($"Schematic {value.SchematicId} has invalid cycleTime {value.CycleTime}.");
        }

        return ValidationResult.Valid();
    }
}
