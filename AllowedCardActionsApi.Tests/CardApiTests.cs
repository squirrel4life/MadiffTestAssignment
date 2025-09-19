using FluentAssertions;
using MadiffTestAssignment.Models;
using Moq;
using System.Net.Http.Json;
using System.Net;

namespace MadiffTestAssignment.Tests;

public class CardApiTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly CustomWebApplicationFactory _factory = factory;

    [Fact]
    public async Task ReturnsCardActions_WhenCardExists()
    {
        var userId = "UserX";
        var cardNumber = "Card123";
        var card = new CardDetails(cardNumber, CardType.Prepaid, CardStatus.Closed, false);

        _factory.CardServiceMock
            .Setup(s => s.GetCardDetails(userId, cardNumber))
            .ReturnsAsync(card);

        _factory.AllowedActionsGeneratorMock
            .Setup(aag => aag.GenerateAllowedActions(card))
            .Returns(["ACTION3", "ACTION4", "ACTION9"]);

        var response = await _client.GetAsync($"/api/card/actions?userId={userId}&cardNumber={cardNumber}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<CardResponse>();

        result!.User.Should().Be(userId);
        result.Card.Should().Be(cardNumber);
        result.Type.Should().Be("Prepaid");
        result.Status.Should().Be("Closed");
        result.Pin.Should().Be(false);
        result.Actions.Should().BeEquivalentTo("ACTION3", "ACTION4", "ACTION9");
    }

    [Fact]
    public async Task Returns404_WhenCardNotFound()
    {
        _factory.CardServiceMock
            .Setup(s => s.GetCardDetails("X", "Y"))
            .ReturnsAsync((CardDetails?)null);

        var response = await _client.GetAsync("/api/card/actions?userId=X&cardNumber=Y");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var body = await response.Content.ReadAsStringAsync();
        body.Should().Contain("X albo nie posiada karty o numerze Y, albo nie istnieje");
    }

    [Fact]
    public async Task Returns422_WhenMissingQueryParameters()
    {
        var response = await _client.GetAsync("/api/card/actions");

        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);

        var json = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        json!.Error.Should().Contain("userId");
    }

    private record CardResponse(string User, string Card, string Type, string Status, bool Pin, List<string> Actions);
}