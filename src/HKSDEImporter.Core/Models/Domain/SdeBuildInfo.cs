namespace HKSDEImporter.Core.Models.Domain;

public sealed record SdeBuildInfo(string SourceKey, int BuildNumber, DateTimeOffset ReleaseDateUtc);
