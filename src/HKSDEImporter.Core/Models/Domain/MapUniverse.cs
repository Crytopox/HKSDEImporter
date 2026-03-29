namespace HKSDEImporter.Core.Models.Domain;

public sealed record MapUniverse(
    int UniverseId,
    string UniverseName,
    double? X,
    double? Y,
    double? Z,
    double? XMin,
    double? XMax,
    double? YMin,
    double? YMax,
    double? ZMin,
    double? ZMax,
    double? Radius);
