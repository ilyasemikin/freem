using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace Freem.Web.Api.Public.Authentication.OpenApi;

internal sealed class JwtBearerOpenApiDocumentTransformer : IOpenApiDocumentTransformer
{
    public Task TransformAsync(
        OpenApiDocument document, 
        OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        var schemes = new Dictionary<string, OpenApiSecurityScheme>
        {
            [JwtBearerOpenApiConstants.AuthenticationScheme] = new()
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                In = ParameterLocation.Header,
                BearerFormat = "Json Web Token"
            }
        };

        document.Components ??= new();
        document.Components.SecuritySchemes = schemes;
        
        return Task.CompletedTask;
    }
}