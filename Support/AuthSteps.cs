using FluentAssertions;
using Reqnroll;
using RestfulBooker.Tests.Clients;
using RestSharp;

namespace RestfulBooker.Tests.Support;

[Binding]
public class AuthSteps(ScenarioState state)
{
    BookingClient bookingClient = new(ApiClientFactory.CreateClient());
    AuthClient authClient = new(ApiClientFactory.CreateClient());
    
    [Given("the API is available")]
    public async Task GivenTheAPIIsAvailable()
    {
        var response = await bookingClient.PingAsync();
        ((int)response.StatusCode).Should().Be(201);
    }
    
    [When("I authenticate with valid credentials")]
    public async Task WhenIAuthenticateWithValidCredentials()
    {
        var response = await authClient.CreateTokenAsync(TestConfig.Username, TestConfig.Password);
        
        state.LastAuthResponse = response;
        state.Token = response.Data?.Token;
    }
    
    [When("I authenticate with invalid credentials")]
    public async Task WhenIAuthenticateWithInvalidCredentials()
    {
        var response = await authClient.CreateTokenAsync("INCORRECT_USERNAME", "INCORRECT_PASSWORD");
        state.LastAuthResponse = response;
    }

    [Then("the response should contain a non-empty token")]
    public async Task ThenTheResponseShouldContainANon_EmptyToken()
    {
        state.Token.Should().NotBeNullOrEmpty();
    }
    
    [Then("the response should contain the reason {string}")]
    public async Task ThenTheResponseShouldContainTheReason(string reason)
    {
        state.LastAuthResponse!.Data!.Reason.Should().Be(reason);
    }
    
    [Then("the response should not contain a token")]
    public async Task ThenTheResponseShouldNotContainAToken()
    {
        state.Token.Should().BeNull();
    }
}