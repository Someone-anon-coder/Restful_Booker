namespace RestfulBooker.Tests.Clients;

using RestSharp;
using RestfulBooker.Tests.Models;

public class AuthClient(RestClient client)
{
    public async Task<RestResponse<AuthResponse>> CreateTokenAsync(string username, string password)
    {
        var request = new RestRequest("/auth", Method.Post)
            .AddJsonBody(new AuthRequest(username, password));
        
        var response = await client.ExecuteAsync<AuthResponse>(request);
        return response;
    }
}