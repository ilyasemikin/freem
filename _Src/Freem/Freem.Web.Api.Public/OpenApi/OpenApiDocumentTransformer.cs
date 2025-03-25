using Freem.Web.Api.Public.Configuration.Extensions;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace Freem.Web.Api.Public.OpenApi;

internal sealed class OpenApiDocumentTransformer : IOpenApiDocumentTransformer
{
    public Task TransformAsync(
        OpenApiDocument document, 
        OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        var configuration = context.ApplicationServices.GetRequiredService<IConfiguration>();
        var info = configuration.GetApiInfo();
        
        var environment = context.ApplicationServices.GetRequiredService<IWebHostEnvironment>();
        if (environment.IsDevelopment())
            return Task.CompletedTask;

        document.Servers.Clear();
        foreach (var address in info.ServerAddresses)
        {
            var server = new OpenApiServer
            {
                Url = address
            };
            
            document.Servers.Add(server);
        }
        
        return Task.CompletedTask;
    }
}