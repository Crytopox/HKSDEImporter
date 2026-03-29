namespace HKSDEImporter.Core.Models.Raw;

public sealed class RawSdeBuildInfo
{
    public string? SourceKey { get; init; }
    public int? BuildNumber { get; init; }
    public DateTimeOffset? ReleaseDateUtc { get; init; }
}
