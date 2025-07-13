namespace MadiffTestAssignment.Models;

public record ErrorResponse(string Error, int Status, string? TraceId);