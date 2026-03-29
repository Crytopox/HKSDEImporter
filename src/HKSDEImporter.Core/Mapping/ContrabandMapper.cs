using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Models.Raw;

namespace HKSDEImporter.Core.Mapping;

public sealed class ContrabandMapper
{
    public IEnumerable<ContrabandTypeRule> Map(RawContrabandType raw)
    {
        foreach (var faction in raw.Factions ?? [])
        {
            yield return new ContrabandTypeRule(
                faction.FactionId,
                raw.TypeId,
                faction.StandingLoss,
                faction.ConfiscateMinSec,
                faction.FineByValue,
                faction.AttackMinSec);
        }
    }
}
