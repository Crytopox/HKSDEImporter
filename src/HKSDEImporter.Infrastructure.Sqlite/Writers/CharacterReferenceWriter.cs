using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Infrastructure.Sqlite.Connections;

namespace HKSDEImporter.Infrastructure.Sqlite.Writers;

public sealed class CharacterReferenceWriter : ICharacterReferenceWriter
{
    private readonly SqliteConnectionFactory _connectionFactory;

    public CharacterReferenceWriter(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task WriteAsync(
        string outputPath,
        IReadOnlyCollection<Faction> factions,
        IReadOnlyCollection<Race> races,
        IReadOnlyCollection<Bloodline> bloodlines,
        IReadOnlyCollection<Ancestry> ancestries,
        IReadOnlyCollection<CharacterAttribute> attributes,
        CancellationToken cancellationToken)
    {
        await using var connection = _connectionFactory.Create(Path.GetFullPath(outputPath));
        await connection.OpenAsync(cancellationToken);
        using var transaction = connection.BeginTransaction();

        await InsertFactionsAsync(connection, transaction, factions, cancellationToken);
        await InsertRacesAsync(connection, transaction, races, cancellationToken);
        await InsertBloodlinesAsync(connection, transaction, bloodlines, cancellationToken);
        await InsertAncestriesAsync(connection, transaction, ancestries, cancellationToken);
        await InsertAttributesAsync(connection, transaction, attributes, cancellationToken);

        transaction.Commit();
    }

    private static async Task InsertFactionsAsync(Microsoft.Data.Sqlite.SqliteConnection connection, Microsoft.Data.Sqlite.SqliteTransaction transaction, IReadOnlyCollection<Faction> rows, CancellationToken cancellationToken)
    {
        using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = "INSERT INTO chrFactions (factionID, factionName, description, raceIDs, solarSystemID, corporationID, sizeFactor, stationCount, stationSystemCount, militiaCorporationID, iconID) VALUES ($factionID, $factionName, $description, $raceIDs, $solarSystemID, $corporationID, $sizeFactor, $stationCount, $stationSystemCount, $militiaCorporationID, $iconID);";

        var factionId = command.CreateParameter(); factionId.ParameterName = "$factionID"; command.Parameters.Add(factionId);
        var factionName = command.CreateParameter(); factionName.ParameterName = "$factionName"; command.Parameters.Add(factionName);
        var description = command.CreateParameter(); description.ParameterName = "$description"; command.Parameters.Add(description);
        var raceIds = command.CreateParameter(); raceIds.ParameterName = "$raceIDs"; command.Parameters.Add(raceIds);
        var solarSystemId = command.CreateParameter(); solarSystemId.ParameterName = "$solarSystemID"; command.Parameters.Add(solarSystemId);
        var corporationId = command.CreateParameter(); corporationId.ParameterName = "$corporationID"; command.Parameters.Add(corporationId);
        var sizeFactor = command.CreateParameter(); sizeFactor.ParameterName = "$sizeFactor"; command.Parameters.Add(sizeFactor);
        var stationCount = command.CreateParameter(); stationCount.ParameterName = "$stationCount"; command.Parameters.Add(stationCount);
        var stationSystemCount = command.CreateParameter(); stationSystemCount.ParameterName = "$stationSystemCount"; command.Parameters.Add(stationSystemCount);
        var militiaCorporationId = command.CreateParameter(); militiaCorporationId.ParameterName = "$militiaCorporationID"; command.Parameters.Add(militiaCorporationId);
        var iconId = command.CreateParameter(); iconId.ParameterName = "$iconID"; command.Parameters.Add(iconId);

        foreach (var row in rows)
        {
            factionId.Value = row.FactionId;
            factionName.Value = string.IsNullOrWhiteSpace(row.FactionName) ? DBNull.Value : row.FactionName;
            description.Value = string.IsNullOrWhiteSpace(row.Description) ? DBNull.Value : row.Description;
            raceIds.Value = row.RaceIds.HasValue ? row.RaceIds.Value : DBNull.Value;
            solarSystemId.Value = row.SolarSystemId.HasValue ? row.SolarSystemId.Value : DBNull.Value;
            corporationId.Value = row.CorporationId.HasValue ? row.CorporationId.Value : DBNull.Value;
            sizeFactor.Value = row.SizeFactor.HasValue ? row.SizeFactor.Value : DBNull.Value;
            stationCount.Value = row.StationCount.HasValue ? row.StationCount.Value : DBNull.Value;
            stationSystemCount.Value = row.StationSystemCount.HasValue ? row.StationSystemCount.Value : DBNull.Value;
            militiaCorporationId.Value = row.MilitiaCorporationId.HasValue ? row.MilitiaCorporationId.Value : DBNull.Value;
            iconId.Value = row.IconId.HasValue ? row.IconId.Value : DBNull.Value;

            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    private static async Task InsertRacesAsync(Microsoft.Data.Sqlite.SqliteConnection connection, Microsoft.Data.Sqlite.SqliteTransaction transaction, IReadOnlyCollection<Race> rows, CancellationToken cancellationToken)
    {
        using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = "INSERT INTO chrRaces (raceID, raceName, description, iconID, shortDescription) VALUES ($raceID, $raceName, $description, $iconID, $shortDescription);";

        var raceId = command.CreateParameter(); raceId.ParameterName = "$raceID"; command.Parameters.Add(raceId);
        var raceName = command.CreateParameter(); raceName.ParameterName = "$raceName"; command.Parameters.Add(raceName);
        var description = command.CreateParameter(); description.ParameterName = "$description"; command.Parameters.Add(description);
        var iconId = command.CreateParameter(); iconId.ParameterName = "$iconID"; command.Parameters.Add(iconId);
        var shortDescription = command.CreateParameter(); shortDescription.ParameterName = "$shortDescription"; command.Parameters.Add(shortDescription);

        foreach (var row in rows)
        {
            raceId.Value = row.RaceId;
            raceName.Value = string.IsNullOrWhiteSpace(row.RaceName) ? DBNull.Value : row.RaceName;
            description.Value = string.IsNullOrWhiteSpace(row.Description) ? DBNull.Value : row.Description;
            iconId.Value = row.IconId.HasValue ? row.IconId.Value : DBNull.Value;
            shortDescription.Value = string.IsNullOrWhiteSpace(row.ShortDescription) ? DBNull.Value : row.ShortDescription;
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    private static async Task InsertBloodlinesAsync(Microsoft.Data.Sqlite.SqliteConnection connection, Microsoft.Data.Sqlite.SqliteTransaction transaction, IReadOnlyCollection<Bloodline> rows, CancellationToken cancellationToken)
    {
        using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = "INSERT INTO chrBloodlines (bloodlineID, bloodlineName, raceID, description, maleDescription, femaleDescription, shipTypeID, corporationID, perception, willpower, charisma, memory, intelligence, iconID, shortDescription, shortMaleDescription, shortFemaleDescription) VALUES ($bloodlineID, $bloodlineName, $raceID, $description, $maleDescription, $femaleDescription, $shipTypeID, $corporationID, $perception, $willpower, $charisma, $memory, $intelligence, $iconID, $shortDescription, $shortMaleDescription, $shortFemaleDescription);";

        var bloodlineId = command.CreateParameter(); bloodlineId.ParameterName = "$bloodlineID"; command.Parameters.Add(bloodlineId);
        var bloodlineName = command.CreateParameter(); bloodlineName.ParameterName = "$bloodlineName"; command.Parameters.Add(bloodlineName);
        var raceId = command.CreateParameter(); raceId.ParameterName = "$raceID"; command.Parameters.Add(raceId);
        var description = command.CreateParameter(); description.ParameterName = "$description"; command.Parameters.Add(description);
        var maleDescription = command.CreateParameter(); maleDescription.ParameterName = "$maleDescription"; command.Parameters.Add(maleDescription);
        var femaleDescription = command.CreateParameter(); femaleDescription.ParameterName = "$femaleDescription"; command.Parameters.Add(femaleDescription);
        var shipTypeId = command.CreateParameter(); shipTypeId.ParameterName = "$shipTypeID"; command.Parameters.Add(shipTypeId);
        var corporationId = command.CreateParameter(); corporationId.ParameterName = "$corporationID"; command.Parameters.Add(corporationId);
        var perception = command.CreateParameter(); perception.ParameterName = "$perception"; command.Parameters.Add(perception);
        var willpower = command.CreateParameter(); willpower.ParameterName = "$willpower"; command.Parameters.Add(willpower);
        var charisma = command.CreateParameter(); charisma.ParameterName = "$charisma"; command.Parameters.Add(charisma);
        var memory = command.CreateParameter(); memory.ParameterName = "$memory"; command.Parameters.Add(memory);
        var intelligence = command.CreateParameter(); intelligence.ParameterName = "$intelligence"; command.Parameters.Add(intelligence);
        var iconId = command.CreateParameter(); iconId.ParameterName = "$iconID"; command.Parameters.Add(iconId);
        var shortDescription = command.CreateParameter(); shortDescription.ParameterName = "$shortDescription"; command.Parameters.Add(shortDescription);
        var shortMaleDescription = command.CreateParameter(); shortMaleDescription.ParameterName = "$shortMaleDescription"; command.Parameters.Add(shortMaleDescription);
        var shortFemaleDescription = command.CreateParameter(); shortFemaleDescription.ParameterName = "$shortFemaleDescription"; command.Parameters.Add(shortFemaleDescription);

        foreach (var row in rows)
        {
            bloodlineId.Value = row.BloodlineId;
            bloodlineName.Value = string.IsNullOrWhiteSpace(row.BloodlineName) ? DBNull.Value : row.BloodlineName;
            raceId.Value = row.RaceId.HasValue ? row.RaceId.Value : DBNull.Value;
            description.Value = string.IsNullOrWhiteSpace(row.Description) ? DBNull.Value : row.Description;
            maleDescription.Value = string.IsNullOrWhiteSpace(row.MaleDescription) ? DBNull.Value : row.MaleDescription;
            femaleDescription.Value = string.IsNullOrWhiteSpace(row.FemaleDescription) ? DBNull.Value : row.FemaleDescription;
            shipTypeId.Value = row.ShipTypeId.HasValue ? row.ShipTypeId.Value : DBNull.Value;
            corporationId.Value = row.CorporationId.HasValue ? row.CorporationId.Value : DBNull.Value;
            perception.Value = row.Perception.HasValue ? row.Perception.Value : DBNull.Value;
            willpower.Value = row.Willpower.HasValue ? row.Willpower.Value : DBNull.Value;
            charisma.Value = row.Charisma.HasValue ? row.Charisma.Value : DBNull.Value;
            memory.Value = row.Memory.HasValue ? row.Memory.Value : DBNull.Value;
            intelligence.Value = row.Intelligence.HasValue ? row.Intelligence.Value : DBNull.Value;
            iconId.Value = row.IconId.HasValue ? row.IconId.Value : DBNull.Value;
            shortDescription.Value = string.IsNullOrWhiteSpace(row.ShortDescription) ? DBNull.Value : row.ShortDescription;
            shortMaleDescription.Value = string.IsNullOrWhiteSpace(row.ShortMaleDescription) ? DBNull.Value : row.ShortMaleDescription;
            shortFemaleDescription.Value = string.IsNullOrWhiteSpace(row.ShortFemaleDescription) ? DBNull.Value : row.ShortFemaleDescription;
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    private static async Task InsertAncestriesAsync(Microsoft.Data.Sqlite.SqliteConnection connection, Microsoft.Data.Sqlite.SqliteTransaction transaction, IReadOnlyCollection<Ancestry> rows, CancellationToken cancellationToken)
    {
        using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = "INSERT INTO chrAncestries (ancestryID, ancestryName, bloodlineID, description, perception, willpower, charisma, memory, intelligence, iconID, shortDescription) VALUES ($ancestryID, $ancestryName, $bloodlineID, $description, $perception, $willpower, $charisma, $memory, $intelligence, $iconID, $shortDescription);";

        var ancestryId = command.CreateParameter(); ancestryId.ParameterName = "$ancestryID"; command.Parameters.Add(ancestryId);
        var ancestryName = command.CreateParameter(); ancestryName.ParameterName = "$ancestryName"; command.Parameters.Add(ancestryName);
        var bloodlineId = command.CreateParameter(); bloodlineId.ParameterName = "$bloodlineID"; command.Parameters.Add(bloodlineId);
        var description = command.CreateParameter(); description.ParameterName = "$description"; command.Parameters.Add(description);
        var perception = command.CreateParameter(); perception.ParameterName = "$perception"; command.Parameters.Add(perception);
        var willpower = command.CreateParameter(); willpower.ParameterName = "$willpower"; command.Parameters.Add(willpower);
        var charisma = command.CreateParameter(); charisma.ParameterName = "$charisma"; command.Parameters.Add(charisma);
        var memory = command.CreateParameter(); memory.ParameterName = "$memory"; command.Parameters.Add(memory);
        var intelligence = command.CreateParameter(); intelligence.ParameterName = "$intelligence"; command.Parameters.Add(intelligence);
        var iconId = command.CreateParameter(); iconId.ParameterName = "$iconID"; command.Parameters.Add(iconId);
        var shortDescription = command.CreateParameter(); shortDescription.ParameterName = "$shortDescription"; command.Parameters.Add(shortDescription);

        foreach (var row in rows)
        {
            ancestryId.Value = row.AncestryId;
            ancestryName.Value = string.IsNullOrWhiteSpace(row.AncestryName) ? DBNull.Value : row.AncestryName;
            bloodlineId.Value = row.BloodlineId.HasValue ? row.BloodlineId.Value : DBNull.Value;
            description.Value = string.IsNullOrWhiteSpace(row.Description) ? DBNull.Value : row.Description;
            perception.Value = row.Perception.HasValue ? row.Perception.Value : DBNull.Value;
            willpower.Value = row.Willpower.HasValue ? row.Willpower.Value : DBNull.Value;
            charisma.Value = row.Charisma.HasValue ? row.Charisma.Value : DBNull.Value;
            memory.Value = row.Memory.HasValue ? row.Memory.Value : DBNull.Value;
            intelligence.Value = row.Intelligence.HasValue ? row.Intelligence.Value : DBNull.Value;
            iconId.Value = row.IconId.HasValue ? row.IconId.Value : DBNull.Value;
            shortDescription.Value = string.IsNullOrWhiteSpace(row.ShortDescription) ? DBNull.Value : row.ShortDescription;
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    private static async Task InsertAttributesAsync(Microsoft.Data.Sqlite.SqliteConnection connection, Microsoft.Data.Sqlite.SqliteTransaction transaction, IReadOnlyCollection<CharacterAttribute> rows, CancellationToken cancellationToken)
    {
        using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = "INSERT INTO chrAttributes (attributeID, attributeName, description, iconID, shortDescription, notes) VALUES ($attributeID, $attributeName, $description, $iconID, $shortDescription, $notes);";

        var attributeId = command.CreateParameter(); attributeId.ParameterName = "$attributeID"; command.Parameters.Add(attributeId);
        var attributeName = command.CreateParameter(); attributeName.ParameterName = "$attributeName"; command.Parameters.Add(attributeName);
        var description = command.CreateParameter(); description.ParameterName = "$description"; command.Parameters.Add(description);
        var iconId = command.CreateParameter(); iconId.ParameterName = "$iconID"; command.Parameters.Add(iconId);
        var shortDescription = command.CreateParameter(); shortDescription.ParameterName = "$shortDescription"; command.Parameters.Add(shortDescription);
        var notes = command.CreateParameter(); notes.ParameterName = "$notes"; command.Parameters.Add(notes);

        foreach (var row in rows)
        {
            attributeId.Value = row.AttributeId;
            attributeName.Value = string.IsNullOrWhiteSpace(row.AttributeName) ? DBNull.Value : row.AttributeName;
            description.Value = string.IsNullOrWhiteSpace(row.Description) ? DBNull.Value : row.Description;
            iconId.Value = row.IconId.HasValue ? row.IconId.Value : DBNull.Value;
            shortDescription.Value = string.IsNullOrWhiteSpace(row.ShortDescription) ? DBNull.Value : row.ShortDescription;
            notes.Value = string.IsNullOrWhiteSpace(row.Notes) ? DBNull.Value : row.Notes;
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }
}
