using System.Text.Json.Serialization;
namespace RestfulBooker.Tests.Models;

public record BookingDates(
    [property: JsonPropertyName("checkin")] string Checkin,
    [property: JsonPropertyName("checkout")] string Checkout
);

public record Booking(
    [property: JsonPropertyName("firstname")] string Firstname,
    [property: JsonPropertyName("lastname")] string Lastname,
    [property: JsonPropertyName("totalprice")] int TotalPrice,
    [property: JsonPropertyName("depositpaid")] bool DepositPaid,
    [property: JsonPropertyName("bookingdates")] BookingDates BookingDates,
    [property: JsonPropertyName("additionalneeds")] string? AdditionalNeeds
);

public record CreateBookingResponse(
    [property: JsonPropertyName("bookingid")] int BookingId,
    [property: JsonPropertyName("booking")] Booking Booking
);