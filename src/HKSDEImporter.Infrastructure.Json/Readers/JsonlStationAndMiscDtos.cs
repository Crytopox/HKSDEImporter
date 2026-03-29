using System.Text.Json.Serialization;

namespace HKSDEImporter.Infrastructure.Json.Readers;

internal sealed class JsonlStationOperationDto
{
    [JsonPropertyName("_key")]
    public int Key { get; init; }

    [JsonPropertyName("activityID")]
    public int? ActivityId { get; init; }

    [JsonPropertyName("operationName")]
    public JsonlLocalizedTextDto? OperationName { get; init; }

    [JsonPropertyName("description")]
    public JsonlLocalizedTextDto? Description { get; init; }

    [JsonPropertyName("fringe")]
    public double? Fringe { get; init; }

    [JsonPropertyName("corridor")]
    public double? Corridor { get; init; }

    [JsonPropertyName("hub")]
    public double? Hub { get; init; }

    [JsonPropertyName("border")]
    public double? Border { get; init; }

    [JsonPropertyName("ratio")]
    public double? Ratio { get; init; }

    [JsonPropertyName("services")]
    public List<int>? Services { get; init; }

    [JsonPropertyName("stationTypes")]
    public List<JsonlIdValueDto>? StationTypes { get; init; }
}

internal sealed class JsonlStationServiceDto
{
    [JsonPropertyName("_key")]
    public int Key { get; init; }

    [JsonPropertyName("serviceName")]
    public JsonlLocalizedTextDto? ServiceName { get; init; }

    [JsonPropertyName("description")]
    public JsonlLocalizedTextDto? Description { get; init; }
}

internal sealed class JsonlNpcStationDto
{
    [JsonPropertyName("_key")]
    public long Key { get; init; }

    [JsonPropertyName("operationID")]
    public int? OperationId { get; init; }

    [JsonPropertyName("ownerID")]
    public int? OwnerId { get; init; }

    [JsonPropertyName("solarSystemID")]
    public int? SolarSystemId { get; init; }

    [JsonPropertyName("typeID")]
    public int? TypeId { get; init; }

    [JsonPropertyName("position")]
    public JsonlPosition3DDto? Position { get; init; }

    [JsonPropertyName("reprocessingEfficiency")]
    public double? ReprocessingEfficiency { get; init; }

    [JsonPropertyName("reprocessingStationsTake")]
    public double? ReprocessingStationsTake { get; init; }

    [JsonPropertyName("reprocessingHangarFlag")]
    public int? ReprocessingHangarFlag { get; init; }
}

internal sealed class JsonlPosition3DDto
{
    [JsonPropertyName("x")]
    public double? X { get; init; }

    [JsonPropertyName("y")]
    public double? Y { get; init; }

    [JsonPropertyName("z")]
    public double? Z { get; init; }
}

internal sealed class JsonlMapSolarSystemDto
{
    [JsonPropertyName("_key")]
    public int Key { get; init; }

    [JsonPropertyName("constellationID")]
    public int? ConstellationId { get; init; }

    [JsonPropertyName("regionID")]
    public int? RegionId { get; init; }

    [JsonPropertyName("factionID")]
    public int? FactionId { get; init; }

    [JsonPropertyName("wormholeClassID")]
    public int? WormholeClassId { get; init; }

    [JsonPropertyName("starID")]
    public int? StarId { get; init; }

    [JsonPropertyName("name")]
    public JsonlLocalizedTextDto? Name { get; init; }

    [JsonPropertyName("securityStatus")]
    public double? SecurityStatus { get; init; }

    [JsonPropertyName("securityClass")]
    public string? SecurityClass { get; init; }

    [JsonPropertyName("luminosity")]
    public double? Luminosity { get; init; }

    [JsonPropertyName("border")]
    public bool Border { get; init; }

    [JsonPropertyName("fringe")]
    public bool Fringe { get; init; }

    [JsonPropertyName("corridor")]
    public bool Corridor { get; init; }

    [JsonPropertyName("hub")]
    public bool Hub { get; init; }

    [JsonPropertyName("international")]
    public bool International { get; init; }

    [JsonPropertyName("regional")]
    public bool Regional { get; init; }

    [JsonPropertyName("position")]
    public JsonlPosition3DDto? Position { get; init; }

    [JsonPropertyName("radius")]
    public double? Radius { get; init; }
}

internal sealed class JsonlMapRegionDto
{
    [JsonPropertyName("_key")]
    public int Key { get; init; }

