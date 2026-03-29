using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Infrastructure.Sqlite.Connections;

namespace HKSDEImporter.Infrastructure.Sqlite.Writers;

public sealed class DogmaEffectWriter : IDogmaEffectWriter
{
    private readonly SqliteConnectionFactory _connectionFactory;

    public DogmaEffectWriter(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task WriteAsync(string outputPath, IReadOnlyCollection<DogmaEffect> effects, CancellationToken cancellationToken)
    {
        await using var connection = _connectionFactory.Create(Path.GetFullPath(outputPath));
        await connection.OpenAsync(cancellationToken);
        using var transaction = connection.BeginTransaction();

        using var command = connection.CreateCommand();
        command.CommandText =
            "INSERT INTO dgmEffects (effectID, effectName, effectCategory, description, guid, iconID, isOffensive, isAssistance, durationAttributeID, dischargeAttributeID, rangeAttributeID, disallowAutoRepeat, published, displayName, isWarpSafe, rangeChance, electronicChance, propulsionChance, distribution, modifierInfo) " +
            "VALUES ($effectID, $effectName, $effectCategory, $description, $guid, $iconID, $isOffensive, $isAssistance, $durationAttributeID, $dischargeAttributeID, $rangeAttributeID, $disallowAutoRepeat, $published, $displayName, $isWarpSafe, $rangeChance, $electronicChance, $propulsionChance, $distribution, $modifierInfo);";
        command.Transaction = transaction;

        var effectId = command.CreateParameter(); effectId.ParameterName = "$effectID"; command.Parameters.Add(effectId);
        var effectName = command.CreateParameter(); effectName.ParameterName = "$effectName"; command.Parameters.Add(effectName);
        var effectCategory = command.CreateParameter(); effectCategory.ParameterName = "$effectCategory"; command.Parameters.Add(effectCategory);
        var description = command.CreateParameter(); description.ParameterName = "$description"; command.Parameters.Add(description);
        var guid = command.CreateParameter(); guid.ParameterName = "$guid"; command.Parameters.Add(guid);
        var iconId = command.CreateParameter(); iconId.ParameterName = "$iconID"; command.Parameters.Add(iconId);
        var isOffensive = command.CreateParameter(); isOffensive.ParameterName = "$isOffensive"; command.Parameters.Add(isOffensive);
        var isAssistance = command.CreateParameter(); isAssistance.ParameterName = "$isAssistance"; command.Parameters.Add(isAssistance);
        var durationAttributeId = command.CreateParameter(); durationAttributeId.ParameterName = "$durationAttributeID"; command.Parameters.Add(durationAttributeId);
        var dischargeAttributeId = command.CreateParameter(); dischargeAttributeId.ParameterName = "$dischargeAttributeID"; command.Parameters.Add(dischargeAttributeId);
        var rangeAttributeId = command.CreateParameter(); rangeAttributeId.ParameterName = "$rangeAttributeID"; command.Parameters.Add(rangeAttributeId);
        var disallowAutoRepeat = command.CreateParameter(); disallowAutoRepeat.ParameterName = "$disallowAutoRepeat"; command.Parameters.Add(disallowAutoRepeat);
        var published = command.CreateParameter(); published.ParameterName = "$published"; command.Parameters.Add(published);
        var displayName = command.CreateParameter(); displayName.ParameterName = "$displayName"; command.Parameters.Add(displayName);
        var isWarpSafe = command.CreateParameter(); isWarpSafe.ParameterName = "$isWarpSafe"; command.Parameters.Add(isWarpSafe);
        var rangeChance = command.CreateParameter(); rangeChance.ParameterName = "$rangeChance"; command.Parameters.Add(rangeChance);
        var electronicChance = command.CreateParameter(); electronicChance.ParameterName = "$electronicChance"; command.Parameters.Add(electronicChance);
        var propulsionChance = command.CreateParameter(); propulsionChance.ParameterName = "$propulsionChance"; command.Parameters.Add(propulsionChance);
        var distribution = command.CreateParameter(); distribution.ParameterName = "$distribution"; command.Parameters.Add(distribution);
        var modifierInfo = command.CreateParameter(); modifierInfo.ParameterName = "$modifierInfo"; command.Parameters.Add(modifierInfo);

        foreach (var item in effects)
        {
            effectId.Value = item.EffectId;
            effectName.Value = item.EffectName;
            effectCategory.Value = item.EffectCategory.HasValue ? item.EffectCategory.Value : DBNull.Value;
            description.Value = string.IsNullOrWhiteSpace(item.Description) ? DBNull.Value : item.Description;
            guid.Value = string.IsNullOrWhiteSpace(item.Guid) ? DBNull.Value : item.Guid;
            iconId.Value = item.IconId.HasValue ? item.IconId.Value : DBNull.Value;
            isOffensive.Value = item.IsOffensive ? 1 : 0;
            isAssistance.Value = item.IsAssistance ? 1 : 0;
            durationAttributeId.Value = item.DurationAttributeId.HasValue ? item.DurationAttributeId.Value : DBNull.Value;
            dischargeAttributeId.Value = item.DischargeAttributeId.HasValue ? item.DischargeAttributeId.Value : DBNull.Value;
            rangeAttributeId.Value = item.RangeAttributeId.HasValue ? item.RangeAttributeId.Value : DBNull.Value;
            disallowAutoRepeat.Value = item.DisallowAutoRepeat ? 1 : 0;
            published.Value = item.Published ? 1 : 0;
            displayName.Value = string.IsNullOrWhiteSpace(item.DisplayName) ? DBNull.Value : item.DisplayName;
            isWarpSafe.Value = item.IsWarpSafe ? 1 : 0;
            rangeChance.Value = item.RangeChance ? 1 : 0;
            electronicChance.Value = item.ElectronicChance ? 1 : 0;
            propulsionChance.Value = item.PropulsionChance ? 1 : 0;
            distribution.Value = item.Distribution.HasValue ? item.Distribution.Value : DBNull.Value;
            modifierInfo.Value = string.IsNullOrWhiteSpace(item.ModifierInfo) ? DBNull.Value : item.ModifierInfo;
            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        transaction.Commit();
    }
}
