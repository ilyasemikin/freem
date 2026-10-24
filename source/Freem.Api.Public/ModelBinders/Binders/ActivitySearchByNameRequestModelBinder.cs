using Freem.Api.Public.Contracts.V1.Activities;
using Freem.Api.Public.ModelBinders.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Freem.Api.Public.ModelBinders.Binders;

internal sealed class ActivitySearchByNameRequestModelBinder : IModelBinder
{
    private readonly string _searchTextName;

    public ActivitySearchByNameRequestModelBinder()
    {
        _searchTextName = "searchText";
    }
    
    public Task BindModelAsync(ModelBindingContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var str = context.ValueProvider.GetValue(_searchTextName);
        
        if (!str.TryGetStringValue(null, out var searchText))
            return Task.CompletedTask;

        var value = new ActivitySearchByNameRequest(searchText);
        context.Result = ModelBindingResult.Success(value);
        
        return Task.CompletedTask;
    }
}