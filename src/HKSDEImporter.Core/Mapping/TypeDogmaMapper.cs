using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Models.Raw;

namespace HKSDEImporter.Core.Mapping;

public sealed class TypeDogmaMapper
{
    public IReadOnlyList<TypeDogmaAttribute> MapAttributes(RawTypeDogma raw)
    {
        var output = new List<TypeDogmaAttribute>(raw.Attributes.Count);
        foreach (var attribute in raw.Attributes)
        {
            var isWholeNumber = Math.Abs(attribute.Value - Math.Truncate(attribute.Value)) < 0.000001;
            var inIntRange = attribute.Value >= int.MinValue && attribute.Value <= int.MaxValue;
            var intValue = isWholeNumber && inIntRange
                ? (int?)Convert.ToInt32(attribute.Value)
                : null;
            double? floatValue = intValue.HasValue ? null : attribute.Value;

            output.Add(new TypeDogmaAttribute(raw.TypeId, attribute.AttributeId, intValue, floatValue));
        }

        return output;
    }

    public IReadOnlyList<TypeDogmaEffect> MapEffects(RawTypeDogma raw)
    {
        var output = new List<TypeDogmaEffect>(raw.Effects.Count);
        foreach (var effect in raw.Effects)
        {
            output.Add(new TypeDogmaEffect(raw.TypeId, effect.EffectId, effect.IsDefault));
        }

        return output;
    }
}
