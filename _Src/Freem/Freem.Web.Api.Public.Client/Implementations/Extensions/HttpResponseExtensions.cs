using System.Text.Json;
using Freem.Web.Api.Public.Client.Models;
using Utilities.Http;
using Utilities.Http.Content.Extensions;

namespace Freem.Web.Api.Public.Client.Implementations.Extensions;

internal static class HttpResponseExtensions
{
    public static ClientResult ToClientResult(this HttpResponse response)
    {
        return new ClientResult(response.StatusCode);
    }

    public static async Task<ClientResult<T>> ToClientResultAsync<T>(this HttpResponse response, JsonSerializerOptions? options = null)
    {
        var value = response.Success
            ? await response.Content.ReadFromJsonAsync<T>(options)
            : default;
        
        return new ClientResult<T>(response.StatusCode, value);
    }
}