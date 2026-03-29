using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Models.Raw;

namespace HKSDEImporter.Core.Mapping;

public sealed class TypeMaterialMapper
{
    public IReadOnlyList<TypeMaterial> Map(RawTypeMaterial raw)
    {
        var output = new List<TypeMaterial>(raw.Materials.Count);
        foreach (var material in raw.Materials)
        {
            output.Add(new TypeMaterial(raw.TypeId, material.MaterialTypeId, material.Quantity));
        }

        return output;
    }
}
