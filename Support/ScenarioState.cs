using RestfulBooker.Tests.Models;
using RestSharp;

namespace RestfulBooker.Tests.Support;

public class ScenarioState
{
    public string? Token { get; set; }
    public int BookingId { get; set; }
    public Booking? LastCreatedBooking { get; set; }
    public RestResponse? LastResponse { get; set; }
    public RestResponse<AuthResponse>? LastAuthResponse { get; set; }
    public List<int> CreatedBookingIds { get; set; } = new();
}