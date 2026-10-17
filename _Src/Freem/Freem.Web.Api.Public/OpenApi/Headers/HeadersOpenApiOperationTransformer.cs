using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

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

        if (operation.Responses is null)
            return Task.CompletedTask;

        var attributes = controller.ControllerTypeInfo.GetCustomAttributes<ProducesHeaderAttribute>();
        
        foreach (var attribute in attributes)
        foreach (var (statusCode, response) in operation.Responses)
        {
            if (attribute.StatusCode == default || statusCode != attribute.StatusCode.ToString())
                continue;

            if (response is not OpenApiResponse concreteResponse)
                continue;

            var header = new OpenApiHeader();
            concreteResponse.Headers ??= new Dictionary<string, IOpenApiHeader>();
            concreteResponse.Headers.Add(attribute.HeaderName, header);
        }
        
        return Task.CompletedTask;
    }
}