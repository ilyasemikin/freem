using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

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
        
        var reference = new OpenApiReference
        {
            Id = JwtBearerOpenApiConstants.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        };

        var scheme = new OpenApiSecurityScheme
        {
            Reference = reference
        };
        
        var requirement = new OpenApiSecurityRequirement
        {
            [scheme] = Array.Empty<string>() 
        };
        
        operation.Security.Add(requirement);

        return Task.CompletedTask;
    }
}