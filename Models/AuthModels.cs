using System.Text.Json.Serialization;
namespace RestfulBooker.Tests.Models;

public record AuthRequest(
    [property: JsonPropertyName("username")] string Username,
    [property: JsonPropertyName("password")] string Password
);

public record AuthResponse(
    [property: JsonPropertyName("token")] string? Token,
    [property: JsonPropertyName("reason")] string? Reason
);