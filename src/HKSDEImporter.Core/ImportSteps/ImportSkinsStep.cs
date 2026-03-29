using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Mapping;
using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Validation;

namespace HKSDEImporter.Core.ImportSteps;

public sealed class ImportSkinsStep : IImportStep
{
    private readonly IRawSdeReader _reader;
    private readonly SkinMapper _mapper;
    private readonly IValidator<Skin> _skinValidator;
    private readonly IValidator<SkinMaterial> _materialValidator;
    private readonly IValidator<SkinShip> _shipValidator;
    private readonly IValidator<SkinLicense> _licenseValidator;
    private readonly ISkinWriter _writer;
    private readonly IImportObserver _observer;

    public ImportSkinsStep(IRawSdeReader reader, SkinMapper mapper, IValidator<Skin> skinValidator, IValidator<SkinMaterial> materialValidator, IValidator<SkinShip> shipValidator, IValidator<SkinLicense> licenseValidator, ISkinWriter writer, IImportObserver observer)
    {
        _reader = reader;
        _mapper = mapper;
        _skinValidator = skinValidator;
        _materialValidator = materialValidator;
        _shipValidator = shipValidator;
        _licenseValidator = licenseValidator;
        _writer = writer;
        _observer = observer;
    }

    public string Name => "Import Skins";

    public async Task ExecuteAsync(ImportContext context, CancellationToken cancellationToken)
    {
        var total = await _reader.CountSkinsAsync(context.ResolvedInputDirectory!, cancellationToken)
                    + await _reader.CountSkinMaterialsAsync(context.ResolvedInputDirectory!, cancellationToken)
                    + await _reader.CountSkinLicensesAsync(context.ResolvedInputDirectory!, cancellationToken);
        _observer.OnStepStarted(Name, total);

        var skins = new List<Skin>();
        var materials = new List<SkinMaterial>();
        var ships = new List<SkinShip>();
        var licenses = new List<SkinLicense>();

        var seenSkin = new HashSet<int>();
        var seenMaterial = new HashSet<int>();
        var seenShip = new HashSet<string>(StringComparer.Ordinal);
        var seenLicense = new HashSet<int>();

        var processed = 0L;

        await foreach (var raw in _reader.ReadSkinsAsync(context.ResolvedInputDirectory!, cancellationToken))
        {
            processed++;
            if (processed % 1000 == 0) _observer.OnStepProgress(Name, processed);

            var skin = _mapper.MapSkin(raw);
            if (seenSkin.Add(skin.SkinId))
            {
                var skinResult = _skinValidator.Validate(skin);
                if (skinResult.IsValid) skins.Add(skin);
                else AddWarning(context, skinResult.Message ?? "Invalid skin row.");
            }

            foreach (var ship in _mapper.MapSkinShips(raw))
            {
                var key = $"{ship.SkinId}:{ship.TypeId}";
                if (!seenShip.Add(key)) continue;

                var shipResult = _shipValidator.Validate(ship);
                if (shipResult.IsValid) ships.Add(ship);
                else AddWarning(context, shipResult.Message ?? "Invalid skin ship row.");
            }
        }

        await foreach (var raw in _reader.ReadSkinMaterialsAsync(context.ResolvedInputDirectory!, cancellationToken))
        {
            processed++;
            var material = _mapper.MapMaterial(raw);
            if (!seenMaterial.Add(material.SkinMaterialId)) continue;

            var materialResult = _materialValidator.Validate(material);
            if (materialResult.IsValid) materials.Add(material);
            else AddWarning(context, materialResult.Message ?? "Invalid skin material row.");
        }

        await foreach (var raw in _reader.ReadSkinLicensesAsync(context.ResolvedInputDirectory!, cancellationToken))
        {
            processed++;
            var license = _mapper.MapLicense(raw);
            if (!seenLicense.Add(license.LicenseTypeId)) continue;

            var licenseResult = _licenseValidator.Validate(license);
            if (licenseResult.IsValid) licenses.Add(license);
            else AddWarning(context, licenseResult.Message ?? "Invalid skin license row.");
        }

        await _writer.WriteAsync(context.Options.OutputPath, skins, materials, ships, licenses, cancellationToken);

        context.SetRowCount("skins", skins.Count);
        context.SetRowCount("skinMaterials", materials.Count);
        context.SetRowCount("skinShip", ships.Count);
        context.SetRowCount("skinLicense", licenses.Count);
        _observer.OnStepProgress(Name, processed);
    }

    private void AddWarning(ImportContext context, string warning)
    {
        context.AddWarning(warning);
        _observer.OnWarning(warning);
    }
}
