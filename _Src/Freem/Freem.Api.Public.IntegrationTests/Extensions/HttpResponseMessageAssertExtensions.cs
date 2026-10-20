using System.Net;
using System.Text.Json;
using Utilities.Errors;

namespace Freem.Api.Public.IntegrationTests.Extensions;

// For raw requests the typed client cannot express; reads the error type straight from the ErrorResponse JSON.
public static class HttpResponseMessageAssertExtensions
{
    public static async Task ShouldHaveErrorAsync(this HttpResponseMessage response, HttpStatusCode statusCode, ErrorType type)
    {
        var body = await response.Content.ReadAsStringAsync();
        using var document = JsonDocument.Parse(body);
        var actualType = document.RootElement
            .GetProperty("error")
            .GetProperty("type")
            .GetString();

        Assert.Equal(statusCode, response.StatusCode);
        Assert.Equal(type.ToString(), actualType);
    }
}
