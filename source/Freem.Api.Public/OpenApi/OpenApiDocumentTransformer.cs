using Freem.Api.Public.Configuration.Subsystems;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace Freem.Api.Public.OpenApi;

internal sealed class OpenApiDocumentTransformer : IOpenApiDocumentTransformer
{
    private readonly OpenApiConfigurationProperties _configuration;

    public OpenApiDocumentTransformer(OpenApiConfigurationProperties configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        _configuration = configuration;
    }

    public Task TransformAsync(
        OpenApiDocument document, 
        OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(_configuration.ServerUrl))
            return Task.CompletedTask;

        var server = new OpenApiServer
        {
            Url = _configuration.ServerUrl
        };
        document.Servers = [server];
        
        return Task.CompletedTask;
    }
}
