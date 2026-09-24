using Reqnroll;
using Reqnroll.BoDi;
using RestfulBooker.Tests.Clients;

namespace RestfulBooker.Tests.Support;

[Binding]
public class ClientRegistrations(IObjectContainer container)
{
    [BeforeScenario(Order = 0)]
    public void RegisterClients()
    {
        var restClient = ApiClientFactory.CreateClient();

        container.RegisterInstanceAs(restClient);
        container.RegisterInstanceAs(new AuthClient(restClient));
        container.RegisterInstanceAs(new BookingClient(restClient));
    }
}
