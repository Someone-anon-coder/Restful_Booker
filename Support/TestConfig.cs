namespace RestfulBooker.Tests.Support;

static class TestConfig
{
    public static readonly string BaseUrl = Environment
        .GetEnvironmentVariable("RESTFUL_BOOKER_BASE_URL") ?? "https://restful-booker.herokuapp.com";
    public static readonly string Username = Environment
        .GetEnvironmentVariable("RESTFUL_BOOKER_USERNAME") ?? "admin";
    public static readonly string Password = Environment
        .GetEnvironmentVariable("RESTFUL_BOOKER_PASSWORD") ?? "password123";
}