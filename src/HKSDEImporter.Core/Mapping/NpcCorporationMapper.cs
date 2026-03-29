using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Models.Raw;

namespace HKSDEImporter.Core.Mapping;

public sealed class NpcCorporationMapper : IMapper<RawNpcCorporation, NpcCorporation>
{
    public NpcCorporation Map(RawNpcCorporation raw)
    {
        var investors = raw.Investors ?? [];

        int? InvestorIdAt(int index) => investors.Count > index ? investors[index].Key : null;
        int? InvestorSharesAt(int index) => investors.Count > index ? investors[index].Value : null;

        return new NpcCorporation(
            raw.Key,
            raw.Size?.Trim(),
            raw.Extent?.Trim(),
            raw.SolarSystemId,
            InvestorIdAt(0),
            InvestorSharesAt(0),
            InvestorIdAt(1),
            InvestorSharesAt(1),
            InvestorIdAt(2),
            InvestorSharesAt(2),
            InvestorIdAt(3),
            InvestorSharesAt(3),
            raw.FriendId,
            raw.EnemyId,
            raw.Shares,
            raw.InitialPrice,
            raw.MinSecurity,
            null,
            null,
            null,
            null,
            null,
            raw.FactionId,
            raw.SizeFactor,
            null,
            null,
            raw.Description?.En?.Trim(),
            raw.IconId);
    }
}
