using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Mapping;
using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Validation;

namespace HKSDEImporter.Core.ImportSteps;

public sealed class ImportNpcAgentDataStep : IImportStep
{
    private readonly IRawSdeReader _reader;
    private readonly NpcAgentMapper _mapper;
    private readonly IValidator<Agent> _agentValidator;
    private readonly IValidator<ResearchAgent> _researchAgentValidator;
    private readonly IValidator<NpcCorporationResearchField> _researchFieldValidator;
    private readonly IValidator<NpcCorporationTrade> _corporationTradeValidator;
    private readonly INpcAgentWriter _writer;
    private readonly IImportObserver _observer;

    public ImportNpcAgentDataStep(
        IRawSdeReader reader,
        NpcAgentMapper mapper,
        IValidator<Agent> agentValidator,
        IValidator<ResearchAgent> researchAgentValidator,
        IValidator<NpcCorporationResearchField> researchFieldValidator,
        IValidator<NpcCorporationTrade> corporationTradeValidator,
        INpcAgentWriter writer,
        IImportObserver observer)
    {
        _reader = reader;
        _mapper = mapper;
        _agentValidator = agentValidator;
        _researchAgentValidator = researchAgentValidator;
        _researchFieldValidator = researchFieldValidator;
        _corporationTradeValidator = corporationTradeValidator;
        _writer = writer;
        _observer = observer;
    }

    public string Name => "Import NPC Agent Data";

    public async Task ExecuteAsync(ImportContext context, CancellationToken cancellationToken)
    {
        var total = await _reader.CountNpcCharactersAsync(context.ResolvedInputDirectory!, cancellationToken)
                    + await _reader.CountNpcCorporationsAsync(context.ResolvedInputDirectory!, cancellationToken);
        _observer.OnStepStarted(Name, total);

        var agents = new List<Agent>();
        var researchAgents = new List<ResearchAgent>();
        var researchFields = new List<NpcCorporationResearchField>();
        var corpTrades = new List<NpcCorporationTrade>();

        var seenAgents = new HashSet<int>();
        var seenResearchAgents = new HashSet<string>(StringComparer.Ordinal);
        var seenResearchFields = new HashSet<string>(StringComparer.Ordinal);
        var seenCorpTrades = new HashSet<string>(StringComparer.Ordinal);

        var processed = 0L;

        await foreach (var raw in _reader.ReadNpcCharactersAsync(context.ResolvedInputDirectory!, cancellationToken))
        {
            processed++;
            if (processed % 2000 == 0) _observer.OnStepProgress(Name, processed);

            var mappedAgent = _mapper.MapAgent(raw);
            if (mappedAgent is not null && seenAgents.Add(mappedAgent.AgentId))
            {
                var result = _agentValidator.Validate(mappedAgent);
                if (result.IsValid)
                {
                    agents.Add(mappedAgent);
                }
                else
                {
                    AddWarning(context, result.Message ?? "Invalid agent row.");
                }
            }

            foreach (var research in _mapper.MapResearchAgents(raw))
            {
                var key = $"{research.AgentId}:{research.TypeId}";
                if (!seenResearchAgents.Add(key)) continue;

                var result = _researchAgentValidator.Validate(research);
                if (result.IsValid)
                {
                    researchAgents.Add(research);
                }
                else
                {
                    AddWarning(context, result.Message ?? "Invalid research agent row.");
                }
            }

            foreach (var field in _mapper.MapResearchFields(raw))
            {
                var key = $"{field.SkillId}:{field.CorporationId}";
                if (!seenResearchFields.Add(key)) continue;

                var result = _researchFieldValidator.Validate(field);
                if (result.IsValid)
                {
                    researchFields.Add(field);
                }
                else
                {
                    AddWarning(context, result.Message ?? "Invalid research field row.");
                }
            }
        }

        await foreach (var rawCorp in _reader.ReadNpcCorporationsAsync(context.ResolvedInputDirectory!, cancellationToken))
        {
            processed++;
            if (processed % 2000 == 0) _observer.OnStepProgress(Name, processed);

            foreach (var trade in _mapper.MapCorporationTrades(rawCorp))
            {
                var key = $"{trade.CorporationId}:{trade.TypeId}";
                if (!seenCorpTrades.Add(key)) continue;

                var result = _corporationTradeValidator.Validate(trade);
                if (result.IsValid)
                {
                    corpTrades.Add(trade);
                }
                else
                {
                    AddWarning(context, result.Message ?? "Invalid corporation trade row.");
                }
            }
        }

        await _writer.WriteAsync(context.Options.OutputPath, agents, researchAgents, researchFields, corpTrades, cancellationToken);

        context.SetRowCount("agtAgents", agents.Count);
        context.SetRowCount("agtResearchAgents", researchAgents.Count);
        context.SetRowCount("crpNPCCorporationResearchFields", researchFields.Count);
        context.SetRowCount("crpNPCCorporationTrades", corpTrades.Count);
        _observer.OnStepProgress(Name, processed);
    }

    private void AddWarning(ImportContext context, string warning)
    {
        context.AddWarning(warning);
        _observer.OnWarning(warning);
    }
}
