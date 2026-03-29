using System.Text.Json;
using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Models.Raw;

namespace HKSDEImporter.Infrastructure.Json.Readers;

public sealed class JsonLinesSdeReader : IRawSdeReader
{
    private static readonly IReadOnlyDictionary<string, int> ActivityNameToId = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
    {
        ["manufacturing"] = 1,
        ["research_time"] = 3,
        ["research_material"] = 4,
        ["copying"] = 5,
        ["invention"] = 8,
        ["reaction"] = 11
    };

    private readonly JsonLinesFileReader _fileReader = new();

    public Task<long> CountCategoriesAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "categories.jsonl", cancellationToken);
    public Task<long> CountGroupsAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "groups.jsonl", cancellationToken);
    public Task<long> CountTypesAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "types.jsonl", cancellationToken);
    public Task<long> CountMarketGroupsAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "marketGroups.jsonl", cancellationToken);
    public Task<long> CountMetaGroupsAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "metaGroups.jsonl", cancellationToken);
    public Task<long> CountDogmaUnitsAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "dogmaUnits.jsonl", cancellationToken);
    public Task<long> CountDogmaAttributeCategoriesAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "dogmaAttributeCategories.jsonl", cancellationToken);
    public Task<long> CountDogmaAttributesAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "dogmaAttributes.jsonl", cancellationToken);
    public Task<long> CountDogmaEffectsAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "dogmaEffects.jsonl", cancellationToken);
    public Task<long> CountTypeDogmaAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "typeDogma.jsonl", cancellationToken);
    public Task<long> CountTypeMaterialsAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "typeMaterials.jsonl", cancellationToken);
    public Task<long> CountBlueprintsAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "blueprints.jsonl", cancellationToken);
    public Task<long> CountPlanetSchematicsAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "planetSchematics.jsonl", cancellationToken);
    public Task<long> CountGraphicsAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "graphics.jsonl", cancellationToken);
    public Task<long> CountIconsAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "icons.jsonl", cancellationToken);
    public Task<long> CountFactionsAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "factions.jsonl", cancellationToken);
    public Task<long> CountRacesAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "races.jsonl", cancellationToken);
    public Task<long> CountBloodlinesAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "bloodlines.jsonl", cancellationToken);
    public Task<long> CountAncestriesAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "ancestries.jsonl", cancellationToken);
    public Task<long> CountCharacterAttributesAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "characterAttributes.jsonl", cancellationToken);
    public Task<long> CountCorporationActivitiesAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "corporationActivities.jsonl", cancellationToken);
    public Task<long> CountNpcCorporationDivisionsAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "npcCorporationDivisions.jsonl", cancellationToken);
    public Task<long> CountNpcCorporationsAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "npcCorporations.jsonl", cancellationToken);
    public Task<long> CountStationOperationsAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "stationOperations.jsonl", cancellationToken);
    public Task<long> CountStationServicesAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "stationServices.jsonl", cancellationToken);
    public Task<long> CountNpcStationsAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "npcStations.jsonl", cancellationToken);
    public Task<long> CountMapRegionsAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "mapRegions.jsonl", cancellationToken);
    public Task<long> CountMapConstellationsAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "mapConstellations.jsonl", cancellationToken);
    public Task<long> CountMapSolarSystemsAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "mapSolarSystems.jsonl", cancellationToken);
    public Task<long> CountMapStargatesAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "mapStargates.jsonl", cancellationToken);
    public Task<long> CountLandmarksAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "landmarks.jsonl", cancellationToken);
    public Task<long> CountMapPlanetsAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "mapPlanets.jsonl", cancellationToken);
    public Task<long> CountMapMoonsAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "mapMoons.jsonl", cancellationToken);
    public Task<long> CountMapAsteroidBeltsAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "mapAsteroidBelts.jsonl", cancellationToken);
    public Task<long> CountMapStarsAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "mapStars.jsonl", cancellationToken);
    public Task<long> CountMapSecondarySunsAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "mapSecondarySuns.jsonl", cancellationToken);
    public Task<long> CountContrabandTypesAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "contrabandTypes.jsonl", cancellationToken);
    public Task<long> CountControlTowerResourcesAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "controlTowerResources.jsonl", cancellationToken);
    public Task<long> CountTranslationLanguagesAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "translationLanguages.jsonl", cancellationToken);
    public Task<long> CountAgentTypesAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "agentTypes.jsonl", cancellationToken);
    public Task<long> CountAgentsInSpaceAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "agentsInSpace.jsonl", cancellationToken);
    public Task<long> CountCertificatesAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "certificates.jsonl", cancellationToken);
    public Task<long> CountMasteriesAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "masteries.jsonl", cancellationToken);
    public Task<long> CountSkinsAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "skins.jsonl", cancellationToken);
    public Task<long> CountSkinMaterialsAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "skinMaterials.jsonl", cancellationToken);
    public Task<long> CountSkinLicensesAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "skinLicenses.jsonl", cancellationToken);
    public Task<long> CountNpcCharactersAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "npcCharacters.jsonl", cancellationToken);
    public Task<long> CountCloneGradesAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "cloneGrades.jsonl", cancellationToken);
    public Task<long> CountCompressibleTypesAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "compressibleTypes.jsonl", cancellationToken);
    public Task<long> CountDbuffCollectionsAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "dbuffCollections.jsonl", cancellationToken);
    public Task<long> CountDynamicItemAttributesAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "dynamicItemAttributes.jsonl", cancellationToken);
    public Task<long> CountFreelanceJobSchemasAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "freelanceJobSchemas.jsonl", cancellationToken);
    public Task<long> CountMercenaryTacticalOperationsAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "mercenaryTacticalOperations.jsonl", cancellationToken);
    public Task<long> CountPlanetResourcesAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "planetResources.jsonl", cancellationToken);
    public Task<long> CountSovereigntyUpgradesAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "sovereigntyUpgrades.jsonl", cancellationToken);
    public Task<long> CountTypeBonusesAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "typeBonus.jsonl", cancellationToken);
    public Task<long> CountSdeBuildInfoAsync(string rootDirectory, CancellationToken cancellationToken) => CountAsync(rootDirectory, "_sde.jsonl", cancellationToken);

    public async IAsyncEnumerable<RawCategory> ReadCategoriesAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "categories.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonlCategoryDto>(path, cancellationToken))
        {
            yield return new RawCategory
            {
                Key = item.Key,
                IconId = item.IconId,
                Name = item.Name is null ? null : new RawLocalizedText { En = item.Name.En },
                Published = item.Published
            };
        }
    }

    public async IAsyncEnumerable<RawGroup> ReadGroupsAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "groups.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonlGroupDto>(path, cancellationToken))
        {
            yield return new RawGroup
            {
                Key = item.Key,
                CategoryId = item.CategoryId,
                IconId = item.IconId,
                UseBasePrice = item.UseBasePrice,
                Anchored = item.Anchored,
                Anchorable = item.Anchorable,
                FittableNonSingleton = item.FittableNonSingleton,
                Name = item.Name is null ? null : new RawLocalizedText { En = item.Name.En },
                Published = item.Published
            };
        }
    }

    public async IAsyncEnumerable<RawType> ReadTypesAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "types.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonlTypeDto>(path, cancellationToken))
        {
            yield return new RawType
            {
                Key = item.Key,
                GroupId = item.GroupId,
                Description = item.Description is null ? null : new RawLocalizedText { En = item.Description.En },
                IconId = item.IconId,
                SoundId = item.SoundId,
                GraphicId = item.GraphicId,
                MarketGroupId = item.MarketGroupId,
                RaceId = item.RaceId,
                MetaGroupId = item.MetaGroupId,
                VariationParentTypeId = item.VariationParentTypeId,
                Mass = item.Mass,
                Volume = item.Volume,
                Capacity = item.Capacity,
                BasePrice = item.BasePrice,
                Name = item.Name is null ? null : new RawLocalizedText { En = item.Name.En },
                PortionSize = item.PortionSize,
                Published = item.Published,
                Radius = item.Radius
            };
        }
    }

    public async IAsyncEnumerable<RawMarketGroup> ReadMarketGroupsAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "marketGroups.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonlMarketGroupDto>(path, cancellationToken))
        {
            yield return new RawMarketGroup
            {
                Key = item.Key,
                ParentGroupId = item.ParentGroupId,
                Name = item.Name is null ? null : new RawLocalizedText { En = item.Name.En },
                Description = item.Description is null ? null : new RawLocalizedText { En = item.Description.En },
                IconId = item.IconId,
                HasTypes = item.HasTypes
            };
        }
    }

    public async IAsyncEnumerable<RawMetaGroup> ReadMetaGroupsAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "metaGroups.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonlMetaGroupDto>(path, cancellationToken))
        {
            yield return new RawMetaGroup
            {
                Key = item.Key,
                Name = item.Name is null ? null : new RawLocalizedText { En = item.Name.En },
                Description = item.Description is null ? null : new RawLocalizedText { En = item.Description.En },
                IconId = item.IconId
            };
        }
    }

    public async IAsyncEnumerable<RawDogmaUnit> ReadDogmaUnitsAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "dogmaUnits.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonlDogmaUnitDto>(path, cancellationToken))
        {
            yield return new RawDogmaUnit
            {
                Key = item.Key,
                Name = item.Name,
                DisplayName = item.DisplayName is null ? null : new RawLocalizedText { En = item.DisplayName.En },
                Description = item.Description is null ? null : new RawLocalizedText { En = item.Description.En }
            };
        }
    }

    public async IAsyncEnumerable<RawDogmaAttributeCategory> ReadDogmaAttributeCategoriesAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "dogmaAttributeCategories.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonlDogmaAttributeCategoryDto>(path, cancellationToken))
        {
            yield return new RawDogmaAttributeCategory
            {
                Key = item.Key,
                Name = item.Name,
                Description = item.Description is null ? null : new RawLocalizedText { En = item.Description.En }
            };
        }
    }

    public async IAsyncEnumerable<RawDogmaAttribute> ReadDogmaAttributesAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "dogmaAttributes.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonlDogmaAttributeDto>(path, cancellationToken))
        {
            yield return new RawDogmaAttribute
            {
                Key = item.Key,
                AttributeCategoryId = item.AttributeCategoryId,
                Name = item.Name,
                DisplayName = item.DisplayName is null ? null : new RawLocalizedText { En = item.DisplayName.En },
                Description = item.Description is null ? null : new RawLocalizedText { En = item.Description.En },
                IconId = item.IconId,
                DefaultValue = item.DefaultValue,
                Published = item.Published,
                UnitId = item.UnitId,
                Stackable = item.Stackable,
                HighIsGood = item.HighIsGood
            };
        }
    }

    public async IAsyncEnumerable<RawDogmaEffect> ReadDogmaEffectsAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "dogmaEffects.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonlDogmaEffectDto>(path, cancellationToken))
        {
            yield return new RawDogmaEffect
            {
                Key = item.Key,
                Name = item.Name,
                EffectCategoryId = item.EffectCategoryId,
                Description = item.Description is null ? null : new RawLocalizedText { En = item.Description.En },
                Guid = item.Guid,
                IconId = item.IconId,
                IsOffensive = item.IsOffensive,
                IsAssistance = item.IsAssistance,
                DurationAttributeId = item.DurationAttributeId,
                DischargeAttributeId = item.DischargeAttributeId,
                RangeAttributeId = item.RangeAttributeId,
                DisallowAutoRepeat = item.DisallowAutoRepeat,
                Published = item.Published,
                DisplayName = item.DisplayName is null ? null : new RawLocalizedText { En = item.DisplayName.En },
                IsWarpSafe = item.IsWarpSafe,
                RangeChance = item.RangeChance,
                ElectronicChance = item.ElectronicChance,
                PropulsionChance = item.PropulsionChance,
                Distribution = item.Distribution,
                ModifierInfoJson = item.ModifierInfo.HasValue ? JsonSerializer.Serialize(item.ModifierInfo.Value) : null
            };
        }
    }

    public async IAsyncEnumerable<RawTypeDogma> ReadTypeDogmaAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "typeDogma.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonlTypeDogmaDto>(path, cancellationToken))
        {
            yield return new RawTypeDogma
            {
                TypeId = item.Key,
                Attributes = item.DogmaAttributes?.Select(a => new RawTypeDogmaAttribute { AttributeId = a.AttributeId, Value = a.Value }).ToList() ?? [],
                Effects = item.DogmaEffects?.Select(e => new RawTypeDogmaEffect { EffectId = e.EffectId, IsDefault = e.IsDefault }).ToList() ?? []
            };
        }
    }

    public async IAsyncEnumerable<RawTypeMaterial> ReadTypeMaterialsAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "typeMaterials.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonlTypeMaterialsDto>(path, cancellationToken))
        {
            yield return new RawTypeMaterial
            {
                TypeId = item.Key,
                Materials = item.Materials?.Select(m => new RawTypeMaterialItem { MaterialTypeId = m.MaterialTypeId, Quantity = m.Quantity }).ToList() ?? []
            };
        }
    }

    public async IAsyncEnumerable<RawBlueprint> ReadBlueprintsAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "blueprints.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonlBlueprintDto>(path, cancellationToken))
        {
            var activities = new List<RawBlueprintActivity>();
            foreach (var kvp in item.Activities ?? new Dictionary<string, JsonElement>())
            {
                if (!ActivityNameToId.TryGetValue(kvp.Key, out var activityId))
                {
                    continue;
                }

                var activity = ParseBlueprintActivity(kvp.Value, activityId);
                activities.Add(activity);
            }

            yield return new RawBlueprint
            {
                TypeId = item.BlueprintTypeId ?? item.Key,
                MaxProductionLimit = item.MaxProductionLimit,
                Activities = activities
            };
        }
    }

    public async IAsyncEnumerable<RawPlanetSchematic> ReadPlanetSchematicsAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "planetSchematics.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonlPlanetSchematicsDto>(path, cancellationToken))
        {
            yield return new RawPlanetSchematic
            {
                SchematicId = item.Key,
                Name = item.Name?.En,
                CycleTime = item.CycleTime,
                PinTypeIds = item.Pins ?? [],
                Types = item.Types?.Select(t => new RawPlanetSchematicType
                {
                    TypeId = t.TypeId,
                    Quantity = t.Quantity,
                    IsInput = t.IsInput
                }).ToList() ?? []
            };
        }
    }

    public async IAsyncEnumerable<RawGraphic> ReadGraphicsAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "graphics.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonlGraphicDto>(path, cancellationToken))
        {
            yield return new RawGraphic
            {
                Key = item.Key,
                GraphicFile = item.GraphicFile,
                SofFactionName = item.SofFactionName,
                SofHullName = item.SofHullName,
                SofRaceName = item.SofRaceName,
                Description = item.Description is null ? null : new RawLocalizedText { En = item.Description.En }
            };
        }
    }

    public async IAsyncEnumerable<RawIcon> ReadIconsAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "icons.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonlIconDto>(path, cancellationToken))
        {
            yield return new RawIcon
            {
                Key = item.Key,
                IconFile = item.IconFile,
                Description = item.Description is null ? null : new RawLocalizedText { En = item.Description.En }
            };
        }
    }

    public async IAsyncEnumerable<RawFaction> ReadFactionsAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "factions.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonlFactionDto>(path, cancellationToken))
        {
            yield return new RawFaction
            {
                Key = item.Key,
                Name = item.Name is null ? null : new RawLocalizedText { En = item.Name.En },
                Description = item.Description is null ? null : new RawLocalizedText { En = item.Description.En },
                MemberRaces = item.MemberRaces,
                SolarSystemId = item.SolarSystemId,
                CorporationId = item.CorporationId,
                SizeFactor = item.SizeFactor,
                MilitiaCorporationId = item.MilitiaCorporationId,
                IconId = item.IconId
            };
        }
    }

    public async IAsyncEnumerable<RawRace> ReadRacesAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "races.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonlRaceDto>(path, cancellationToken))
        {
            yield return new RawRace
            {
                Key = item.Key,
                Name = item.Name is null ? null : new RawLocalizedText { En = item.Name.En },
                Description = item.Description is null ? null : new RawLocalizedText { En = item.Description.En },
                IconId = item.IconId,
                ShortDescription = item.ShortDescription is null ? null : new RawLocalizedText { En = item.ShortDescription.En }
            };
        }
    }

    public async IAsyncEnumerable<RawBloodline> ReadBloodlinesAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "bloodlines.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonlBloodlineDto>(path, cancellationToken))
        {
            yield return new RawBloodline
            {
                Key = item.Key,
                Name = item.Name is null ? null : new RawLocalizedText { En = item.Name.En },
                RaceId = item.RaceId,
                Description = item.Description is null ? null : new RawLocalizedText { En = item.Description.En },
                MaleDescription = item.MaleDescription is null ? null : new RawLocalizedText { En = item.MaleDescription.En },
                FemaleDescription = item.FemaleDescription is null ? null : new RawLocalizedText { En = item.FemaleDescription.En },
                ShipTypeId = item.ShipTypeId,
                CorporationId = item.CorporationId,
                Perception = item.Perception,
                Willpower = item.Willpower,
                Charisma = item.Charisma,
                Memory = item.Memory,
                Intelligence = item.Intelligence,
                IconId = item.IconId,
                ShortDescription = item.ShortDescription is null ? null : new RawLocalizedText { En = item.ShortDescription.En },
                ShortMaleDescription = item.ShortMaleDescription is null ? null : new RawLocalizedText { En = item.ShortMaleDescription.En },
                ShortFemaleDescription = item.ShortFemaleDescription is null ? null : new RawLocalizedText { En = item.ShortFemaleDescription.En }
            };
        }
    }

    public async IAsyncEnumerable<RawAncestry> ReadAncestriesAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "ancestries.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonlAncestryDto>(path, cancellationToken))
        {
            yield return new RawAncestry
            {
                Key = item.Key,
                Name = item.Name is null ? null : new RawLocalizedText { En = item.Name.En },
                BloodlineId = item.BloodlineId,
                Description = item.Description is null ? null : new RawLocalizedText { En = item.Description.En },
                Perception = item.Perception,
                Willpower = item.Willpower,
                Charisma = item.Charisma,
                Memory = item.Memory,
                Intelligence = item.Intelligence,
                IconId = item.IconId,
                ShortDescription = item.ShortDescription is null ? null : new RawLocalizedText { En = item.ShortDescription.En }
            };
        }
    }

    public async IAsyncEnumerable<RawCharacterAttribute> ReadCharacterAttributesAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "characterAttributes.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonlCharacterAttributeDto>(path, cancellationToken))
        {
            yield return new RawCharacterAttribute
            {
                Key = item.Key,
                Name = item.Name is null ? null : new RawLocalizedText { En = item.Name.En },
                Description = item.Description is null ? null : new RawLocalizedText { En = item.Description.En },
                IconId = item.IconId,
                Notes = item.Notes,
                ShortDescription = item.ShortDescription is null ? null : new RawLocalizedText { En = item.ShortDescription.En }
            };
        }
    }

    public async IAsyncEnumerable<RawCorporationActivity> ReadCorporationActivitiesAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "corporationActivities.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonlCorporationActivityDto>(path, cancellationToken))
        {
            yield return new RawCorporationActivity
            {
                Key = item.Key,
                Name = item.Name is null ? null : new RawLocalizedText { En = item.Name.En }
            };
        }
    }

    public async IAsyncEnumerable<RawNpcCorporationDivision> ReadNpcCorporationDivisionsAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "npcCorporationDivisions.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonlNpcCorporationDivisionDto>(path, cancellationToken))
        {
            yield return new RawNpcCorporationDivision
            {
                Key = item.Key,
                Name = item.Name is null ? null : new RawLocalizedText { En = item.Name.En },
                DisplayName = item.DisplayName,
                LeaderTypeName = item.LeaderTypeName is null ? null : new RawLocalizedText { En = item.LeaderTypeName.En }
            };
        }
    }

    public async IAsyncEnumerable<RawNpcCorporation> ReadNpcCorporationsAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "npcCorporations.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonlNpcCorporationDto>(path, cancellationToken))
        {
            yield return new RawNpcCorporation
            {
                Key = item.Key,
                Description = item.Description is null ? null : new RawLocalizedText { En = item.Description.En },
                Size = item.Size,
                Extent = item.Extent,
                SolarSystemId = item.SolarSystemId,
                Investors = item.Investors?.Select(i => new RawNpcCorporationInvestor { Key = i.Key, Value = i.Value }).ToList(),
                FriendId = item.FriendId,
                EnemyId = item.EnemyId,
                Shares = item.Shares,
                InitialPrice = item.InitialPrice,
                MinSecurity = item.MinSecurity,
                FactionId = item.FactionId,
                SizeFactor = item.SizeFactor,
                IconId = item.IconId,
                CorporationTrades = item.CorporationTrades?.Select(t => new RawNpcCorporationTrade
                {
                    Key = t.Key,
                    Value = t.Value
                }).ToList(),
                Divisions = item.Divisions?.Select(d => new RawNpcCorporationDivisionLink
                {
                    DivisionId = d.Key,
                    Size = d.Size
                }).ToList()
            };
        }
    }

    public async IAsyncEnumerable<RawStationOperation> ReadStationOperationsAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "stationOperations.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonlStationOperationDto>(path, cancellationToken))
        {
            yield return new RawStationOperation
            {
                Key = item.Key,
                ActivityId = item.ActivityId,
                OperationName = item.OperationName is null ? null : new RawLocalizedText { En = item.OperationName.En },
                Description = item.Description is null ? null : new RawLocalizedText { En = item.Description.En },
                Fringe = item.Fringe,
                Corridor = item.Corridor,
                Hub = item.Hub,
                Border = item.Border,
                Ratio = item.Ratio,
                Services = item.Services,
                StationTypes = item.StationTypes?.Select(x => new RawRaceStationType { RaceKey = x.Key, StationTypeId = x.Value }).ToList()
            };
        }
    }

    public async IAsyncEnumerable<RawStationService> ReadStationServicesAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "stationServices.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonlStationServiceDto>(path, cancellationToken))
        {
            yield return new RawStationService
            {
                Key = item.Key,
                ServiceName = item.ServiceName is null ? null : new RawLocalizedText { En = item.ServiceName.En },
                Description = item.Description is null ? null : new RawLocalizedText { En = item.Description.En }
            };
        }
    }

    public async IAsyncEnumerable<RawNpcStation> ReadNpcStationsAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "npcStations.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonlNpcStationDto>(path, cancellationToken))
        {
            yield return new RawNpcStation
            {
                Key = item.Key,
                OperationId = item.OperationId,
                OwnerId = item.OwnerId,
                SolarSystemId = item.SolarSystemId,
                TypeId = item.TypeId,
                Position = item.Position is null ? null : new RawPosition3D
                {
                    X = item.Position.X,
                    Y = item.Position.Y,
                    Z = item.Position.Z
                },
                ReprocessingEfficiency = item.ReprocessingEfficiency,
                ReprocessingStationsTake = item.ReprocessingStationsTake,
                ReprocessingHangarFlag = item.ReprocessingHangarFlag
            };
        }
    }

    public async IAsyncEnumerable<RawMapRegion> ReadMapRegionsAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "mapRegions.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonlMapRegionDto>(path, cancellationToken))
        {
            yield return new RawMapRegion
            {
                Key = item.Key,
                NameEn = item.Name?.En,
                FactionId = item.FactionId,
                NebulaId = item.NebulaId,
                WormholeClassId = item.WormholeClassId,
                Position = item.Position is null ? null : new RawPosition3D
                {
                    X = item.Position.X,
                    Y = item.Position.Y,
                    Z = item.Position.Z
                }
            };
        }
    }

    public async IAsyncEnumerable<RawMapConstellation> ReadMapConstellationsAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "mapConstellations.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonlMapConstellationDto>(path, cancellationToken))
        {
            yield return new RawMapConstellation
            {
                Key = item.Key,
                RegionId = item.RegionId,
                NameEn = item.Name?.En,
                FactionId = item.FactionId,
                WormholeClassId = item.WormholeClassId,
                Position = item.Position is null ? null : new RawPosition3D
                {
                    X = item.Position.X,
                    Y = item.Position.Y,
                    Z = item.Position.Z
                }
            };
        }
    }

    public async IAsyncEnumerable<RawMapSolarSystem> ReadMapSolarSystemsAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "mapSolarSystems.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonlMapSolarSystemDto>(path, cancellationToken))
        {
            yield return new RawMapSolarSystem
            {
                Key = item.Key,
                ConstellationId = item.ConstellationId,
                RegionId = item.RegionId,
                FactionId = item.FactionId,
                WormholeClassId = item.WormholeClassId,
                StarId = item.StarId,
                NameEn = item.Name?.En,
                SecurityStatus = item.SecurityStatus,
                SecurityClass = item.SecurityClass,
                Luminosity = item.Luminosity,
                Border = item.Border,
                Fringe = item.Fringe,
                Corridor = item.Corridor,
                Hub = item.Hub,
                International = item.International,
                Regional = item.Regional,
                Position = item.Position is null ? null : new RawPosition3D
                {
                    X = item.Position.X,
                    Y = item.Position.Y,
                    Z = item.Position.Z
                },
                Radius = item.Radius
            };
        }
    }

    public async IAsyncEnumerable<RawMapStargate> ReadMapStargatesAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "mapStargates.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonlMapStargateDto>(path, cancellationToken))
        {
            yield return new RawMapStargate
            {
                StargateId = item.Key,
                SolarSystemId = item.SolarSystemId,
                DestinationSolarSystemId = item.Destination?.SolarSystemId,
                DestinationStargateId = item.Destination?.StargateId
            };
        }
    }

    public async IAsyncEnumerable<RawLandmark> ReadLandmarksAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "landmarks.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonlLandmarkDto>(path, cancellationToken))
        {
            yield return new RawLandmark
            {
                Key = item.Key,
                NameEn = item.Name?.En,
                DescriptionEn = item.Description?.En,
                IconId = item.IconId,
                Position = item.Position is null ? null : new RawPosition3D
                {
                    X = item.Position.X,
                    Y = item.Position.Y,
                    Z = item.Position.Z
                }
            };
        }
    }

    public async IAsyncEnumerable<RawMapPlanet> ReadMapPlanetsAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "mapPlanets.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonlMapPlanetDto>(path, cancellationToken))
        {
            yield return new RawMapPlanet
            {
                Key = item.Key,
                TypeId = item.TypeId,
                SolarSystemId = item.SolarSystemId,
                OrbitId = item.OrbitId,
                CelestialIndex = item.CelestialIndex,
                Position = item.Position is null ? null : new RawPosition3D
                {
                    X = item.Position.X,
                    Y = item.Position.Y,
                    Z = item.Position.Z
                },
                Radius = item.Radius,
                Attributes = MapAttributes(item.Attributes),
                Statistics = MapStatistics(item.Statistics)
            };
        }
    }

    public async IAsyncEnumerable<RawMapMoon> ReadMapMoonsAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "mapMoons.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonlMapMoonDto>(path, cancellationToken))
        {
            yield return new RawMapMoon
            {
                Key = item.Key,
                TypeId = item.TypeId,
                SolarSystemId = item.SolarSystemId,
                OrbitId = item.OrbitId,
                CelestialIndex = item.CelestialIndex,
                OrbitIndex = item.OrbitIndex,
                Position = item.Position is null ? null : new RawPosition3D
                {
                    X = item.Position.X,
                    Y = item.Position.Y,
                    Z = item.Position.Z
                },
                Radius = item.Radius,
                Attributes = MapAttributes(item.Attributes),
                Statistics = MapStatistics(item.Statistics)
            };
        }
    }

    public async IAsyncEnumerable<RawMapAsteroidBelt> ReadMapAsteroidBeltsAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "mapAsteroidBelts.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonlMapAsteroidBeltDto>(path, cancellationToken))
        {
            yield return new RawMapAsteroidBelt
            {
                Key = item.Key,
                TypeId = item.TypeId,
                SolarSystemId = item.SolarSystemId,
                OrbitId = item.OrbitId,
                CelestialIndex = item.CelestialIndex,
                OrbitIndex = item.OrbitIndex,
                Position = item.Position is null ? null : new RawPosition3D
                {
                    X = item.Position.X,
                    Y = item.Position.Y,
                    Z = item.Position.Z
                },
                Radius = item.Radius,
                Statistics = MapStatistics(item.Statistics)
            };
        }
    }

    public async IAsyncEnumerable<RawMapStar> ReadMapStarsAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "mapStars.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonlMapStarDto>(path, cancellationToken))
        {
            yield return new RawMapStar
            {
                Key = item.Key,
                TypeId = item.TypeId,
                SolarSystemId = item.SolarSystemId,
                Radius = item.Radius,
                Statistics = MapStatistics(item.Statistics)
            };
        }
    }

    public async IAsyncEnumerable<RawMapSecondarySun> ReadMapSecondarySunsAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "mapSecondarySuns.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonlMapSecondarySunDto>(path, cancellationToken))
        {
            yield return new RawMapSecondarySun
            {
                Key = item.Key,
                TypeId = item.TypeId,
                SolarSystemId = item.SolarSystemId,
                Position = item.Position is null ? null : new RawPosition3D
                {
                    X = item.Position.X,
                    Y = item.Position.Y,
                    Z = item.Position.Z
                }
            };
        }
    }

    public async IAsyncEnumerable<RawContrabandType> ReadContrabandTypesAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "contrabandTypes.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonlContrabandTypeDto>(path, cancellationToken))
        {
            yield return new RawContrabandType
            {
                TypeId = item.Key,
                Factions = item.Factions?.Select(f => new RawContrabandFactionRule
                {
                    FactionId = f.FactionId,
                    StandingLoss = f.StandingLoss,
                    ConfiscateMinSec = f.ConfiscateMinSec,
                    FineByValue = f.FineByValue,
                    AttackMinSec = f.AttackMinSec
                }).ToList() ?? []
            };
        }
    }

    public async IAsyncEnumerable<RawControlTowerResourceSet> ReadControlTowerResourcesAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "controlTowerResources.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonlControlTowerResourceSetDto>(path, cancellationToken))
        {
            yield return new RawControlTowerResourceSet
            {
                ControlTowerTypeId = item.Key,
                Resources = item.Resources?.Select(r => new RawControlTowerResource
                {
                    ResourceTypeId = r.ResourceTypeId,
                    Purpose = r.Purpose,
                    Quantity = r.Quantity,
                    MinSecurityLevel = r.MinSecurityLevel,
                    FactionId = r.FactionId
                }).ToList() ?? []
            };
        }
    }

    public async IAsyncEnumerable<RawTranslationLanguage> ReadTranslationLanguagesAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "translationLanguages.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonlTranslationLanguageDto>(path, cancellationToken))
        {
            yield return new RawTranslationLanguage
            {
                Key = item.Key,
                Name = item.Name
            };
        }
    }

    public async IAsyncEnumerable<RawAgentType> ReadAgentTypesAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "agentTypes.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonlAgentTypeDto>(path, cancellationToken))
        {
            yield return new RawAgentType
            {
                Key = item.Key,
                Name = item.Name
            };
        }
    }

    public async IAsyncEnumerable<RawAgentInSpace> ReadAgentsInSpaceAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "agentsInSpace.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonlAgentInSpaceDto>(path, cancellationToken))
        {
            yield return new RawAgentInSpace
            {
                AgentId = item.Key,
                DungeonId = item.DungeonId,
                SolarSystemId = item.SolarSystemId,
                SpawnPointId = item.SpawnPointId,
                TypeId = item.TypeId
            };
        }
    }

    public async IAsyncEnumerable<RawCertificate> ReadCertificatesAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "certificates.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonlCertificateDto>(path, cancellationToken))
        {
            yield return new RawCertificate
            {
                CertId = item.Key,
                Name = item.Name?.En,
                Description = item.Description?.En,
                GroupId = item.GroupId,
                Skills = item.SkillTypes?.Select(s => new RawCertificateSkill
                {
                    SkillId = s.SkillId,
                    Basic = s.Basic,
                    Standard = s.Standard,
                    Improved = s.Improved,
                    Advanced = s.Advanced,
                    Elite = s.Elite
                }).ToList() ?? []
            };
        }
    }

    public async IAsyncEnumerable<RawMastery> ReadMasteriesAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "masteries.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonlMasteryDto>(path, cancellationToken))
        {
            yield return new RawMastery
            {
                TypeId = item.Key,
                Levels = item.Levels?.Select(l => new RawMasteryLevel
                {
                    MasteryLevel = l.MasteryLevel,
                    CertIds = l.CertIds ?? []
                }).ToList() ?? []
            };
        }
    }

    public async IAsyncEnumerable<RawSkin> ReadSkinsAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "skins.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonlSkinDto>(path, cancellationToken))
        {
            yield return new RawSkin
            {
                SkinId = item.Key,
                InternalName = item.InternalName,
                SkinMaterialId = item.SkinMaterialId,
                TypeIds = item.Types ?? []
            };
        }
    }

    public async IAsyncEnumerable<RawSkinMaterial> ReadSkinMaterialsAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "skinMaterials.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonlSkinMaterialDto>(path, cancellationToken))
        {
            yield return new RawSkinMaterial
            {
                SkinMaterialId = item.Key,
                DisplayName = item.DisplayName?.En,
                MaterialSetId = item.MaterialSetId
            };
        }
    }

    public async IAsyncEnumerable<RawSkinLicense> ReadSkinLicensesAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "skinLicenses.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonlSkinLicenseDto>(path, cancellationToken))
        {
            yield return new RawSkinLicense
            {
                LicenseTypeId = item.LicenseTypeId,
                Duration = item.Duration,
                SkinId = item.SkinId
            };
        }
    }

    public async IAsyncEnumerable<RawNpcCharacter> ReadNpcCharactersAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "npcCharacters.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonlNpcCharacterDto>(path, cancellationToken))
        {
            yield return new RawNpcCharacter
            {
                CharacterId = item.Key,
                CorporationId = item.CorporationId,
                LocationId = item.LocationId,
                Agent = item.Agent is null ? null : new RawNpcAgentData
                {
                    AgentTypeId = item.Agent.AgentTypeId,
                    DivisionId = item.Agent.DivisionId,
                    IsLocator = item.Agent.IsLocator,
                    Level = item.Agent.Level,
                    Quality = item.Agent.Quality
                },
                SkillTypeIds = item.Skills?.Where(s => s.TypeId.HasValue).Select(s => s.TypeId!.Value).ToList()
            };
        }
    }

    public async IAsyncEnumerable<RawCloneGrade> ReadCloneGradesAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "cloneGrades.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonElement>(path, cancellationToken))
        {
            var skills = new List<RawCloneGradeSkill>();
            if (item.TryGetProperty("skills", out var skillsElement) && skillsElement.ValueKind == JsonValueKind.Array)
            {
                foreach (var skill in skillsElement.EnumerateArray())
                {
                    var typeId = GetInt(skill, "typeID");
                    var level = GetInt(skill, "level");
                    if (typeId.HasValue && level.HasValue)
                    {
                        skills.Add(new RawCloneGradeSkill
                        {
                            TypeId = typeId.Value,
                            Level = level.Value
                        });
                    }
                }
            }

            yield return new RawCloneGrade
            {
                CloneGradeId = GetInt(item, "_key") ?? 0,
                Name = GetString(item, "name"),
                Skills = skills
            };
        }
    }

    public async IAsyncEnumerable<RawCompressibleType> ReadCompressibleTypesAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "compressibleTypes.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonElement>(path, cancellationToken))
        {
            yield return new RawCompressibleType
            {
                TypeId = GetInt(item, "_key") ?? 0,
                CompressedTypeId = GetInt(item, "compressedTypeID") ?? 0
            };
        }
    }

    public async IAsyncEnumerable<RawDbuffCollection> ReadDbuffCollectionsAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "dbuffCollections.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonElement>(path, cancellationToken))
        {
            yield return new RawDbuffCollection
            {
                CollectionId = GetInt(item, "_key") ?? 0,
                AggregateMode = GetString(item, "aggregateMode"),
                OperationName = GetString(item, "operationName"),
                ShowOutputValueInUi = GetString(item, "showOutputValueInUI"),
                DeveloperDescription = GetString(item, "developerDescription"),
                DisplayNameEn = GetLocalizedEn(item, "displayName"),
                RawJson = item.GetRawText()
            };
        }
    }

    public async IAsyncEnumerable<RawDynamicItemAttribute> ReadDynamicItemAttributesAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "dynamicItemAttributes.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonElement>(path, cancellationToken))
        {
            yield return new RawDynamicItemAttribute
            {
                TypeId = GetInt(item, "_key") ?? 0,
                RawJson = item.GetRawText()
            };
        }
    }

    public async IAsyncEnumerable<RawFreelanceJobSchema> ReadFreelanceJobSchemasAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "freelanceJobSchemas.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonElement>(path, cancellationToken))
        {
            yield return new RawFreelanceJobSchema
            {
                SchemaId = GetInt(item, "_key") ?? 0,
                RawJson = item.GetRawText()
            };
        }
    }

    public async IAsyncEnumerable<RawMercenaryTacticalOperation> ReadMercenaryTacticalOperationsAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "mercenaryTacticalOperations.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonElement>(path, cancellationToken))
        {
            yield return new RawMercenaryTacticalOperation
            {
                OperationId = GetInt(item, "_key") ?? 0,
                NameEn = GetLocalizedEn(item, "name"),
                DescriptionEn = GetLocalizedEn(item, "description"),
                AnarchyImpact = GetInt(item, "anarchy_impact"),
                DevelopmentImpact = GetInt(item, "development_impact"),
                InfomorphBonus = GetInt(item, "infomorph_bonus"),
                RawJson = item.GetRawText()
            };
        }
    }

    public async IAsyncEnumerable<RawPlanetResource> ReadPlanetResourcesAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "planetResources.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonElement>(path, cancellationToken))
        {
            yield return new RawPlanetResource
            {
                TypeId = GetInt(item, "_key") ?? 0,
                Power = GetInt(item, "power"),
                Workforce = GetInt(item, "workforce"),
                RawJson = item.GetRawText()
            };
        }
    }

    public async IAsyncEnumerable<RawSovereigntyUpgrade> ReadSovereigntyUpgradesAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "sovereigntyUpgrades.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonElement>(path, cancellationToken))
        {
            int? fuelTypeId = null;
            int? fuelHourlyUpkeep = null;
            int? fuelStartupCost = null;
            if (item.TryGetProperty("fuel", out var fuel) && fuel.ValueKind == JsonValueKind.Object)
            {
                fuelTypeId = GetInt(fuel, "type_id");
                fuelHourlyUpkeep = GetInt(fuel, "hourly_upkeep");
                fuelStartupCost = GetInt(fuel, "startup_cost");
            }

            yield return new RawSovereigntyUpgrade
            {
                TypeId = GetInt(item, "_key") ?? 0,
                MutuallyExclusiveGroup = GetString(item, "mutually_exclusive_group"),
                PowerAllocation = GetInt(item, "power_allocation"),
                WorkforceAllocation = GetInt(item, "workforce_allocation"),
                FuelTypeId = fuelTypeId,
                FuelHourlyUpkeep = fuelHourlyUpkeep,
                FuelStartupCost = fuelStartupCost,
                RawJson = item.GetRawText()
            };
        }
    }

    public async IAsyncEnumerable<RawTypeBonus> ReadTypeBonusesAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "typeBonus.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonElement>(path, cancellationToken))
        {
            yield return new RawTypeBonus
            {
                TypeId = GetInt(item, "_key") ?? 0,
                RawJson = item.GetRawText()
            };
        }
    }

    public async IAsyncEnumerable<RawSdeBuildInfo> ReadSdeBuildInfoAsync(string rootDirectory, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var path = Resolve(rootDirectory, "_sde.jsonl");
        await foreach (var item in _fileReader.ReadAsync<JsonElement>(path, cancellationToken))
        {
            DateTimeOffset? releaseDate = null;
            var releaseDateText = GetString(item, "releaseDate");
            if (!string.IsNullOrWhiteSpace(releaseDateText) && DateTimeOffset.TryParse(releaseDateText, out var parsed))
            {
                releaseDate = parsed.ToUniversalTime();
            }

            yield return new RawSdeBuildInfo
            {
                SourceKey = GetString(item, "_key"),
                BuildNumber = GetInt(item, "buildNumber"),
                ReleaseDateUtc = releaseDate
            };
        }
    }

    private static RawBlueprintActivity ParseBlueprintActivity(JsonElement element, int activityId)
    {
        int? time = null;
        if (element.TryGetProperty("time", out var timeEl) && timeEl.ValueKind == JsonValueKind.Number && timeEl.TryGetInt32(out var timeValue))
        {
            time = timeValue;
        }

        var materials = new List<RawBlueprintMaterial>();
        if (element.TryGetProperty("materials", out var materialsEl) && materialsEl.ValueKind == JsonValueKind.Array)
        {
            foreach (var material in materialsEl.EnumerateArray())
            {
                if (TryGetInt(material, "typeID", out var typeId) && TryGetInt(material, "quantity", out var quantity))
                {
                    materials.Add(new RawBlueprintMaterial { TypeId = typeId, Quantity = quantity });
                }
            }
        }

        var products = new List<RawBlueprintProduct>();
        if (element.TryGetProperty("products", out var productsEl) && productsEl.ValueKind == JsonValueKind.Array)
        {
            foreach (var product in productsEl.EnumerateArray())
            {
                if (TryGetInt(product, "typeID", out var typeId) && TryGetInt(product, "quantity", out var quantity))
                {
                    double? probability = null;
                    if (product.TryGetProperty("probability", out var probabilityEl) && probabilityEl.ValueKind == JsonValueKind.Number)
                    {
                        probability = probabilityEl.GetDouble();
                    }

                    products.Add(new RawBlueprintProduct { TypeId = typeId, Quantity = quantity, Probability = probability });
                }
            }
        }

        var skills = new List<RawBlueprintSkill>();
        if (element.TryGetProperty("skills", out var skillsEl) && skillsEl.ValueKind == JsonValueKind.Array)
        {
            foreach (var skill in skillsEl.EnumerateArray())
            {
                if (TryGetInt(skill, "typeID", out var typeId) && TryGetInt(skill, "level", out var level))
                {
                    skills.Add(new RawBlueprintSkill { TypeId = typeId, Level = level });
                }
            }
        }

        return new RawBlueprintActivity
        {
            ActivityId = activityId,
            Time = time,
            Materials = materials,
            Products = products,
            Skills = skills
        };
    }

    private static bool TryGetInt(JsonElement element, string propertyName, out int value)
    {
        value = default;
        return element.TryGetProperty(propertyName, out var prop)
               && prop.ValueKind == JsonValueKind.Number
               && prop.TryGetInt32(out value);
    }

    private static int? GetInt(JsonElement element, string propertyName)
    {
        if (!element.TryGetProperty(propertyName, out var property) || property.ValueKind != JsonValueKind.Number)
        {
            return null;
        }

        return property.TryGetInt32(out var value) ? value : null;
    }

    private static string? GetString(JsonElement element, string propertyName)
    {
        if (!element.TryGetProperty(propertyName, out var property) || property.ValueKind != JsonValueKind.String)
        {
            return null;
        }

        return property.GetString();
    }

    private static string? GetLocalizedEn(JsonElement element, string propertyName)
    {
        if (!element.TryGetProperty(propertyName, out var localized) || localized.ValueKind != JsonValueKind.Object)
        {
            return null;
        }

        return GetString(localized, "en");
    }

    private static RawCelestialAttributes? MapAttributes(JsonlMapCelestialAttributesDto? source)
    {
        if (source is null)
        {
            return null;
        }

        return new RawCelestialAttributes
        {
            HeightMap1 = source.HeightMap1,
            HeightMap2 = source.HeightMap2,
            ShaderPreset = source.ShaderPreset,
            Population = source.Population
        };
    }

    private static RawCelestialStatistics? MapStatistics(JsonlMapCelestialStatisticsDto? source)
    {
        if (source is null)
        {
            return null;
        }

        return new RawCelestialStatistics
        {
            Temperature = source.Temperature,
            SpectralClass = source.SpectralClass,
            Luminosity = source.Luminosity,
            Age = source.Age,
            Life = source.Life,
            OrbitRadius = source.OrbitRadius,
            Eccentricity = source.Eccentricity,
            MassDust = source.MassDust,
            MassGas = source.MassGas,
            Fragmented = source.Fragmented,
            Density = source.Density,
            SurfaceGravity = source.SurfaceGravity,
            EscapeVelocity = source.EscapeVelocity,
            OrbitPeriod = source.OrbitPeriod,
            RotationRate = source.RotationRate,
            Locked = source.Locked,
            Pressure = source.Pressure,
            Radius = source.Radius,
            Mass = source.Mass
        };
    }

    private Task<long> CountAsync(string rootDirectory, string fileName, CancellationToken cancellationToken)
        => _fileReader.CountLinesAsync(Resolve(rootDirectory, fileName), cancellationToken);

    private static string Resolve(string rootDirectory, string fileName)
        => FileSystem.SdeFileLocator.LocateRequiredFile(rootDirectory, fileName);
}
