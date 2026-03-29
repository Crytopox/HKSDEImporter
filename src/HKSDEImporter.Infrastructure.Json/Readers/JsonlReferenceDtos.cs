using System.Text.Json.Serialization;

namespace HKSDEImporter.Infrastructure.Json.Readers;

internal sealed class JsonlGraphicDto
{
    [JsonPropertyName("_key")]
    public int Key { get; init; }

    [JsonPropertyName("graphicFile")]
    public string? GraphicFile { get; init; }

    [JsonPropertyName("sofFactionName")]
    public string? SofFactionName { get; init; }

    [JsonPropertyName("sofHullName")]
    public string? SofHullName { get; init; }

    [JsonPropertyName("sofRaceName")]
    public string? SofRaceName { get; init; }

    [JsonPropertyName("description")]
    public JsonlLocalizedTextDto? Description { get; init; }
}

internal sealed class JsonlIconDto
{
    [JsonPropertyName("_key")]
    public int Key { get; init; }

    [JsonPropertyName("iconFile")]
    public string? IconFile { get; init; }

    [JsonPropertyName("description")]
    public JsonlLocalizedTextDto? Description { get; init; }
}

internal sealed class JsonlFactionDto
{
    [JsonPropertyName("_key")]
    public int Key { get; init; }

    [JsonPropertyName("name")]
    public JsonlLocalizedTextDto? Name { get; init; }

    [JsonPropertyName("description")]
    public JsonlLocalizedTextDto? Description { get; init; }

    [JsonPropertyName("memberRaces")]
    public List<int>? MemberRaces { get; init; }

    [JsonPropertyName("solarSystemID")]
    public int? SolarSystemId { get; init; }

    [JsonPropertyName("corporationID")]
    public int? CorporationId { get; init; }

    [JsonPropertyName("sizeFactor")]
    public double? SizeFactor { get; init; }

    [JsonPropertyName("militiaCorporationID")]
    public int? MilitiaCorporationId { get; init; }

    [JsonPropertyName("iconID")]
    public int? IconId { get; init; }
}

internal sealed class JsonlRaceDto
{
    [JsonPropertyName("_key")]
    public int Key { get; init; }

    [JsonPropertyName("name")]
    public JsonlLocalizedTextDto? Name { get; init; }

    [JsonPropertyName("description")]
    public JsonlLocalizedTextDto? Description { get; init; }

    [JsonPropertyName("iconID")]
    public int? IconId { get; init; }

    [JsonPropertyName("shortDescription")]
    public JsonlLocalizedTextDto? ShortDescription { get; init; }
}

internal sealed class JsonlBloodlineDto
{
    [JsonPropertyName("_key")]
    public int Key { get; init; }

    [JsonPropertyName("name")]
    public JsonlLocalizedTextDto? Name { get; init; }

    [JsonPropertyName("raceID")]
    public int? RaceId { get; init; }

    [JsonPropertyName("description")]
    public JsonlLocalizedTextDto? Description { get; init; }

    [JsonPropertyName("maleDescription")]
    public JsonlLocalizedTextDto? MaleDescription { get; init; }

    [JsonPropertyName("femaleDescription")]
    public JsonlLocalizedTextDto? FemaleDescription { get; init; }

    [JsonPropertyName("shipTypeID")]
    public int? ShipTypeId { get; init; }

    [JsonPropertyName("corporationID")]
    public int? CorporationId { get; init; }

    [JsonPropertyName("perception")]
    public int? Perception { get; init; }

    [JsonPropertyName("willpower")]
    public int? Willpower { get; init; }

    [JsonPropertyName("charisma")]
    public int? Charisma { get; init; }

    [JsonPropertyName("memory")]
    public int? Memory { get; init; }

    [JsonPropertyName("intelligence")]
    public int? Intelligence { get; init; }

    [JsonPropertyName("iconID")]
    public int? IconId { get; init; }

    [JsonPropertyName("shortDescription")]
    public JsonlLocalizedTextDto? ShortDescription { get; init; }

    [JsonPropertyName("shortMaleDescription")]
    public JsonlLocalizedTextDto? ShortMaleDescription { get; init; }

    [JsonPropertyName("shortFemaleDescription")]
    public JsonlLocalizedTextDto? ShortFemaleDescription { get; init; }
}

