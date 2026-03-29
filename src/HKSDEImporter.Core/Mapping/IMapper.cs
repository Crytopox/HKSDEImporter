using HKSDEImporter.Core.Models.Raw;

namespace HKSDEImporter.Core.Mapping;

public interface IMapper<in TRaw, out TDomain>
{
    TDomain Map(TRaw raw);
}
