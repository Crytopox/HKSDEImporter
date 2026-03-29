namespace HKSDEImporter.Core.Models.Domain;

public sealed record MapLandmark(
    int LandmarkId,
    string LandmarkName,
    string? Description,
    int? IconId,
    double? X,
    double? Y,
    double? Z);
