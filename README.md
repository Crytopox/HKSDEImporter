# HKSDEImporter

C# `.NET 10` console app that imports EVE Online SDE JSONL into SQLite.

## Features (V1)
- Input modes:
  - `direct` (default): download latest JSONL from EVE official endpoint
  - `local`: import from a local JSONL folder
- Output: SQLite only
- Entity scope: `categories`, `groups`, `types`
- Step-based pipeline with mapping + validation + warnings
- Styled console UI with menu/prompts + progress + summary

## Build
```powershell
dotnet build src/HKSDEImporter.Console/HKSDEImporter.Console.csproj -m:1
```

## Run
Direct mode (recommended):
```powershell
dotnet run --project src/HKSDEImporter.Console/HKSDEImporter.Console.csproj -- --output ./eve.db --overwrite
```

Local mode:
```powershell
dotnet run --project src/HKSDEImporter.Console/HKSDEImporter.Console.csproj -- --source local --input ./.local-sde-copy --output ./eve.db --overwrite
```

## Test
```powershell
dotnet test tests/HKSDEImporter.Tests/HKSDEImporter.Tests.csproj -m:1
```

## Notes
- English localization is imported (`name.en`, `description.en`).
- Validation issues are warnings; structural parse failures fail the import.
