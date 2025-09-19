using MadiffTestAssignment.Models;

namespace MadiffTestAssignment.Services;

public interface ICardService
{
    Task<CardDetails?> GetCardDetails(string userId, string cardNumber);
}