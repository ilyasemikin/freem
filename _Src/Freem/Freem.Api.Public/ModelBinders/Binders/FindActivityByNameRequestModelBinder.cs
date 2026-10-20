using Freem.Api.Public.Contracts.DTO.Activities;
using Freem.Api.Public.ModelBinders.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Freem.Api.Public.ModelBinders.Binders;

internal sealed class FindActivityByNameRequestModelBinder : IModelBinder
{
    private readonly string _searchTextName;

    public FindActivityByNameRequestModelBinder()
    {
        _searchTextName = "searchText";
    }
    
    public Task BindModelAsync(ModelBindingContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var str = context.ValueProvider.GetValue(_searchTextName);
        
        if (!str.TryGetStringValue(null, out var searchText))
            return Task.CompletedTask;

        var value = new FindActivityByNameRequest(searchText);
        context.Result = ModelBindingResult.Success(value);
        
        return Task.CompletedTask;
    }
}