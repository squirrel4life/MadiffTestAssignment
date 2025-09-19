namespace MadiffTestAssignment.Models;

public record ActionRule(string ActionName, Func<CardDetails, bool> Condition);
