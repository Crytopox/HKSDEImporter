using Microsoft.Data.Sqlite;

namespace HKSDEImporter.Infrastructure.Sqlite.Schema;

public sealed class SqliteSchemaBuilder
{
    public async Task CreateSchemaAsync(SqliteConnection connection, CancellationToken cancellationToken)
    {
        var commands = new[]
        {
            """
            CREATE TABLE IF NOT EXISTS invCategories (
                categoryID INTEGER PRIMARY KEY,
                categoryName TEXT NOT NULL,
                iconID INTEGER NULL,
                published INTEGER NOT NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS invGroups (
                groupID INTEGER PRIMARY KEY,
                categoryID INTEGER NOT NULL,
                groupName TEXT NOT NULL,
                iconID INTEGER NULL,
                useBasePrice INTEGER NOT NULL,
                anchored INTEGER NOT NULL,
                anchorable INTEGER NOT NULL,
                fittableNonSingleton INTEGER NOT NULL,
                published INTEGER NOT NULL,
                FOREIGN KEY (categoryID) REFERENCES invCategories(categoryID)
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS invTypes (
                typeID INTEGER PRIMARY KEY,
                groupID INTEGER NOT NULL,
                typeName TEXT NOT NULL,
                description TEXT NULL,
                mass REAL NULL,
                volume REAL NULL,
                capacity REAL NULL,
                portionSize INTEGER NOT NULL,
                raceID INTEGER NULL,
                basePrice DECIMAL(19,4) NULL,
                published INTEGER NOT NULL,
                marketGroupID INTEGER NULL,
                iconID INTEGER NULL,
                soundID INTEGER NULL,
                graphicID INTEGER NULL,
                radius REAL NULL,
                FOREIGN KEY (groupID) REFERENCES invGroups(groupID)
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS invMarketGroups (
                marketGroupID INTEGER PRIMARY KEY,
                parentGroupID INTEGER NULL,
                marketGroupName TEXT NOT NULL,
                description TEXT NULL,
                iconID INTEGER NULL,
                hasTypes INTEGER NOT NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS invMetaGroups (
                metaGroupID INTEGER PRIMARY KEY,
                metaGroupName TEXT NOT NULL,
                description TEXT NULL,
                iconID INTEGER NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS eveUnits (
                unitID INTEGER PRIMARY KEY,
                unitName TEXT NOT NULL,
                displayName TEXT NULL,
                description TEXT NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS dgmAttributeCategories (
                categoryID INTEGER PRIMARY KEY,
                categoryName TEXT NOT NULL,
                categoryDescription TEXT NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS dgmAttributeTypes (
                attributeID INTEGER PRIMARY KEY,
                attributeName TEXT NOT NULL,
                description TEXT NULL,
                iconID INTEGER NULL,
                defaultValue REAL NULL,
                published INTEGER NOT NULL,
                displayName TEXT NULL,
                unitID INTEGER NULL,
                stackable INTEGER NOT NULL,
                highIsGood INTEGER NOT NULL,
                categoryID INTEGER NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS dgmEffects (
                effectID INTEGER PRIMARY KEY,
                effectName TEXT NOT NULL,
                effectCategory INTEGER NULL,
                description TEXT NULL,
                guid TEXT NULL,
                iconID INTEGER NULL,
                isOffensive INTEGER NOT NULL,
                isAssistance INTEGER NOT NULL,
                durationAttributeID INTEGER NULL,
                dischargeAttributeID INTEGER NULL,
                rangeAttributeID INTEGER NULL,
                disallowAutoRepeat INTEGER NOT NULL,
                published INTEGER NOT NULL,
                displayName TEXT NULL,
                isWarpSafe INTEGER NOT NULL,
                rangeChance INTEGER NOT NULL,
                electronicChance INTEGER NOT NULL,
                propulsionChance INTEGER NOT NULL,
                distribution INTEGER NULL,
                modifierInfo TEXT NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS dgmTypeAttributes (
                typeID INTEGER NOT NULL,
                attributeID INTEGER NOT NULL,
                valueInt INTEGER NULL,
                valueFloat REAL NULL,
                PRIMARY KEY (typeID, attributeID)
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS dgmTypeEffects (
                typeID INTEGER NOT NULL,
                effectID INTEGER NOT NULL,
                isDefault INTEGER NOT NULL,
                PRIMARY KEY (typeID, effectID)
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS invTypeMaterials (
                typeID INTEGER NOT NULL,
                materialTypeID INTEGER NOT NULL,
                quantity INTEGER NOT NULL,
                PRIMARY KEY (typeID, materialTypeID)
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS industryBlueprints (
                typeID INTEGER PRIMARY KEY,
                maxProductionLimit INTEGER NOT NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS industryActivity (
                typeID INTEGER NOT NULL,
                activityID INTEGER NOT NULL,
                time INTEGER NOT NULL,
                PRIMARY KEY (typeID, activityID)
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS industryActivityMaterials (
                typeID INTEGER NOT NULL,
                activityID INTEGER NOT NULL,
                materialTypeID INTEGER NOT NULL,
                quantity INTEGER NOT NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS industryActivityProducts (
                typeID INTEGER NOT NULL,
                activityID INTEGER NOT NULL,
                productTypeID INTEGER NOT NULL,
                quantity INTEGER NOT NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS industryActivityProbabilities (
                typeID INTEGER NOT NULL,
                activityID INTEGER NOT NULL,
                productTypeID INTEGER NOT NULL,
                probability REAL NOT NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS industryActivitySkills (
                typeID INTEGER NOT NULL,
                activityID INTEGER NOT NULL,
                skillID INTEGER NOT NULL,
                level INTEGER NOT NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS planetSchematics (
                schematicID INTEGER PRIMARY KEY,
                schematicName TEXT NOT NULL,
                cycleTime INTEGER NOT NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS planetSchematicsPinMap (
                schematicID INTEGER NOT NULL,
                pinTypeID INTEGER NOT NULL,
                PRIMARY KEY (schematicID, pinTypeID)
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS planetSchematicsTypeMap (
                schematicID INTEGER NOT NULL,
                typeID INTEGER NOT NULL,
                quantity INTEGER NOT NULL,
                isInput INTEGER NOT NULL,
                PRIMARY KEY (schematicID, typeID)
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS eveGraphics (
                graphicID INTEGER PRIMARY KEY,
                sofFactionName VARCHAR(100) NULL,
                graphicFile VARCHAR(256) NULL,
                sofHullName VARCHAR(100) NULL,
                sofRaceName VARCHAR(100) NULL,
                description TEXT NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS eveIcons (
                iconID INTEGER PRIMARY KEY,
                iconFile VARCHAR(500) NULL,
                description TEXT NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS chrFactions (
                factionID INTEGER PRIMARY KEY,
                factionName VARCHAR(100) NULL,
                description VARCHAR(2000) NULL,
                raceIDs INTEGER NULL,
                solarSystemID INTEGER NULL,
                corporationID INTEGER NULL,
                sizeFactor FLOAT NULL,
                stationCount INTEGER NULL,
                stationSystemCount INTEGER NULL,
                militiaCorporationID INTEGER NULL,
                iconID INTEGER NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS chrRaces (
                raceID INTEGER PRIMARY KEY,
                raceName VARCHAR(100) NULL,
                description VARCHAR(1000) NULL,
                iconID INTEGER NULL,
                shortDescription VARCHAR(500) NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS chrBloodlines (
                bloodlineID INTEGER PRIMARY KEY,
                bloodlineName VARCHAR(100) NULL,
                raceID INTEGER NULL,
                description VARCHAR(1000) NULL,
                maleDescription VARCHAR(1000) NULL,
                femaleDescription VARCHAR(1000) NULL,
                shipTypeID INTEGER NULL,
                corporationID INTEGER NULL,
                perception INTEGER NULL,
                willpower INTEGER NULL,
                charisma INTEGER NULL,
                memory INTEGER NULL,
                intelligence INTEGER NULL,
                iconID INTEGER NULL,
                shortDescription VARCHAR(500) NULL,
                shortMaleDescription VARCHAR(500) NULL,
                shortFemaleDescription VARCHAR(500) NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS chrAncestries (
                ancestryID INTEGER PRIMARY KEY,
                ancestryName VARCHAR(100) NULL,
                bloodlineID INTEGER NULL,
                description VARCHAR(1000) NULL,
                perception INTEGER NULL,
                willpower INTEGER NULL,
                charisma INTEGER NULL,
                memory INTEGER NULL,
                intelligence INTEGER NULL,
                iconID INTEGER NULL,
                shortDescription VARCHAR(500) NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS chrAttributes (
                attributeID INTEGER PRIMARY KEY,
                attributeName VARCHAR(100) NULL,
                description VARCHAR(1000) NULL,
                iconID INTEGER NULL,
                shortDescription VARCHAR(500) NULL,
                notes VARCHAR(500) NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS crpActivities (
                activityID INTEGER PRIMARY KEY,
                activityName VARCHAR(100) NULL,
                description VARCHAR(1000) NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS crpNPCDivisions (
                divisionID INTEGER PRIMARY KEY,
                divisionName VARCHAR(100) NULL,
                description VARCHAR(1000) NULL,
                leaderType VARCHAR(100) NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS crpNPCCorporations (
                corporationID INTEGER PRIMARY KEY,
                size CHAR(1) NULL,
                extent CHAR(1) NULL,
                solarSystemID INTEGER NULL,
                investorID1 INTEGER NULL,
                investorShares1 INTEGER NULL,
                investorID2 INTEGER NULL,
                investorShares2 INTEGER NULL,
                investorID3 INTEGER NULL,
                investorShares3 INTEGER NULL,
                investorID4 INTEGER NULL,
                investorShares4 INTEGER NULL,
                friendID INTEGER NULL,
                enemyID INTEGER NULL,
                publicShares INTEGER NULL,
                initialPrice INTEGER NULL,
                minSecurity FLOAT NULL,
                scattered BOOLEAN NULL,
                fringe INTEGER NULL,
                corridor INTEGER NULL,
                hub INTEGER NULL,
                border INTEGER NULL,
                factionID INTEGER NULL,
                sizeFactor FLOAT NULL,
                stationCount INTEGER NULL,
                stationSystemCount INTEGER NULL,
                description VARCHAR(4000) NULL,
                iconID INTEGER NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS staOperations (
                activityID INTEGER NULL,
                operationID INTEGER PRIMARY KEY,
                operationName VARCHAR(100) NULL,
                description VARCHAR(1000) NULL,
                fringe REAL NULL,
                corridor REAL NULL,
                hub REAL NULL,
                border REAL NULL,
                ratio REAL NULL,
                caldariStationTypeID INTEGER NULL,
                minmatarStationTypeID INTEGER NULL,
                amarrStationTypeID INTEGER NULL,
                gallenteStationTypeID INTEGER NULL,
                joveStationTypeID INTEGER NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS staServices (
                serviceID INTEGER PRIMARY KEY,
                serviceName VARCHAR(100) NULL,
                description VARCHAR(1000) NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS staOperationServices (
                operationID INTEGER NOT NULL,
                serviceID INTEGER NOT NULL,
                PRIMARY KEY (operationID, serviceID)
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS staStations (
                stationID BIGINT PRIMARY KEY,
                security FLOAT NULL,
                dockingCostPerVolume FLOAT NULL,
                maxShipVolumeDockable FLOAT NULL,
                officeRentalCost INTEGER NULL,
                operationID INTEGER NULL,
                stationTypeID INTEGER NULL,
                corporationID INTEGER NULL,
                solarSystemID INTEGER NULL,
                constellationID INTEGER NULL,
                regionID INTEGER NULL,
                stationName VARCHAR(100) NULL,
                x FLOAT NULL,
                y FLOAT NULL,
                z FLOAT NULL,
                reprocessingEfficiency FLOAT NULL,
                reprocessingStationsTake FLOAT NULL,
                reprocessingHangarFlag INTEGER NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS mapRegions (
                regionID INTEGER PRIMARY KEY,
                regionName VARCHAR(100) NULL,
                x REAL NULL,
                y REAL NULL,
                z REAL NULL,
                factionID INTEGER NULL,
                nebulaID INTEGER NULL,
                wormholeClassID INTEGER NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS mapConstellations (
                constellationID INTEGER PRIMARY KEY,
                regionID INTEGER NULL,
                constellationName VARCHAR(100) NULL,
                x REAL NULL,
                y REAL NULL,
                z REAL NULL,
                factionID INTEGER NULL,
                wormholeClassID INTEGER NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS mapSolarSystems (
                solarSystemID INTEGER PRIMARY KEY,
                regionID INTEGER NULL,
                constellationID INTEGER NULL,
                solarSystemName VARCHAR(100) NULL,
                security REAL NULL,
                securityClass VARCHAR(10) NULL,
                factionID INTEGER NULL,
                starID INTEGER NULL,
                x REAL NULL,
                y REAL NULL,
                z REAL NULL,
                radius REAL NULL,
                luminosity REAL NULL,
                border INTEGER NOT NULL,
                fringe INTEGER NOT NULL,
                corridor INTEGER NOT NULL,
                hub INTEGER NOT NULL,
                international INTEGER NOT NULL,
                regional INTEGER NOT NULL,
                wormholeClassID INTEGER NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS mapJumps (
                stargateID INTEGER NULL,
                destinationStargateID INTEGER NULL,
                fromRegionID INTEGER NOT NULL,
                fromConstellationID INTEGER NOT NULL,
                fromSolarSystemID INTEGER NOT NULL,
                toRegionID INTEGER NOT NULL,
                toConstellationID INTEGER NOT NULL,
                toSolarSystemID INTEGER NOT NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS mapRegionJumps (
                fromRegionID INTEGER NOT NULL,
                toRegionID INTEGER NOT NULL,
                PRIMARY KEY (fromRegionID, toRegionID)
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS mapConstellationJumps (
                fromConstellationID INTEGER NOT NULL,
                toConstellationID INTEGER NOT NULL,
                PRIMARY KEY (fromConstellationID, toConstellationID)
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS mapSolarSystemJumps (
                fromSolarSystemID INTEGER NOT NULL,
                toSolarSystemID INTEGER NOT NULL,
                PRIMARY KEY (fromSolarSystemID, toSolarSystemID)
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS mapLandmarks (
                landmarkID INTEGER PRIMARY KEY,
                landmarkName VARCHAR(200) NULL,
                description TEXT NULL,
                iconID INTEGER NULL,
                x REAL NULL,
                y REAL NULL,
                z REAL NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS mapDenormalize (
                itemID INTEGER NOT NULL PRIMARY KEY,
                typeID INTEGER NULL,
                groupID INTEGER NULL,
                solarSystemID INTEGER NULL,
                constellationID INTEGER NULL,
                regionID INTEGER NULL,
                orbitID INTEGER NULL,
                x FLOAT NULL,
                y FLOAT NULL,
                z FLOAT NULL,
                radius FLOAT NULL,
                itemName VARCHAR(100) NULL,
                security FLOAT NULL,
                celestialIndex INTEGER NULL,
                orbitIndex INTEGER NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS mapCelestialGraphics (
                celestialID INTEGER NOT NULL PRIMARY KEY,
                heightMap1 INTEGER NULL,
                heightMap2 INTEGER NULL,
                shaderPreset INTEGER NULL,
                population BOOLEAN NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS mapCelestialStatistics (
                celestialID INTEGER NOT NULL PRIMARY KEY,
                temperature FLOAT NULL,
                spectralClass VARCHAR(10) NULL,
                luminosity FLOAT NULL,
                age FLOAT NULL,
                life FLOAT NULL,
                orbitRadius FLOAT NULL,
                eccentricity FLOAT NULL,
                massDust FLOAT NULL,
                massGas FLOAT NULL,
                fragmented BOOLEAN NULL,
                density FLOAT NULL,
                surfaceGravity FLOAT NULL,
                escapeVelocity FLOAT NULL,
                orbitPeriod FLOAT NULL,
                rotationRate FLOAT NULL,
                locked BOOLEAN NULL,
                pressure FLOAT NULL,
                radius FLOAT NULL,
                mass INTEGER NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS mapUniverse (
                universeID INTEGER NOT NULL PRIMARY KEY,
                universeName VARCHAR(100) NULL,
                x FLOAT NULL,
                y FLOAT NULL,
                z FLOAT NULL,
                xMin FLOAT NULL,
                xMax FLOAT NULL,
                yMin FLOAT NULL,
                yMax FLOAT NULL,
                zMin FLOAT NULL,
                zMax FLOAT NULL,
                radius FLOAT NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS invContrabandTypes (
                factionID INTEGER NOT NULL,
                typeID INTEGER NOT NULL,
                standingLoss FLOAT NULL,
                confiscateMinSec FLOAT NULL,
                fineByValue FLOAT NULL,
                attackMinSec FLOAT NULL,
                PRIMARY KEY (factionID, typeID)
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS invControlTowerResources (
                controlTowerTypeID INTEGER NOT NULL,
                resourceTypeID INTEGER NOT NULL,
                purpose INTEGER NULL,
                quantity INTEGER NULL,
                minSecurityLevel FLOAT NULL,
                factionID INTEGER NULL,
                PRIMARY KEY (controlTowerTypeID, resourceTypeID)
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS invControlTowerResourcePurposes (
                purpose INTEGER PRIMARY KEY,
                purposeText VARCHAR(100) NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS chrCloneGrades (
                cloneGradeID INTEGER PRIMARY KEY,
                cloneGradeName VARCHAR(100) NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS chrCloneGradeSkills (
                cloneGradeID INTEGER NOT NULL,
                typeID INTEGER NOT NULL,
                level INTEGER NOT NULL,
                PRIMARY KEY (cloneGradeID, typeID)
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS invCompressibleTypes (
                typeID INTEGER PRIMARY KEY,
                compressedTypeID INTEGER NOT NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS dgmBuffCollections (
                collectionID INTEGER PRIMARY KEY,
                aggregateMode VARCHAR(50) NULL,
                operationName VARCHAR(50) NULL,
                showOutputValueInUI VARCHAR(50) NULL,
                developerDescription TEXT NULL,
                displayName TEXT NULL,
                rawJson TEXT NOT NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS dgmDynamicItemAttributes (
                typeID INTEGER PRIMARY KEY,
                rawJson TEXT NOT NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS frtFreelanceJobSchemas (
                schemaID INTEGER PRIMARY KEY,
                rawJson TEXT NOT NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS mercenaryTacticalOperations (
                operationID INTEGER PRIMARY KEY,
                operationName TEXT NULL,
                description TEXT NULL,
                anarchyImpact INTEGER NULL,
                developmentImpact INTEGER NULL,
                infomorphBonus INTEGER NULL,
                rawJson TEXT NOT NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS planetResources (
                typeID INTEGER PRIMARY KEY,
                power INTEGER NULL,
                workforce INTEGER NULL,
                rawJson TEXT NOT NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS sovSovereigntyUpgrades (
                typeID INTEGER PRIMARY KEY,
                mutuallyExclusiveGroup VARCHAR(100) NULL,
                powerAllocation INTEGER NULL,
                workforceAllocation INTEGER NULL,
                fuelTypeID INTEGER NULL,
                fuelHourlyUpkeep INTEGER NULL,
                fuelStartupCost INTEGER NULL,
                rawJson TEXT NOT NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS invTraits (
                typeID INTEGER PRIMARY KEY,
                rawJson TEXT NOT NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS trnTranslationLanguages (
                numericLanguageID INTEGER PRIMARY KEY,
                languageID VARCHAR(50) NULL,
                languageName VARCHAR(200) NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS agtAgentTypes (
                agentTypeID INTEGER PRIMARY KEY,
                agentType VARCHAR(50) NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS agtAgentsInSpace (
                agentID INTEGER PRIMARY KEY,
                dungeonID INTEGER NULL,
                solarSystemID INTEGER NULL,
                spawnPointID INTEGER NULL,
                typeID INTEGER NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS agtAgents (
                agentID INTEGER PRIMARY KEY,
                divisionID INTEGER NULL,
                corporationID INTEGER NULL,
                locationID INTEGER NULL,
                level INTEGER NULL,
                quality INTEGER NULL,
                agentTypeID INTEGER NULL,
                isLocator BOOLEAN NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS agtResearchAgents (
                agentID INTEGER NOT NULL,
                typeID INTEGER NOT NULL,
                PRIMARY KEY (agentID, typeID)
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS crpNPCCorporationResearchFields (
                skillID INTEGER NOT NULL,
                corporationID INTEGER NOT NULL,
                PRIMARY KEY (skillID, corporationID)
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS crpNPCCorporationTrades (
                corporationID INTEGER NOT NULL,
                typeID INTEGER NOT NULL,
                PRIMARY KEY (corporationID, typeID)
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS certCerts (
                certID INTEGER PRIMARY KEY,
                description TEXT NULL,
                groupID INTEGER NULL,
                name VARCHAR(255) NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS certMasteries (
                typeID INTEGER NOT NULL,
                masteryLevel INTEGER NOT NULL,
                certID INTEGER NOT NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS certSkills (
                certID INTEGER NOT NULL,
                skillID INTEGER NOT NULL,
                certLevelInt INTEGER NOT NULL,
                skillLevel INTEGER NOT NULL,
                certLevelText VARCHAR(8) NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS skins (
                skinID INTEGER PRIMARY KEY,
                internalName VARCHAR(70) NULL,
                skinMaterialID INTEGER NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS skinMaterials (
                skinMaterialID INTEGER PRIMARY KEY,
                displayNameID INTEGER NULL,
                materialSetID INTEGER NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS skinShip (
                skinID INTEGER NOT NULL,
                typeID INTEGER NOT NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS skinLicense (
                licenseTypeID INTEGER PRIMARY KEY,
                duration INTEGER NULL,
                skinID INTEGER NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS crpNPCCorporationDivisions (
                corporationID INTEGER NOT NULL,
                divisionID INTEGER NOT NULL,
                size INTEGER NULL,
                PRIMARY KEY (corporationID, divisionID)
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS invMetaTypes (
                typeID INTEGER PRIMARY KEY,
                parentTypeID INTEGER NULL,
                metaGroupID INTEGER NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS invNames (
                itemID INTEGER PRIMARY KEY,
                itemName VARCHAR(200) NOT NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS invPositions (
                itemID INTEGER PRIMARY KEY,
                x FLOAT NOT NULL,
                y FLOAT NOT NULL,
                z FLOAT NOT NULL,
                yaw FLOAT NULL,
                pitch FLOAT NULL,
                roll FLOAT NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS invTypeReactions (
                reactionTypeID INTEGER NOT NULL,
                input BOOLEAN NOT NULL,
                typeID INTEGER NOT NULL,
                quantity INTEGER NULL,
                PRIMARY KEY (reactionTypeID, input, typeID)
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS invUniqueNames (
                itemID INTEGER PRIMARY KEY,
                itemName VARCHAR(200) NOT NULL,
                groupID INTEGER NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS invVolumes (
                typeID INTEGER PRIMARY KEY,
                volume INTEGER NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS mapLocationScenes (
                locationID INTEGER PRIMARY KEY,
                graphicID INTEGER NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS mapLocationWormholeClasses (
                locationID INTEGER PRIMARY KEY,
                wormholeClassID INTEGER NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS _hki_import_metadata (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                started_at_utc TEXT NOT NULL,
                completed_at_utc TEXT NOT NULL,
                duration_ms INTEGER NOT NULL,
                row_counts_json TEXT NOT NULL,
                warning_count INTEGER NOT NULL,
                error_count INTEGER NOT NULL,
                warnings_json TEXT NULL,
                errors_json TEXT NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS _hki_sde_build (
                source_key TEXT PRIMARY KEY,
                build_number INTEGER NOT NULL,
                release_date_utc TEXT NOT NULL
            );
            """
        };

        foreach (var sql in commands)
        {
            using var command = connection.CreateCommand();
            command.CommandText = sql;
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    public async Task CreateIndexesAsync(SqliteConnection connection, CancellationToken cancellationToken)
    {
        var commands = new[]
        {
            "CREATE INDEX IF NOT EXISTS idx_invGroups_categoryID ON invGroups(categoryID);",
            "CREATE INDEX IF NOT EXISTS idx_invTypes_groupID ON invTypes(groupID);",
            "CREATE INDEX IF NOT EXISTS idx_invTypes_typeName ON invTypes(typeName);",
            "CREATE INDEX IF NOT EXISTS idx_invMarketGroups_parentGroupID ON invMarketGroups(parentGroupID);",
            "CREATE INDEX IF NOT EXISTS idx_dgmAttributeTypes_categoryID ON dgmAttributeTypes(categoryID);",
            "CREATE INDEX IF NOT EXISTS idx_dgmTypeAttributes_attributeID ON dgmTypeAttributes(attributeID);",
            "CREATE INDEX IF NOT EXISTS idx_dgmTypeEffects_effectID ON dgmTypeEffects(effectID);",
            "CREATE INDEX IF NOT EXISTS idx_invTypeMaterials_materialTypeID ON invTypeMaterials(materialTypeID);",
            "CREATE INDEX IF NOT EXISTS idx_industryActivity_activityID ON industryActivity(activityID);",
            "CREATE INDEX IF NOT EXISTS idx_industryActivityMaterials_materialTypeID ON industryActivityMaterials(materialTypeID);",
            "CREATE INDEX IF NOT EXISTS idx_industryActivityProducts_productTypeID ON industryActivityProducts(productTypeID);",
            "CREATE INDEX IF NOT EXISTS idx_industryActivitySkills_skillID ON industryActivitySkills(skillID);",
            "CREATE INDEX IF NOT EXISTS idx_planetSchematicsTypeMap_typeID ON planetSchematicsTypeMap(typeID);",
            "CREATE INDEX IF NOT EXISTS idx_chrFactions_factionName ON chrFactions(factionName);",
            "CREATE INDEX IF NOT EXISTS idx_chrRaces_raceName ON chrRaces(raceName);",
            "CREATE INDEX IF NOT EXISTS idx_chrBloodlines_raceID ON chrBloodlines(raceID);",
            "CREATE INDEX IF NOT EXISTS idx_chrAncestries_bloodlineID ON chrAncestries(bloodlineID);",
            "CREATE INDEX IF NOT EXISTS idx_crpNPCCorporations_factionID ON crpNPCCorporations(factionID);",
            "CREATE INDEX IF NOT EXISTS idx_staOperationServices_serviceID ON staOperationServices(serviceID);",
            "CREATE INDEX IF NOT EXISTS idx_staStations_solarSystemID ON staStations(solarSystemID);",
            "CREATE INDEX IF NOT EXISTS idx_staStations_corporationID ON staStations(corporationID);",
            "CREATE INDEX IF NOT EXISTS idx_mapConstellations_regionID ON mapConstellations(regionID);",
            "CREATE INDEX IF NOT EXISTS idx_mapSolarSystems_regionID ON mapSolarSystems(regionID);",
            "CREATE INDEX IF NOT EXISTS idx_mapSolarSystems_constellationID ON mapSolarSystems(constellationID);",
            "CREATE INDEX IF NOT EXISTS idx_mapSolarSystems_security ON mapSolarSystems(security);",
            "CREATE INDEX IF NOT EXISTS idx_mapJumps_fromSolarSystemID ON mapJumps(fromSolarSystemID);",
            "CREATE INDEX IF NOT EXISTS idx_mapJumps_toSolarSystemID ON mapJumps(toSolarSystemID);",
            "CREATE INDEX IF NOT EXISTS idx_mapRegionJumps_toRegionID ON mapRegionJumps(toRegionID);",
            "CREATE INDEX IF NOT EXISTS idx_mapConstellationJumps_toConstellationID ON mapConstellationJumps(toConstellationID);",
            "CREATE INDEX IF NOT EXISTS idx_mapSolarSystemJumps_toSolarSystemID ON mapSolarSystemJumps(toSolarSystemID);",
            "CREATE INDEX IF NOT EXISTS idx_mapDenormalize_typeID ON mapDenormalize(typeID);",
            "CREATE INDEX IF NOT EXISTS idx_mapDenormalize_solarSystemID ON mapDenormalize(solarSystemID);",
            "CREATE INDEX IF NOT EXISTS idx_mapDenormalize_constellationID ON mapDenormalize(constellationID);",
            "CREATE INDEX IF NOT EXISTS idx_mapDenormalize_regionID ON mapDenormalize(regionID);",
            "CREATE INDEX IF NOT EXISTS idx_mapDenormalize_orbitID ON mapDenormalize(orbitID);",
            "CREATE INDEX IF NOT EXISTS idx_mapDenormalize_groupSystem ON mapDenormalize(groupID, solarSystemID);",
            "CREATE INDEX IF NOT EXISTS idx_mapDenormalize_groupConstellation ON mapDenormalize(groupID, constellationID);",
            "CREATE INDEX IF NOT EXISTS idx_mapDenormalize_groupRegion ON mapDenormalize(groupID, regionID);",
            "CREATE INDEX IF NOT EXISTS idx_invContrabandTypes_typeID ON invContrabandTypes(typeID);",
            "CREATE INDEX IF NOT EXISTS idx_invControlTowerResources_resourceTypeID ON invControlTowerResources(resourceTypeID);",
            "CREATE INDEX IF NOT EXISTS idx_chrCloneGradeSkills_typeID ON chrCloneGradeSkills(typeID);",
            "CREATE INDEX IF NOT EXISTS idx_invCompressibleTypes_compressedTypeID ON invCompressibleTypes(compressedTypeID);",
            "CREATE INDEX IF NOT EXISTS idx_agtAgentsInSpace_solarSystemID ON agtAgentsInSpace(solarSystemID);",
            "CREATE INDEX IF NOT EXISTS idx_certMasteries_typeID ON certMasteries(typeID);",
            "CREATE INDEX IF NOT EXISTS idx_certSkills_skillID ON certSkills(skillID);",
            "CREATE INDEX IF NOT EXISTS idx_skinShip_typeID ON skinShip(typeID);",
            "CREATE INDEX IF NOT EXISTS idx_skins_skinMaterialID ON skins(skinMaterialID);",
            "CREATE INDEX IF NOT EXISTS idx_agtAgents_corporationID ON agtAgents(corporationID);",
            "CREATE INDEX IF NOT EXISTS idx_agtAgents_locationID ON agtAgents(locationID);",
            "CREATE INDEX IF NOT EXISTS idx_agtResearchAgents_typeID ON agtResearchAgents(typeID);",
            "CREATE INDEX IF NOT EXISTS idx_crpNPCCorporationResearchFields_corporationID ON crpNPCCorporationResearchFields(corporationID);",
            "CREATE INDEX IF NOT EXISTS idx_crpNPCCorporationTrades_typeID ON crpNPCCorporationTrades(typeID);",
            "CREATE INDEX IF NOT EXISTS idx_invMetaTypes_parentTypeID ON invMetaTypes(parentTypeID);",
            "CREATE INDEX IF NOT EXISTS idx_invMetaTypes_metaGroupID ON invMetaTypes(metaGroupID);",
            "CREATE INDEX IF NOT EXISTS idx_invNames_itemName ON invNames(itemName);",
            "CREATE INDEX IF NOT EXISTS idx_invUniqueNames_groupID ON invUniqueNames(groupID);",
            "CREATE INDEX IF NOT EXISTS idx_invTypeReactions_typeID ON invTypeReactions(typeID);",
            "CREATE INDEX IF NOT EXISTS idx_crpNPCCorporationDivisions_divisionID ON crpNPCCorporationDivisions(divisionID);",
            "CREATE INDEX IF NOT EXISTS idx_mapLocationWormholeClasses_wormholeClassID ON mapLocationWormholeClasses(wormholeClassID);"
        };

        foreach (var sql in commands)
        {
            using var command = connection.CreateCommand();
            command.CommandText = sql;
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }
}
