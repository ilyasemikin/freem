using System.Text.Json;
using Freem.Http.Requests.Entities;
using Freem.Http.Requests.Entities.Models.Contents.Extensions;
using Freem.Web.Api.Public.Client.Models;

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
            ? await response.Content.ReadJsonAsAsync<T>(options)
            : default;
        
        return new ClientResult<T>(response.StatusCode, value);
    }
}