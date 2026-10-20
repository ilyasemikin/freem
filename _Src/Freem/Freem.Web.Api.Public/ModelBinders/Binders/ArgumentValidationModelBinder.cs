using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Freem.Web.Api.Public.ModelBinders.Binders;

/// <summary>
/// Turns an <see cref="ArgumentException"/> thrown by a DTO constructor during binding into a model error,
/// so an invalid query is answered with 400 instead of an unhandled 500.
/// </summary>
internal sealed class ArgumentValidationModelBinder : IModelBinder
{
    private readonly IModelBinder _binder;

    public ArgumentValidationModelBinder(IModelBinder binder)
    {
        ArgumentNullException.ThrowIfNull(binder);

        _binder = binder;
    }

    public async Task BindModelAsync(ModelBindingContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        try
        {
            await _binder.BindModelAsync(context);
        }
        catch (ArgumentException exception)
        {
            context.ModelState.TryAddModelError(context.ModelName, exception.Message);
            context.Result = ModelBindingResult.Failed();
        }
    }
}
