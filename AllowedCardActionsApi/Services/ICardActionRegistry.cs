using MadiffTestAssignment.Models;

namespace MadiffTestAssignment.Services
{
    public interface ICardActionRegistry
    {
        List<string> GetActions(CardDetails card);
    }
}