using MadiffTestAssignment.Services;
using Microsoft.AspNetCore.Mvc;

namespace MadiffTestAssignment.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CardController(ICardService cardService, IAllowedActionsGenerator allowedActionsGenerator) : Controller
{
    private readonly ICardService _cardService = cardService;
    private readonly IAllowedActionsGenerator _allowedActionsGenerator = allowedActionsGenerator;

    [HttpGet("actions")]
    public async Task<IActionResult> GetCardActions([FromQuery] string userId, [FromQuery] string cardNumber)
    {
        var card = await _cardService.GetCardDetails(userId, cardNumber);
        
        if (card is null)
        {
            return NotFound($"Użytkownik {userId} albo nie posiada karty o numerze {cardNumber}, albo nie istnieje");
        }

        var actions = _allowedActionsGenerator.GenerateAllowedActions(card);

        return Ok(new 
        { 
            user = userId, 
            card = cardNumber, 
            type = card.CardType, 
            pin = card.IsPinSet, 
            status = card.CardStatus, 
            actions 
        });
    }
}
