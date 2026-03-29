namespace HKSDEImporter.Core.Models.Raw;

public sealed class RawCloneGrade
{
    public int CloneGradeId { get; init; }
    public string? Name { get; init; }
    public List<RawCloneGradeSkill> Skills { get; init; } = [];
}

public sealed class RawCloneGradeSkill
{
    public int TypeId { get; init; }
    public int Level { get; init; }
}

public sealed class RawCompressibleType
{
    public int TypeId { get; init; }
    public int CompressedTypeId { get; init; }
}

public sealed class RawDbuffCollection
{
    public int CollectionId { get; init; }
    public string? AggregateMode { get; init; }
    public string? OperationName { get; init; }
    public string? ShowOutputValueInUi { get; init; }
    public string? DeveloperDescription { get; init; }
    public string? DisplayNameEn { get; init; }
    public string RawJson { get; init; } = "{}";
}

public sealed class RawDynamicItemAttribute
{
    public int TypeId { get; init; }
    public string RawJson { get; init; } = "{}";
}

public sealed class RawFreelanceJobSchema
{
    public int SchemaId { get; init; }
    public string RawJson { get; init; } = "{}";
}

public sealed class RawMercenaryTacticalOperation
{
    public int OperationId { get; init; }
    public string? NameEn { get; init; }
    public string? DescriptionEn { get; init; }
    public int? AnarchyImpact { get; init; }
    public int? DevelopmentImpact { get; init; }
    public int? InfomorphBonus { get; init; }
    public string RawJson { get; init; } = "{}";
}

public sealed class RawPlanetResource
{
    public int TypeId { get; init; }
    public int? Power { get; init; }
    public int? Workforce { get; init; }
    public string RawJson { get; init; } = "{}";
}

public sealed class RawSovereigntyUpgrade
{
    public int TypeId { get; init; }
    public string? MutuallyExclusiveGroup { get; init; }
    public int? PowerAllocation { get; init; }
    public int? WorkforceAllocation { get; init; }
    public int? FuelTypeId { get; init; }
    public int? FuelHourlyUpkeep { get; init; }
    public int? FuelStartupCost { get; init; }
    public string RawJson { get; init; } = "{}";
}

public sealed class RawTypeBonus
{
    public int TypeId { get; init; }
    public string RawJson { get; init; } = "{}";
}
