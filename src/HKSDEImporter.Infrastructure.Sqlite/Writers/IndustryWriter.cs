using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Infrastructure.Sqlite.Connections;

namespace HKSDEImporter.Infrastructure.Sqlite.Writers;

public sealed class IndustryWriter : IIndustryWriter
{
    private readonly SqliteConnectionFactory _connectionFactory;

    public IndustryWriter(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task WriteAsync(
        string outputPath,
        IReadOnlyCollection<IndustryBlueprint> blueprints,
        IReadOnlyCollection<IndustryActivity> activities,
        IReadOnlyCollection<IndustryActivityMaterial> materials,
        IReadOnlyCollection<IndustryActivityProduct> products,
        IReadOnlyCollection<IndustryActivityProbability> probabilities,
        IReadOnlyCollection<IndustryActivitySkill> skills,
        CancellationToken cancellationToken)
    {
        await using var connection = _connectionFactory.Create(Path.GetFullPath(outputPath));
        await connection.OpenAsync(cancellationToken);
        using var transaction = connection.BeginTransaction();

        await InsertBlueprintsAsync(connection, transaction, blueprints, cancellationToken);
        await InsertActivitiesAsync(connection, transaction, activities, cancellationToken);
        await InsertMaterialsAsync(connection, transaction, materials, cancellationToken);
        await InsertProductsAsync(connection, transaction, products, cancellationToken);
        await InsertProbabilitiesAsync(connection, transaction, probabilities, cancellationToken);
        await InsertSkillsAsync(connection, transaction, skills, cancellationToken);

        transaction.Commit();
    }

    private static async Task InsertBlueprintsAsync(Microsoft.Data.Sqlite.SqliteConnection connection, Microsoft.Data.Sqlite.SqliteTransaction transaction, IReadOnlyCollection<IndustryBlueprint> rows, CancellationToken cancellationToken)
    {
        using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = "INSERT INTO industryBlueprints (typeID, maxProductionLimit) VALUES ($typeID, $maxProductionLimit);";
        var typeId = command.CreateParameter(); typeId.ParameterName = "$typeID"; command.Parameters.Add(typeId);
        var maxProductionLimit = command.CreateParameter(); maxProductionLimit.ParameterName = "$maxProductionLimit"; command.Parameters.Add(maxProductionLimit);

        foreach (var row in rows)
        {
            typeId.Value = row.TypeId;
            maxProductionLimit.Value = row.MaxProductionLimit;
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    private static async Task InsertActivitiesAsync(Microsoft.Data.Sqlite.SqliteConnection connection, Microsoft.Data.Sqlite.SqliteTransaction transaction, IReadOnlyCollection<IndustryActivity> rows, CancellationToken cancellationToken)
    {
        using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = "INSERT INTO industryActivity (typeID, activityID, time) VALUES ($typeID, $activityID, $time);";
        var typeId = command.CreateParameter(); typeId.ParameterName = "$typeID"; command.Parameters.Add(typeId);
        var activityId = command.CreateParameter(); activityId.ParameterName = "$activityID"; command.Parameters.Add(activityId);
        var time = command.CreateParameter(); time.ParameterName = "$time"; command.Parameters.Add(time);

        foreach (var row in rows)
        {
            typeId.Value = row.TypeId;
            activityId.Value = row.ActivityId;
            time.Value = row.Time;
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    private static async Task InsertMaterialsAsync(Microsoft.Data.Sqlite.SqliteConnection connection, Microsoft.Data.Sqlite.SqliteTransaction transaction, IReadOnlyCollection<IndustryActivityMaterial> rows, CancellationToken cancellationToken)
    {
        using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = "INSERT INTO industryActivityMaterials (typeID, activityID, materialTypeID, quantity) VALUES ($typeID, $activityID, $materialTypeID, $quantity);";
        var typeId = command.CreateParameter(); typeId.ParameterName = "$typeID"; command.Parameters.Add(typeId);
        var activityId = command.CreateParameter(); activityId.ParameterName = "$activityID"; command.Parameters.Add(activityId);
        var materialTypeId = command.CreateParameter(); materialTypeId.ParameterName = "$materialTypeID"; command.Parameters.Add(materialTypeId);
        var quantity = command.CreateParameter(); quantity.ParameterName = "$quantity"; command.Parameters.Add(quantity);

        foreach (var row in rows)
        {
            typeId.Value = row.TypeId;
            activityId.Value = row.ActivityId;
            materialTypeId.Value = row.MaterialTypeId;
            quantity.Value = row.Quantity;
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    private static async Task InsertProductsAsync(Microsoft.Data.Sqlite.SqliteConnection connection, Microsoft.Data.Sqlite.SqliteTransaction transaction, IReadOnlyCollection<IndustryActivityProduct> rows, CancellationToken cancellationToken)
    {
        using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = "INSERT INTO industryActivityProducts (typeID, activityID, productTypeID, quantity) VALUES ($typeID, $activityID, $productTypeID, $quantity);";
        var typeId = command.CreateParameter(); typeId.ParameterName = "$typeID"; command.Parameters.Add(typeId);
        var activityId = command.CreateParameter(); activityId.ParameterName = "$activityID"; command.Parameters.Add(activityId);
        var productTypeId = command.CreateParameter(); productTypeId.ParameterName = "$productTypeID"; command.Parameters.Add(productTypeId);
        var quantity = command.CreateParameter(); quantity.ParameterName = "$quantity"; command.Parameters.Add(quantity);

        foreach (var row in rows)
        {
            typeId.Value = row.TypeId;
            activityId.Value = row.ActivityId;
            productTypeId.Value = row.ProductTypeId;
            quantity.Value = row.Quantity;
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    private static async Task InsertProbabilitiesAsync(Microsoft.Data.Sqlite.SqliteConnection connection, Microsoft.Data.Sqlite.SqliteTransaction transaction, IReadOnlyCollection<IndustryActivityProbability> rows, CancellationToken cancellationToken)
    {
        using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = "INSERT INTO industryActivityProbabilities (typeID, activityID, productTypeID, probability) VALUES ($typeID, $activityID, $productTypeID, $probability);";
        var typeId = command.CreateParameter(); typeId.ParameterName = "$typeID"; command.Parameters.Add(typeId);
        var activityId = command.CreateParameter(); activityId.ParameterName = "$activityID"; command.Parameters.Add(activityId);
        var productTypeId = command.CreateParameter(); productTypeId.ParameterName = "$productTypeID"; command.Parameters.Add(productTypeId);
        var probability = command.CreateParameter(); probability.ParameterName = "$probability"; command.Parameters.Add(probability);

        foreach (var row in rows)
        {
            typeId.Value = row.TypeId;
            activityId.Value = row.ActivityId;
            productTypeId.Value = row.ProductTypeId;
            probability.Value = row.Probability;
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    private static async Task InsertSkillsAsync(Microsoft.Data.Sqlite.SqliteConnection connection, Microsoft.Data.Sqlite.SqliteTransaction transaction, IReadOnlyCollection<IndustryActivitySkill> rows, CancellationToken cancellationToken)
    {
        using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = "INSERT INTO industryActivitySkills (typeID, activityID, skillID, level) VALUES ($typeID, $activityID, $skillID, $level);";
        var typeId = command.CreateParameter(); typeId.ParameterName = "$typeID"; command.Parameters.Add(typeId);
        var activityId = command.CreateParameter(); activityId.ParameterName = "$activityID"; command.Parameters.Add(activityId);
        var skillId = command.CreateParameter(); skillId.ParameterName = "$skillID"; command.Parameters.Add(skillId);
        var level = command.CreateParameter(); level.ParameterName = "$level"; command.Parameters.Add(level);

        foreach (var row in rows)
        {
            typeId.Value = row.TypeId;
            activityId.Value = row.ActivityId;
            skillId.Value = row.SkillId;
            level.Value = row.Level;
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }
}
