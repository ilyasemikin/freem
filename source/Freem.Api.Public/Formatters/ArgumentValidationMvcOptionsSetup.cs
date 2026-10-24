using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;

namespace Freem.Api.Public.Formatters;

/// <summary>
/// Wraps the default <see cref="SystemTextJsonInputFormatter"/> into <see cref="ArgumentValidationJsonInputFormatter"/>;
/// a post-configuration, because MVC adds its formatters in its own options setup.
/// </summary>
internal sealed class ArgumentValidationMvcOptionsSetup : IPostConfigureOptions<MvcOptions>
{
    public void PostConfigure(string? name, MvcOptions options)
    {
        var formatters = options.InputFormatters;
        for (var i = 0; i < formatters.Count; i++)
        {
            if (formatters[i] is SystemTextJsonInputFormatter formatter)
                formatters[i] = new ArgumentValidationJsonInputFormatter(formatter);
        }
    }
}
