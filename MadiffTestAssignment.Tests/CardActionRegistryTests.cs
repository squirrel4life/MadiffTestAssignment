using FluentAssertions;
using MadiffTestAssignment.Config;
using MadiffTestAssignment.Models;
using MadiffTestAssignment.Services;
using Microsoft.Extensions.Options;

namespace MadiffTestAssignment.Tests;

public class CardActionRegistryTests
{
    [Fact]
    public void ReturnsMatchingActions_ForValidCard()
    {
        // Arrange
        var config = new ActionRulesConfig
        {
            Rules =
            [
                new()
                {
                    CardType = CardType.Credit,
                    CardStatus = CardStatus.Blocked,
                    Actions = ["ACTION3", "ACTION5"],
                    RequirePin = null
                },
                new()
                {
                    CardType = CardType.Credit,
                    CardStatus = CardStatus.Blocked,
                    Actions = ["ACTION6"],
                    RequirePin = true
                }
            ]
        };

        var options = Options.Create(config);
        var registry = new CardActionRegistry(new OptionsMonitorStub<ActionRulesConfig>(options));

        var card = new CardDetails("C1", CardType.Credit, CardStatus.Blocked, true);

        // Act
        var result = registry.GetActions(card);

        // Assert
        result.Should().BeEquivalentTo("ACTION3", "ACTION5", "ACTION6");
    }

    [Fact]
    public void ReturnsEmptyList_ForUnknownCardType()
    {
        var config = new ActionRulesConfig(); // brak regu³
        var options = Options.Create(config);
        var registry = new CardActionRegistry(new OptionsMonitorStub<ActionRulesConfig>(options));

        var card = new CardDetails("C2", CardType.Debit, CardStatus.Closed, false);

        var result = registry.GetActions(card);
        result.Should().BeEmpty();
    }
}