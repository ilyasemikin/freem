using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Freem.Api.Public.Formatters;

/// <summary>
/// Wraps the JSON body formatter and turns an <see cref="ArgumentException"/> thrown by a DTO constructor during
/// deserialization (missing or invalid field) into a model error, so the request is answered with 400 instead of
/// an unhandled 500. <see cref="SystemTextJsonInputFormatter"/> itself handles only JSON, format and overflow errors.
/// </summary>
internal sealed class ArgumentValidationJsonInputFormatter : IInputFormatter, IApiRequestFormatMetadataProvider, IInputFormatterExceptionPolicy
{
    private readonly SystemTextJsonInputFormatter _formatter;

    public InputFormatterExceptionPolicy ExceptionPolicy => ((IInputFormatterExceptionPolicy)_formatter).ExceptionPolicy;

    public ArgumentValidationJsonInputFormatter(SystemTextJsonInputFormatter formatter)
    {
        ArgumentNullException.ThrowIfNull(formatter);

        _formatter = formatter;
    }

    public bool CanRead(InputFormatterContext context)
    {
        return _formatter.CanRead(context);
    }

    public async Task<InputFormatterResult> ReadAsync(InputFormatterContext context)
    {
        try
        {
            return await _formatter.ReadAsync(context);
        }
        catch (ArgumentException exception)
        {
            context.ModelState.TryAddModelError(context.ModelName, exception.Message);
            return await InputFormatterResult.FailureAsync();
        }
    }

    public IReadOnlyList<string>? GetSupportedContentTypes(string? contentType, Type objectType)
    {
        return _formatter.GetSupportedContentTypes(contentType, objectType);
    }
}
