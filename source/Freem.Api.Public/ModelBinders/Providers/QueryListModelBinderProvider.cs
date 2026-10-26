using Freem.Api.Public.Contracts.V1.Activities;
using Freem.Api.Public.Contracts.V1.Events;
using Freem.Api.Public.Contracts.V1.DataTransfer.Import;
using Freem.Api.Public.Contracts.V1.Records;
using Freem.Api.Public.Contracts.V1.Statistics;
using Freem.Api.Public.Contracts.V1.Tags;
using Freem.Api.Public.ModelBinders.Binders;
using Freem.Api.Public.ModelBinders.Binders.DataTransfer;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Freem.Api.Public.ModelBinders.Providers;

internal sealed class QueryListModelBinderProvider : IModelBinderProvider
{
    private readonly IReadOnlyDictionary<Type, IModelBinder> _binders;

    public QueryListModelBinderProvider()
    {
        _binders = new Dictionary<Type, IModelBinder>
        {
            [typeof(ActivityListRequest)] = new ListActivityRequestModelBinder(),
            [typeof(TagListRequest)] = new ListTagRequestModelBinder(),
            [typeof(RecordListRequest)] = new ListRecordRequestModelBinder(),
            [typeof(RecordListByPeriodRequest)] = new ListRecordByPeriodRequestModelBinder(),
            [typeof(ImportListRequest)] = new ListImportRequestModelBinder(),
            [typeof(ImportRowListRequest)] = new ListImportRowRequestModelBinder(),
            [typeof(ImportActivityListRequest)] = new ListImportActivityRequestModelBinder(),
            [typeof(ImportTagListRequest)] = new ListImportTagRequestModelBinder(),
            [typeof(EventListRequest)] = new ListEventRequestModelBinder(),
            [typeof(ActivitySearchByNameRequest)] = new ActivitySearchByNameRequestModelBinder(),
            [typeof(StatisticsPerPeriodRequest)] = new StatisticsPerPeriodRequestModelBinder(),
        };
    }
    
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        return _binders.TryGetValue(context.Metadata.ModelType, out var binder)
            ? new ArgumentValidationModelBinder(binder)
            : null;
    }
}