    [JsonPropertyName("name")]
    public JsonlLocalizedTextDto? Name { get; init; }

    [JsonPropertyName("factionID")]
    public int? FactionId { get; init; }

    [JsonPropertyName("nebulaID")]
    public int? NebulaId { get; init; }

    [JsonPropertyName("wormholeClassID")]
    public int? WormholeClassId { get; init; }

    [JsonPropertyName("position")]
    public JsonlPosition3DDto? Position { get; init; }
}

internal sealed class JsonlMapConstellationDto
{
    [JsonPropertyName("_key")]
    public int Key { get; init; }

    [JsonPropertyName("regionID")]
    public int? RegionId { get; init; }

    [JsonPropertyName("name")]
    public JsonlLocalizedTextDto? Name { get; init; }

    [JsonPropertyName("factionID")]
    public int? FactionId { get; init; }

    [JsonPropertyName("wormholeClassID")]
    public int? WormholeClassId { get; init; }

    [JsonPropertyName("position")]
    public JsonlPosition3DDto? Position { get; init; }
}

internal sealed class JsonlMapStargateDestinationDto
{
    [JsonPropertyName("solarSystemID")]
    public int? SolarSystemId { get; init; }

    [JsonPropertyName("stargateID")]
    public int? StargateId { get; init; }
}

internal sealed class JsonlMapStargateDto
{
    [JsonPropertyName("_key")]
    public int Key { get; init; }

    [JsonPropertyName("solarSystemID")]
    public int? SolarSystemId { get; init; }

    [JsonPropertyName("destination")]
    public JsonlMapStargateDestinationDto? Destination { get; init; }
}

internal sealed class JsonlLandmarkDto
{
    [JsonPropertyName("_key")]
    public int Key { get; init; }

    [JsonPropertyName("name")]
    public JsonlLocalizedTextDto? Name { get; init; }

    [JsonPropertyName("description")]
    public JsonlLocalizedTextDto? Description { get; init; }

    [JsonPropertyName("iconID")]
    public int? IconId { get; init; }

    [JsonPropertyName("position")]
    public JsonlPosition3DDto? Position { get; init; }
}

internal sealed class JsonlContrabandTypeDto
{
    [JsonPropertyName("_key")]
    public int Key { get; init; }

    [JsonPropertyName("factions")]
    public List<JsonlContrabandFactionDto>? Factions { get; init; }
}

internal sealed class JsonlContrabandFactionDto
{
    [JsonPropertyName("_key")]
    public int FactionId { get; init; }

    [JsonPropertyName("standingLoss")]
    public double? StandingLoss { get; init; }

    [JsonPropertyName("confiscateMinSec")]
    public double? ConfiscateMinSec { get; init; }

    [JsonPropertyName("fineByValue")]
    public double? FineByValue { get; init; }

    [JsonPropertyName("attackMinSec")]
    public double? AttackMinSec { get; init; }
}

internal sealed class JsonlControlTowerResourceSetDto
{
    [JsonPropertyName("_key")]
    public int Key { get; init; }

    [JsonPropertyName("resources")]
    public List<JsonlControlTowerResourceDto>? Resources { get; init; }
}

internal sealed class JsonlControlTowerResourceDto
{
    [JsonPropertyName("resourceTypeID")]
    public int ResourceTypeId { get; init; }

    [JsonPropertyName("purpose")]
    public int? Purpose { get; init; }

    [JsonPropertyName("quantity")]
    public int? Quantity { get; init; }

    [JsonPropertyName("minSecurityLevel")]
    public double? MinSecurityLevel { get; init; }

    [JsonPropertyName("factionID")]
    public int? FactionId { get; init; }
}

internal sealed class JsonlTranslationLanguageDto
{
    [JsonPropertyName("_key")]
    public string Key { get; init; } = string.Empty;

    [JsonPropertyName("name")]
    public string? Name { get; init; }
}

internal sealed class JsonlAgentTypeDto
{
    [JsonPropertyName("_key")]
    public int Key { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }
}

internal sealed class JsonlAgentInSpaceDto
{
    [JsonPropertyName("_key")]
    public int Key { get; init; }

    [JsonPropertyName("dungeonID")]
    public int? DungeonId { get; init; }

    [JsonPropertyName("solarSystemID")]
    public int? SolarSystemId { get; init; }

    [JsonPropertyName("spawnPointID")]
    public int? SpawnPointId { get; init; }

    [JsonPropertyName("typeID")]
    public int? TypeId { get; init; }
}
