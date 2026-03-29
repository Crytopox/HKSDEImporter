using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Infrastructure.Sqlite.Connections;

namespace HKSDEImporter.Infrastructure.Sqlite.Writers;

public sealed class SupplementalDataWriter : ISupplementalDataWriter
{
    private readonly SqliteConnectionFactory _connectionFactory;

    public SupplementalDataWriter(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task WriteAsync(
        string outputPath,
        IReadOnlyCollection<CloneGrade> cloneGrades,
        IReadOnlyCollection<CloneGradeSkill> cloneGradeSkills,
        IReadOnlyCollection<CompressibleType> compressibleTypes,
        IReadOnlyCollection<DbuffCollection> dbuffCollections,
        IReadOnlyCollection<DynamicItemAttribute> dynamicItemAttributes,
        IReadOnlyCollection<FreelanceJobSchema> freelanceJobSchemas,
        IReadOnlyCollection<MercenaryTacticalOperation> mercenaryTacticalOperations,
        IReadOnlyCollection<PlanetResource> planetResources,
        IReadOnlyCollection<SovereigntyUpgrade> sovereigntyUpgrades,
        IReadOnlyCollection<TypeBonus> typeBonuses,
        IReadOnlyCollection<ControlTowerResourcePurpose> controlTowerResourcePurposes,
        CancellationToken cancellationToken)
    {
        await using var connection = _connectionFactory.Create(Path.GetFullPath(outputPath));
        await connection.OpenAsync(cancellationToken);
        using var transaction = connection.BeginTransaction();

        await InsertCloneGradesAsync(connection, transaction, cloneGrades, cancellationToken);
        await InsertCloneGradeSkillsAsync(connection, transaction, cloneGradeSkills, cancellationToken);
        await InsertCompressibleTypesAsync(connection, transaction, compressibleTypes, cancellationToken);
        await InsertDbuffCollectionsAsync(connection, transaction, dbuffCollections, cancellationToken);
        await InsertDynamicItemAttributesAsync(connection, transaction, dynamicItemAttributes, cancellationToken);
        await InsertFreelanceJobSchemasAsync(connection, transaction, freelanceJobSchemas, cancellationToken);
        await InsertMercenaryTacticalOperationsAsync(connection, transaction, mercenaryTacticalOperations, cancellationToken);
        await InsertPlanetResourcesAsync(connection, transaction, planetResources, cancellationToken);
        await InsertSovereigntyUpgradesAsync(connection, transaction, sovereigntyUpgrades, cancellationToken);
        await InsertTypeBonusesAsync(connection, transaction, typeBonuses, cancellationToken);
        await InsertControlTowerResourcePurposesAsync(connection, transaction, controlTowerResourcePurposes, cancellationToken);

        transaction.Commit();
    }

    private static async Task InsertCloneGradesAsync(Microsoft.Data.Sqlite.SqliteConnection connection, Microsoft.Data.Sqlite.SqliteTransaction transaction, IReadOnlyCollection<CloneGrade> rows, CancellationToken cancellationToken)
    {
        using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = "INSERT INTO chrCloneGrades (cloneGradeID, cloneGradeName) VALUES ($id, $name);";
        var id = command.CreateParameter(); id.ParameterName = "$id"; command.Parameters.Add(id);
        var name = command.CreateParameter(); name.ParameterName = "$name"; command.Parameters.Add(name);

        foreach (var row in rows)
        {
            id.Value = row.CloneGradeId;
            name.Value = row.Name is null ? DBNull.Value : row.Name;
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    private static async Task InsertCloneGradeSkillsAsync(Microsoft.Data.Sqlite.SqliteConnection connection, Microsoft.Data.Sqlite.SqliteTransaction transaction, IReadOnlyCollection<CloneGradeSkill> rows, CancellationToken cancellationToken)
    {
        using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = "INSERT INTO chrCloneGradeSkills (cloneGradeID, typeID, level) VALUES ($cloneGradeId, $typeId, $level);";
        var cloneGradeId = command.CreateParameter(); cloneGradeId.ParameterName = "$cloneGradeId"; command.Parameters.Add(cloneGradeId);
        var typeId = command.CreateParameter(); typeId.ParameterName = "$typeId"; command.Parameters.Add(typeId);
        var level = command.CreateParameter(); level.ParameterName = "$level"; command.Parameters.Add(level);

        foreach (var row in rows)
        {
            cloneGradeId.Value = row.CloneGradeId;
            typeId.Value = row.TypeId;
            level.Value = row.Level;
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    private static async Task InsertCompressibleTypesAsync(Microsoft.Data.Sqlite.SqliteConnection connection, Microsoft.Data.Sqlite.SqliteTransaction transaction, IReadOnlyCollection<CompressibleType> rows, CancellationToken cancellationToken)
    {
        using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = "INSERT INTO invCompressibleTypes (typeID, compressedTypeID) VALUES ($typeId, $compressedTypeId);";
        var typeId = command.CreateParameter(); typeId.ParameterName = "$typeId"; command.Parameters.Add(typeId);
        var compressedTypeId = command.CreateParameter(); compressedTypeId.ParameterName = "$compressedTypeId"; command.Parameters.Add(compressedTypeId);

        foreach (var row in rows)
        {
            typeId.Value = row.TypeId;
            compressedTypeId.Value = row.CompressedTypeId;
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    private static async Task InsertDbuffCollectionsAsync(Microsoft.Data.Sqlite.SqliteConnection connection, Microsoft.Data.Sqlite.SqliteTransaction transaction, IReadOnlyCollection<DbuffCollection> rows, CancellationToken cancellationToken)
    {
        using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = """
            INSERT INTO dgmBuffCollections (collectionID, aggregateMode, operationName, showOutputValueInUI, developerDescription, displayName, rawJson)
            VALUES ($id, $aggregateMode, $operationName, $showOutputValueInUI, $developerDescription, $displayName, $rawJson);
            """;
        var id = command.CreateParameter(); id.ParameterName = "$id"; command.Parameters.Add(id);
        var aggregateMode = command.CreateParameter(); aggregateMode.ParameterName = "$aggregateMode"; command.Parameters.Add(aggregateMode);
        var operationName = command.CreateParameter(); operationName.ParameterName = "$operationName"; command.Parameters.Add(operationName);
        var showOutputValueInUi = command.CreateParameter(); showOutputValueInUi.ParameterName = "$showOutputValueInUI"; command.Parameters.Add(showOutputValueInUi);
        var developerDescription = command.CreateParameter(); developerDescription.ParameterName = "$developerDescription"; command.Parameters.Add(developerDescription);
        var displayName = command.CreateParameter(); displayName.ParameterName = "$displayName"; command.Parameters.Add(displayName);
        var rawJson = command.CreateParameter(); rawJson.ParameterName = "$rawJson"; command.Parameters.Add(rawJson);

        foreach (var row in rows)
        {
            id.Value = row.CollectionId;
            aggregateMode.Value = row.AggregateMode is null ? DBNull.Value : row.AggregateMode;
            operationName.Value = row.OperationName is null ? DBNull.Value : row.OperationName;
            showOutputValueInUi.Value = row.ShowOutputValueInUi is null ? DBNull.Value : row.ShowOutputValueInUi;
            developerDescription.Value = row.DeveloperDescription is null ? DBNull.Value : row.DeveloperDescription;
            displayName.Value = row.DisplayNameEn is null ? DBNull.Value : row.DisplayNameEn;
            rawJson.Value = row.RawJson;
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    private static async Task InsertDynamicItemAttributesAsync(Microsoft.Data.Sqlite.SqliteConnection connection, Microsoft.Data.Sqlite.SqliteTransaction transaction, IReadOnlyCollection<DynamicItemAttribute> rows, CancellationToken cancellationToken)
    {
        using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = "INSERT INTO dgmDynamicItemAttributes (typeID, rawJson) VALUES ($typeId, $rawJson);";
        var typeId = command.CreateParameter(); typeId.ParameterName = "$typeId"; command.Parameters.Add(typeId);
        var rawJson = command.CreateParameter(); rawJson.ParameterName = "$rawJson"; command.Parameters.Add(rawJson);

        foreach (var row in rows)
        {
            typeId.Value = row.TypeId;
            rawJson.Value = row.RawJson;
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    private static async Task InsertFreelanceJobSchemasAsync(Microsoft.Data.Sqlite.SqliteConnection connection, Microsoft.Data.Sqlite.SqliteTransaction transaction, IReadOnlyCollection<FreelanceJobSchema> rows, CancellationToken cancellationToken)
    {
        using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = "INSERT INTO frtFreelanceJobSchemas (schemaID, rawJson) VALUES ($schemaId, $rawJson);";
        var schemaId = command.CreateParameter(); schemaId.ParameterName = "$schemaId"; command.Parameters.Add(schemaId);
        var rawJson = command.CreateParameter(); rawJson.ParameterName = "$rawJson"; command.Parameters.Add(rawJson);

        foreach (var row in rows)
        {
            schemaId.Value = row.SchemaId;
            rawJson.Value = row.RawJson;
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    private static async Task InsertMercenaryTacticalOperationsAsync(Microsoft.Data.Sqlite.SqliteConnection connection, Microsoft.Data.Sqlite.SqliteTransaction transaction, IReadOnlyCollection<MercenaryTacticalOperation> rows, CancellationToken cancellationToken)
    {
        using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = """
            INSERT INTO mercenaryTacticalOperations (operationID, operationName, description, anarchyImpact, developmentImpact, infomorphBonus, rawJson)
            VALUES ($id, $name, $description, $anarchy, $development, $infomorph, $rawJson);
            """;
        var id = command.CreateParameter(); id.ParameterName = "$id"; command.Parameters.Add(id);
        var name = command.CreateParameter(); name.ParameterName = "$name"; command.Parameters.Add(name);
        var description = command.CreateParameter(); description.ParameterName = "$description"; command.Parameters.Add(description);
        var anarchy = command.CreateParameter(); anarchy.ParameterName = "$anarchy"; command.Parameters.Add(anarchy);
        var development = command.CreateParameter(); development.ParameterName = "$development"; command.Parameters.Add(development);
        var infomorph = command.CreateParameter(); infomorph.ParameterName = "$infomorph"; command.Parameters.Add(infomorph);
        var rawJson = command.CreateParameter(); rawJson.ParameterName = "$rawJson"; command.Parameters.Add(rawJson);

        foreach (var row in rows)
        {
            id.Value = row.OperationId;
            name.Value = row.NameEn is null ? DBNull.Value : row.NameEn;
            description.Value = row.DescriptionEn is null ? DBNull.Value : row.DescriptionEn;
            anarchy.Value = row.AnarchyImpact.HasValue ? row.AnarchyImpact.Value : DBNull.Value;
            development.Value = row.DevelopmentImpact.HasValue ? row.DevelopmentImpact.Value : DBNull.Value;
            infomorph.Value = row.InfomorphBonus.HasValue ? row.InfomorphBonus.Value : DBNull.Value;
            rawJson.Value = row.RawJson;
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    private static async Task InsertPlanetResourcesAsync(Microsoft.Data.Sqlite.SqliteConnection connection, Microsoft.Data.Sqlite.SqliteTransaction transaction, IReadOnlyCollection<PlanetResource> rows, CancellationToken cancellationToken)
    {
        using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = "INSERT INTO planetResources (typeID, power, workforce, rawJson) VALUES ($typeId, $power, $workforce, $rawJson);";
        var typeId = command.CreateParameter(); typeId.ParameterName = "$typeId"; command.Parameters.Add(typeId);
        var power = command.CreateParameter(); power.ParameterName = "$power"; command.Parameters.Add(power);
        var workforce = command.CreateParameter(); workforce.ParameterName = "$workforce"; command.Parameters.Add(workforce);
        var rawJson = command.CreateParameter(); rawJson.ParameterName = "$rawJson"; command.Parameters.Add(rawJson);

        foreach (var row in rows)
        {
            typeId.Value = row.TypeId;
            power.Value = row.Power.HasValue ? row.Power.Value : DBNull.Value;
            workforce.Value = row.Workforce.HasValue ? row.Workforce.Value : DBNull.Value;
            rawJson.Value = row.RawJson;
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    private static async Task InsertSovereigntyUpgradesAsync(Microsoft.Data.Sqlite.SqliteConnection connection, Microsoft.Data.Sqlite.SqliteTransaction transaction, IReadOnlyCollection<SovereigntyUpgrade> rows, CancellationToken cancellationToken)
    {
        using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = """
            INSERT INTO sovSovereigntyUpgrades (
                typeID, mutuallyExclusiveGroup, powerAllocation, workforceAllocation, fuelTypeID, fuelHourlyUpkeep, fuelStartupCost, rawJson)
            VALUES ($typeId, $group, $power, $workforce, $fuelTypeId, $fuelHourlyUpkeep, $fuelStartupCost, $rawJson);
            """;
        var typeId = command.CreateParameter(); typeId.ParameterName = "$typeId"; command.Parameters.Add(typeId);
        var group = command.CreateParameter(); group.ParameterName = "$group"; command.Parameters.Add(group);
        var power = command.CreateParameter(); power.ParameterName = "$power"; command.Parameters.Add(power);
        var workforce = command.CreateParameter(); workforce.ParameterName = "$workforce"; command.Parameters.Add(workforce);
        var fuelTypeId = command.CreateParameter(); fuelTypeId.ParameterName = "$fuelTypeId"; command.Parameters.Add(fuelTypeId);
        var fuelHourlyUpkeep = command.CreateParameter(); fuelHourlyUpkeep.ParameterName = "$fuelHourlyUpkeep"; command.Parameters.Add(fuelHourlyUpkeep);
        var fuelStartupCost = command.CreateParameter(); fuelStartupCost.ParameterName = "$fuelStartupCost"; command.Parameters.Add(fuelStartupCost);
        var rawJson = command.CreateParameter(); rawJson.ParameterName = "$rawJson"; command.Parameters.Add(rawJson);

        foreach (var row in rows)
        {
            typeId.Value = row.TypeId;
            group.Value = row.MutuallyExclusiveGroup is null ? DBNull.Value : row.MutuallyExclusiveGroup;
            power.Value = row.PowerAllocation.HasValue ? row.PowerAllocation.Value : DBNull.Value;
            workforce.Value = row.WorkforceAllocation.HasValue ? row.WorkforceAllocation.Value : DBNull.Value;
            fuelTypeId.Value = row.FuelTypeId.HasValue ? row.FuelTypeId.Value : DBNull.Value;
            fuelHourlyUpkeep.Value = row.FuelHourlyUpkeep.HasValue ? row.FuelHourlyUpkeep.Value : DBNull.Value;
            fuelStartupCost.Value = row.FuelStartupCost.HasValue ? row.FuelStartupCost.Value : DBNull.Value;
            rawJson.Value = row.RawJson;
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    private static async Task InsertTypeBonusesAsync(Microsoft.Data.Sqlite.SqliteConnection connection, Microsoft.Data.Sqlite.SqliteTransaction transaction, IReadOnlyCollection<TypeBonus> rows, CancellationToken cancellationToken)
    {
        using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = "INSERT INTO invTraits (typeID, rawJson) VALUES ($typeId, $rawJson);";
        var typeId = command.CreateParameter(); typeId.ParameterName = "$typeId"; command.Parameters.Add(typeId);
        var rawJson = command.CreateParameter(); rawJson.ParameterName = "$rawJson"; command.Parameters.Add(rawJson);

        foreach (var row in rows)
        {
            typeId.Value = row.TypeId;
            rawJson.Value = row.RawJson;
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    private static async Task InsertControlTowerResourcePurposesAsync(Microsoft.Data.Sqlite.SqliteConnection connection, Microsoft.Data.Sqlite.SqliteTransaction transaction, IReadOnlyCollection<ControlTowerResourcePurpose> rows, CancellationToken cancellationToken)
    {
        using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = "INSERT INTO invControlTowerResourcePurposes (purpose, purposeText) VALUES ($purpose, $purposeText);";
        var purpose = command.CreateParameter(); purpose.ParameterName = "$purpose"; command.Parameters.Add(purpose);
        var purposeText = command.CreateParameter(); purposeText.ParameterName = "$purposeText"; command.Parameters.Add(purposeText);

        foreach (var row in rows)
        {
            purpose.Value = row.Purpose;
            purposeText.Value = row.PurposeText is null ? DBNull.Value : row.PurposeText;
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }
}
