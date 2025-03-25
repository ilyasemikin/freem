using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace Freem.Web.Api.Public.OpenApi.Headers;

internal class HeadersOpenApiOperationTransformer : IOpenApiOperationTransformer
{
    public Task TransformAsync(
        OpenApiOperation operation, 
        OpenApiOperationTransformerContext context,
        CancellationToken cancellationToken)
    {
        if (context.Description.ActionDescriptor is not ControllerActionDescriptor controller)
            return Task.CompletedTask;

        var attributes = controller.ControllerTypeInfo.GetCustomAttributes<ProducesHeaderAttribute>();
        
        foreach (var attribute in attributes)
        foreach (var (statusCode, response) in operation.Responses)
        {
            if (attribute.StatusCode == default || statusCode != attribute.StatusCode.ToString())
                continue;

            var header = new OpenApiHeader();
            response.Headers.Add(attribute.HeaderName, header);
        }
        
        return Task.CompletedTask;
    }
}