internal sealed class JsonlAncestryDto
{
    [JsonPropertyName("_key")]
    public int Key { get; init; }

    [JsonPropertyName("name")]
    public JsonlLocalizedTextDto? Name { get; init; }

    [JsonPropertyName("bloodlineID")]
    public int? BloodlineId { get; init; }

    [JsonPropertyName("description")]
    public JsonlLocalizedTextDto? Description { get; init; }

    [JsonPropertyName("perception")]
    public int? Perception { get; init; }

    [JsonPropertyName("willpower")]
    public int? Willpower { get; init; }

    [JsonPropertyName("charisma")]
    public int? Charisma { get; init; }

    [JsonPropertyName("memory")]
    public int? Memory { get; init; }

    [JsonPropertyName("intelligence")]
    public int? Intelligence { get; init; }

    [JsonPropertyName("iconID")]
    public int? IconId { get; init; }

    [JsonPropertyName("shortDescription")]
    public JsonlLocalizedTextDto? ShortDescription { get; init; }
}

internal sealed class JsonlCharacterAttributeDto
{
    [JsonPropertyName("_key")]
    public int Key { get; init; }

    [JsonPropertyName("name")]
    public JsonlLocalizedTextDto? Name { get; init; }

    [JsonPropertyName("description")]
    public JsonlLocalizedTextDto? Description { get; init; }

    [JsonPropertyName("iconID")]
    public int? IconId { get; init; }

    [JsonPropertyName("notes")]
    public string? Notes { get; init; }

    [JsonPropertyName("shortDescription")]
    public JsonlLocalizedTextDto? ShortDescription { get; init; }
}

internal sealed class JsonlCorporationActivityDto
{
    [JsonPropertyName("_key")]
    public int Key { get; init; }

    [JsonPropertyName("name")]
    public JsonlLocalizedTextDto? Name { get; init; }
}

internal sealed class JsonlNpcCorporationDivisionDto
{
    [JsonPropertyName("_key")]
    public int Key { get; init; }

    [JsonPropertyName("name")]
    public JsonlLocalizedTextDto? Name { get; init; }

    [JsonPropertyName("displayName")]
    public string? DisplayName { get; init; }

    [JsonPropertyName("leaderTypeName")]
    public JsonlLocalizedTextDto? LeaderTypeName { get; init; }
}

internal sealed class JsonlNpcCorporationDto
{
    [JsonPropertyName("_key")]
    public int Key { get; init; }

    [JsonPropertyName("description")]
    public JsonlLocalizedTextDto? Description { get; init; }

    [JsonPropertyName("size")]
    public string? Size { get; init; }

    [JsonPropertyName("extent")]
    public string? Extent { get; init; }

    [JsonPropertyName("solarSystemID")]
    public int? SolarSystemId { get; init; }

    [JsonPropertyName("investors")]
    public List<JsonlIdValueDto>? Investors { get; init; }

    [JsonPropertyName("divisions")]
    public List<JsonlNpcCorporationDivisionLinkDto>? Divisions { get; init; }

    [JsonPropertyName("corporationTrades")]
    public List<JsonlIdDoubleDto>? CorporationTrades { get; init; }

    [JsonPropertyName("friendID")]
    public int? FriendId { get; init; }

    [JsonPropertyName("enemyID")]
    public int? EnemyId { get; init; }

    [JsonPropertyName("shares")]
    public long? Shares { get; init; }

    [JsonPropertyName("initialPrice")]
    public int? InitialPrice { get; init; }

    [JsonPropertyName("minSecurity")]
    public double? MinSecurity { get; init; }

    [JsonPropertyName("factionID")]
    public int? FactionId { get; init; }

    [JsonPropertyName("sizeFactor")]
    public double? SizeFactor { get; init; }

    [JsonPropertyName("iconID")]
    public int? IconId { get; init; }
}

internal sealed class JsonlNpcCorporationDivisionLinkDto
{
    [JsonPropertyName("_key")]
    public int Key { get; init; }

    [JsonPropertyName("size")]
    public int? Size { get; init; }
}

internal sealed class JsonlIdValueDto
{
    [JsonPropertyName("_key")]
    public int Key { get; init; }

    [JsonPropertyName("_value")]
    public int Value { get; init; }
}
