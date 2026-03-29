namespace HKSDEImporter.Core.Models.Domain;

public sealed record ContrabandTypeRule(
    int FactionId,
    int TypeId,
    double? StandingLoss,
    double? ConfiscateMinSec,
    double? FineByValue,
    double? AttackMinSec);
