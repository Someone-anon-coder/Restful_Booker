namespace RestfulBooker.Tests.Clients;

using RestSharp;
using RestfulBooker.Tests.Models;

public class BookingClient(RestClient client)
{
    public async Task<RestResponse<CreateBookingResponse>> CreateAsync(Booking booking)
    {
        var request = new RestRequest("/booking", Method.Post)
            .AddJsonBody(booking);
        
        var response = await client.ExecuteAsync<CreateBookingResponse>(request);
        return response;
    }

    public async Task<RestResponse<Booking>> GetAsync(int id)
    {
        var request = new RestRequest($"/booking/{id}", Method.Get);
        var response = await client.ExecuteAsync<Booking>(request);
        return response;
    }

    public async Task<RestResponse> UpdateAsync(int id, Booking booking, string? token)
    {
        var request = new RestRequest($"/booking/{id}", Method.Put)
            .AddJsonBody(booking);
        
        if (token != null)
        {
            request.AddHeader("Cookie", $"token={token}");
        }

        var response = await client.ExecuteAsync(request);
        return response;
    }

    public async Task<RestResponse> PatchAsync(int id, object partial, string? token)
    {
        var request = new RestRequest($"/booking/{id}", Method.Patch)
            .AddJsonBody(partial);
        
        if (token != null)
        {
            request.AddHeader("Cookie", $"token={token}");
        }
        
        var response = await client.ExecuteAsync(request);
        return response;
    }

    public async Task<RestResponse> DeleteAsync(int id, string? token)
    {
        var request = new RestRequest($"/booking/{id}", Method.Delete);
        if (token != null)
        {
            request.AddHeader("Cookie", $"token={token}");
        }

        var response = await client.ExecuteAsync(request);
        return response;
    }

    public async Task<RestResponse> PingAsync()
    {
        var request = new RestRequest("/ping", Method.Get);
        var response = await client.ExecuteAsync(request);
        return response;
    }
}