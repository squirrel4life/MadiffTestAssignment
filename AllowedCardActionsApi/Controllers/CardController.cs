using MadiffTestAssignment.Exceptions;
using MadiffTestAssignment.Services;
using Microsoft.AspNetCore.Mvc;
using MadiffTestAssignment.Util;
using MadiffTestAssignment.Models;

namespace MadiffTestAssignment.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CardController(ICardService cardService, IAllowedActionsGenerator allowedActionsGenerator) : Controller
{
    private readonly ICardService _cardService = cardService;
    private readonly IAllowedActionsGenerator _allowedActionsGenerator = allowedActionsGenerator;

    [HttpGet("actions")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCardActions([FromQuery] string userId, [FromQuery] string cardNumber)
    {
        var card = await _cardService.GetCardDetails(userId, cardNumber) ?? throw new KeyNotFoundException(string.Format(ErrorMessages.CardNotFound, userId, cardNumber));
        var actions = _allowedActionsGenerator.GenerateAllowedActions(card);

        if (actions is []) throw new AppException(ErrorMessages.NoRulesConfigured);

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
