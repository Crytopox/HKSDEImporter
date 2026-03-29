namespace HKSDEImporter.Core.Models.Domain;

public sealed record CloneGrade(int CloneGradeId, string? Name);
public sealed record CloneGradeSkill(int CloneGradeId, int TypeId, int Level);
public sealed record CompressibleType(int TypeId, int CompressedTypeId);
public sealed record DbuffCollection(int CollectionId, string? AggregateMode, string? OperationName, string? ShowOutputValueInUi, string? DeveloperDescription, string? DisplayNameEn, string RawJson);
public sealed record DynamicItemAttribute(int TypeId, string RawJson);
public sealed record FreelanceJobSchema(int SchemaId, string RawJson);
public sealed record MercenaryTacticalOperation(int OperationId, string? NameEn, string? DescriptionEn, int? AnarchyImpact, int? DevelopmentImpact, int? InfomorphBonus, string RawJson);
public sealed record PlanetResource(int TypeId, int? Power, int? Workforce, string RawJson);
public sealed record SovereigntyUpgrade(int TypeId, string? MutuallyExclusiveGroup, int? PowerAllocation, int? WorkforceAllocation, int? FuelTypeId, int? FuelHourlyUpkeep, int? FuelStartupCost, string RawJson);
public sealed record TypeBonus(int TypeId, string RawJson);
public sealed record ControlTowerResourcePurpose(int Purpose, string? PurposeText);
public sealed record MetaType(int TypeId, int? ParentTypeId, int? MetaGroupId);
public sealed record NpcCorporationDivisionLink(int CorporationId, int DivisionId, int? Size);
