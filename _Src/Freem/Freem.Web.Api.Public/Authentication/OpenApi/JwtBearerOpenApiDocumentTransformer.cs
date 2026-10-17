using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace Freem.Web.Api.Public.Authentication.OpenApi;

internal sealed class JwtBearerOpenApiDocumentTransformer : IOpenApiDocumentTransformer
{
    public Task TransformAsync(
        OpenApiDocument document, 
        OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        var scheme = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            In = ParameterLocation.Header,
            BearerFormat = "Json Web Token"
        };

        var schemes = new Dictionary<string, IOpenApiSecurityScheme>
        {
            [JwtBearerOpenApiConstants.AuthenticationScheme] = scheme
        };

        document.Components ??= new();
        document.Components.SecuritySchemes = schemes;
        
        return Task.CompletedTask;
    }
}