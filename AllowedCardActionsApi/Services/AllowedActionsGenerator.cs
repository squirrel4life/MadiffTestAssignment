using MadiffTestAssignment.Models;

namespace MadiffTestAssignment.Services;

public class AllowedActionsGenerator(ICardActionRegistry cardActionRegistry) : IAllowedActionsGenerator
{
    private readonly ICardActionRegistry _cardActionRegistry = cardActionRegistry;

    public List<string> GenerateAllowedActions(CardDetails details) => _cardActionRegistry.GetActions(details);
}
