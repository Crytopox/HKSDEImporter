using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Mapping;
using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Models.Raw;
using HKSDEImporter.Core.Validation;

namespace HKSDEImporter.Core.ImportSteps;

public sealed class ImportAgentReferencesStep : IImportStep
{
    private readonly IRawSdeReader _reader;
    private readonly IMapper<RawAgentType, AgentType> _agentTypeMapper;
    private readonly IMapper<RawAgentInSpace, AgentInSpace> _agentInSpaceMapper;
    private readonly IValidator<AgentType> _agentTypeValidator;
    private readonly IValidator<AgentInSpace> _agentInSpaceValidator;
    private readonly IAgentReferenceWriter _writer;
    private readonly IImportObserver _observer;

    public ImportAgentReferencesStep(IRawSdeReader reader, IMapper<RawAgentType, AgentType> agentTypeMapper, IMapper<RawAgentInSpace, AgentInSpace> agentInSpaceMapper, IValidator<AgentType> agentTypeValidator, IValidator<AgentInSpace> agentInSpaceValidator, IAgentReferenceWriter writer, IImportObserver observer)
    {
        _reader = reader;
        _agentTypeMapper = agentTypeMapper;
        _agentInSpaceMapper = agentInSpaceMapper;
        _agentTypeValidator = agentTypeValidator;
        _agentInSpaceValidator = agentInSpaceValidator;
        _writer = writer;
        _observer = observer;
    }

    public string Name => "Import Agent References";

    public async Task ExecuteAsync(ImportContext context, CancellationToken cancellationToken)
    {
        var total = await _reader.CountAgentTypesAsync(context.ResolvedInputDirectory!, cancellationToken)
                    + await _reader.CountAgentsInSpaceAsync(context.ResolvedInputDirectory!, cancellationToken);
        _observer.OnStepStarted(Name, total);

        var types = new List<AgentType>();
        var agents = new List<AgentInSpace>();

        var seenTypes = new HashSet<int>();
        var seenAgents = new HashSet<int>();
        var processed = 0L;

        await foreach (var raw in _reader.ReadAgentTypesAsync(context.ResolvedInputDirectory!, cancellationToken))
        {
            processed++;
            var mapped = _agentTypeMapper.Map(raw);
            if (!seenTypes.Add(mapped.AgentTypeId)) continue;

            var result = _agentTypeValidator.Validate(mapped);
            if (result.IsValid)
            {
                types.Add(mapped);
            }
            else
            {
                var warning = result.Message ?? "Invalid agent type row.";
                context.AddWarning(warning);
                _observer.OnWarning(warning);
            }
        }

        await foreach (var raw in _reader.ReadAgentsInSpaceAsync(context.ResolvedInputDirectory!, cancellationToken))
        {
            processed++;
            if (processed % 2000 == 0) _observer.OnStepProgress(Name, processed);

            var mapped = _agentInSpaceMapper.Map(raw);
            if (!seenAgents.Add(mapped.AgentId)) continue;

            var result = _agentInSpaceValidator.Validate(mapped);
            if (result.IsValid)
            {
                agents.Add(mapped);
            }
            else
            {
                var warning = result.Message ?? "Invalid agent-in-space row.";
                context.AddWarning(warning);
                _observer.OnWarning(warning);
            }
        }

        await _writer.WriteAsync(context.Options.OutputPath, types, agents, cancellationToken);

        context.SetRowCount("agtAgentTypes", types.Count);
        context.SetRowCount("agtAgentsInSpace", agents.Count);
        _observer.OnStepProgress(Name, processed);
    }
}
