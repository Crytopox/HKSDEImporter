using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Models.Raw;

namespace HKSDEImporter.Core.Mapping;

public sealed class PlanetSchematicsMapper
{
    public PlanetSchematic MapSchematic(RawPlanetSchematic raw)
    {
        return new PlanetSchematic(raw.SchematicId, raw.Name?.Trim() ?? string.Empty, raw.CycleTime);
    }

    public IReadOnlyList<PlanetSchematicPinMap> MapPinMaps(RawPlanetSchematic raw)
    {
        return raw.PinTypeIds.Select(pinTypeId => new PlanetSchematicPinMap(raw.SchematicId, pinTypeId)).ToList();
    }

    public IReadOnlyList<PlanetSchematicTypeMap> MapTypeMaps(RawPlanetSchematic raw)
    {
        return raw.Types
            .Select(type => new PlanetSchematicTypeMap(raw.SchematicId, type.TypeId, type.Quantity, type.IsInput))
            .ToList();
    }
}
