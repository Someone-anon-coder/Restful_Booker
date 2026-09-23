namespace RestfulBooker.Tests.Clients;

using RestSharp;
using RestfulBooker.Tests.Models;
using RestfulBooker.Tests.Support;

static class ApiClientFactory
{
    public static RestClient CreateClient()
    {
        var restClientOptions = new RestClientOptions(new Uri(TestConfig.BaseUrl));
        var restClient = new RestClient(restClientOptions);
        
        restClient.AddDefaultHeader("Accept", "application/json");
        restClient.AddDefaultHeader("Content-Type", "application/json");

        return restClient;
    }
}