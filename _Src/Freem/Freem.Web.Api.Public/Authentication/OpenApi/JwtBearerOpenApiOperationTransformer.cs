using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace Freem.Web.Api.Public.Authentication.OpenApi;

internal sealed class JwtBearerOpenApiOperationTransformer : IOpenApiOperationTransformer
{
    public Task TransformAsync(
        OpenApiOperation operation, 
        OpenApiOperationTransformerContext context,
        CancellationToken cancellationToken)
    {
        if (context.Description.ActionDescriptor is not ControllerActionDescriptor controller)
            return Task.CompletedTask;

        var attribute = controller.ControllerTypeInfo.GetCustomAttribute<AuthorizeAttribute>();
        if (attribute is null)
            return Task.CompletedTask;
        
        var scheme = new OpenApiSecuritySchemeReference(
            JwtBearerOpenApiConstants.AuthenticationScheme, 
            context.Document);
        
        var requirement = new OpenApiSecurityRequirement
        {
            [scheme] = new List<string>() 
        };
        
        operation.Security ??= new List<OpenApiSecurityRequirement>();
        operation.Security.Add(requirement);

        return Task.CompletedTask;
    }
}