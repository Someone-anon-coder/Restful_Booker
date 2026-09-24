using Reqnroll;
using RestfulBooker.Tests.Clients;

namespace RestfulBooker.Tests.Support;

[Binding]
public class Hooks(ScenarioState state)
{
    readonly BookingClient bookingClient = new(ApiClientFactory.CreateClient());
    readonly AuthClient authClient = new(ApiClientFactory.CreateClient());

    [BeforeTestRun]
    public static async Task OnceBeforeAnything()
    {
        BookingClient client = new(ApiClientFactory.CreateClient());
        var response = await client.PingAsync();

        if (((int)response.StatusCode) != 201)
        {
            Assert.Fail("API is not reachable, aborting test run");
        }
    }

    [BeforeScenario("requires_auth")]
    public async Task BeforeTaggedScenario()
    {
        var response = await authClient.CreateTokenAsync(TestConfig.Username, TestConfig.Password);
        state.Token = response.Data?.Token;
    }

    [AfterScenario]
    public async Task AfterAllScenario()
    {
        foreach (var id in state.CreatedBookingIds)
        {
            try
            {
                await bookingClient.DeleteAsync(id, state.Token);
            }
            catch (System.Exception)
            {
                Console.Write("Could not delete Booking: " + id);
            }
        }
    }
}