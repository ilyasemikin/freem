using Freem.Domain;
using Freem.Web.Api.Public.Contracts.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Freem.Web.Api.Public.ErrorHandling;

internal static class InvalidModelStateResponseFactory
{
    private const string DefaultMessage = "The request is invalid";
    
    public static IActionResult Create(ActionContext context)
    {
        var entry = context.ModelState.FirstOrDefault(pair => pair.Value?.Errors.Count > 0);
        var message = entry.Value?.Errors.FirstOrDefault()?.ErrorMessage;
        var text = string.IsNullOrEmpty(message)
            ? DefaultMessage
            : string.IsNullOrEmpty(entry.Key)
                ? message
                : $"{entry.Key}: {message}";

        var error = DomainErrorCatalog.Factories.CreateInvalidRequest(text);
        var response = new ErrorResponse(error);
        return new ObjectResult(response)
        {
            StatusCode = StatusCodes.Status400BadRequest
        };
    }
}
