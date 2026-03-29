using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Mapping;
using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Models.Raw;
using HKSDEImporter.Core.Validation;

namespace HKSDEImporter.Core.ImportSteps;

public sealed class ImportStationReferencesStep : IImportStep
{
    private readonly IRawSdeReader _reader;
    private readonly StationMapper _mapper;
    private readonly IValidator<StationOperation> _operationValidator;
    private readonly IValidator<StationService> _serviceValidator;
    private readonly IValidator<Station> _stationValidator;
    private readonly IStationWriter _writer;
    private readonly IImportObserver _observer;

    public ImportStationReferencesStep(IRawSdeReader reader, StationMapper mapper, IValidator<StationOperation> operationValidator, IValidator<StationService> serviceValidator, IValidator<Station> stationValidator, IStationWriter writer, IImportObserver observer)
    {
        _reader = reader;
        _mapper = mapper;
        _operationValidator = operationValidator;
        _serviceValidator = serviceValidator;
        _stationValidator = stationValidator;
        _writer = writer;
        _observer = observer;
    }

    public string Name => "Import Station References";

    public async Task ExecuteAsync(ImportContext context, CancellationToken cancellationToken)
    {
        var total =
            await _reader.CountMapSolarSystemsAsync(context.ResolvedInputDirectory!, cancellationToken) +
            await _reader.CountStationOperationsAsync(context.ResolvedInputDirectory!, cancellationToken) +
            await _reader.CountStationServicesAsync(context.ResolvedInputDirectory!, cancellationToken) +
            await _reader.CountNpcStationsAsync(context.ResolvedInputDirectory!, cancellationToken);

        _observer.OnStepStarted(Name, total);

        var solarSystems = new Dictionary<int, RawMapSolarSystem>();
        var operations = new List<StationOperation>();
        var opServices = new List<StationOperationService>();
        var services = new List<StationService>();
        var stations = new List<Station>();

        var seenOperation = new HashSet<int>();
        var seenOpService = new HashSet<string>(StringComparer.Ordinal);
        var seenService = new HashSet<int>();
        var seenStation = new HashSet<long>();

        var processed = 0L;

        await foreach (var raw in _reader.ReadMapSolarSystemsAsync(context.ResolvedInputDirectory!, cancellationToken))
        {
            processed++;
            solarSystems[raw.Key] = raw;
        }

        await foreach (var raw in _reader.ReadStationOperationsAsync(context.ResolvedInputDirectory!, cancellationToken))
        {
            processed++;
            if (processed % 1000 == 0) _observer.OnStepProgress(Name, processed);

            var mapped = _mapper.MapOperation(raw);
            if (seenOperation.Add(mapped.OperationId))
            {
                var result = _operationValidator.Validate(mapped);
                if (result.IsValid)
                {
                    operations.Add(mapped);
                }
                else
                {
                    AddWarning(context, result.Message ?? "Invalid station operation row.");
                }
            }

            foreach (var row in _mapper.MapOperationServices(raw))
            {
                var key = $"{row.OperationId}:{row.ServiceId}";
                if (seenOpService.Add(key))
                {
                    opServices.Add(row);
                }
            }
        }

        await foreach (var raw in _reader.ReadStationServicesAsync(context.ResolvedInputDirectory!, cancellationToken))
        {
            processed++;
            if (processed % 1000 == 0) _observer.OnStepProgress(Name, processed);

            var mapped = _mapper.MapService(raw);
            if (!seenService.Add(mapped.ServiceId))
            {
                continue;
            }

            var result = _serviceValidator.Validate(mapped);
            if (result.IsValid)
            {
                services.Add(mapped);
            }
            else
            {
                AddWarning(context, result.Message ?? "Invalid station service row.");
            }
        }

        await foreach (var raw in _reader.ReadNpcStationsAsync(context.ResolvedInputDirectory!, cancellationToken))
        {
            processed++;
            if (processed % 2000 == 0) _observer.OnStepProgress(Name, processed);

            var mapped = _mapper.MapStation(raw, solarSystems);
            if (!seenStation.Add(mapped.StationId))
            {
                continue;
            }

            var result = _stationValidator.Validate(mapped);
            if (result.IsValid)
            {
                stations.Add(mapped);
            }
            else
            {
                AddWarning(context, result.Message ?? "Invalid station row.");
            }
        }

        await _writer.WriteAsync(context.Options.OutputPath, operations, opServices, services, stations, cancellationToken);

        context.SetRowCount("staOperations", operations.Count);
        context.SetRowCount("staOperationServices", opServices.Count);
        context.SetRowCount("staServices", services.Count);
        context.SetRowCount("staStations", stations.Count);
        _observer.OnStepProgress(Name, processed);
    }

    private void AddWarning(ImportContext context, string warning)
    {
        context.AddWarning(warning);
        _observer.OnWarning(warning);
    }
}
