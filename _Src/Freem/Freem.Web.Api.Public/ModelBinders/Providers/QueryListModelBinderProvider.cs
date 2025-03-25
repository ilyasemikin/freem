using Freem.Web.Api.Public.Contracts.Activities;
using Freem.Web.Api.Public.Contracts.Events;
using Freem.Web.Api.Public.Contracts.Records;
using Freem.Web.Api.Public.Contracts.Statistics;
using Freem.Web.Api.Public.Contracts.Tags;
using Freem.Web.Api.Public.ModelBinders.Binders;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Freem.Web.Api.Public.ModelBinders.Providers;

internal sealed class QueryListModelBinderProvider : IModelBinderProvider
{
    private readonly IReadOnlyDictionary<Type, IModelBinder> _binders;

    public QueryListModelBinderProvider()
    {
        _binders = new Dictionary<Type, IModelBinder>
        {
            [typeof(ListActivityRequest)] = new ListActivityRequestModelBinder(),
            [typeof(ListTagRequest)] = new ListTagRequestModelBinder(),
            [typeof(ListRecordRequest)] = new ListRecordRequestModelBinder(),
            [typeof(ListRecordByPeriodRequest)] = new ListRecordByPeriodRequestModelBinder(),
            [typeof(ListEventRequest)] = new ListEventRequestModelBinder(),
            [typeof(StatisticsPerDaysRequest)] = new StatisticsPerDaysRequestModelBinder()
        };
    }
    
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        return _binders.GetValueOrDefault(context.Metadata.ModelType);
    }
}