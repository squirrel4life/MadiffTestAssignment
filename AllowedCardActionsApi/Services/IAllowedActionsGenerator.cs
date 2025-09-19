using MadiffTestAssignment.Models;

namespace MadiffTestAssignment.Services;

public interface IAllowedActionsGenerator
{
    List<string> GenerateAllowedActions(CardDetails details);
}