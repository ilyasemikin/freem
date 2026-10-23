using Freem.Domain.UseCases.Activities;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Activities.Archive;
using Freem.Domain.UseCases.Contracts.Activities.Create;
using Freem.Domain.UseCases.Contracts.Activities.Find;
using Freem.Domain.UseCases.Contracts.Activities.Get;
using Freem.Domain.UseCases.Contracts.Activities.List;
using Freem.Domain.UseCases.Contracts.Activities.Remove;
using Freem.Domain.UseCases.Contracts.Activities.Unarchive;
using Freem.Domain.UseCases.Contracts.Activities.Update;
using Freem.Domain.UseCases.Contracts.Events.Get;
using Freem.Domain.UseCases.Contracts.Events.List;
using Freem.Domain.UseCases.Contracts.Records.AddPeriod;
using Freem.Domain.UseCases.Contracts.Records.Create;
using Freem.Domain.UseCases.Contracts.Records.Get;
using Freem.Domain.UseCases.Contracts.Records.List;
using Freem.Domain.UseCases.Contracts.Records.PeriodList;
using Freem.Domain.UseCases.Contracts.Records.Remove;
using Freem.Domain.UseCases.Contracts.Records.RemovePeriod;
using Freem.Domain.UseCases.Contracts.Records.Update;
using Freem.Domain.UseCases.Contracts.Records.UpdatePeriod;
using Freem.Domain.UseCases.Contracts.RunningRecords.Get;
using Freem.Domain.UseCases.Contracts.RunningRecords.Remove;
using Freem.Domain.UseCases.Contracts.RunningRecords.Start;
using Freem.Domain.UseCases.Contracts.RunningRecords.Stop;
using Freem.Domain.UseCases.Contracts.RunningRecords.Update;
using Freem.Domain.UseCases.Contracts.Statistics.PerDays;
using Freem.Domain.UseCases.Contracts.Statistics.PerPeriod;
using Freem.Domain.UseCases.Contracts.Tags.Create;
using Freem.Domain.UseCases.Contracts.Tags.Get;
using Freem.Domain.UseCases.Contracts.Tags.GetByName;
using Freem.Domain.UseCases.Contracts.Tags.List;
using Freem.Domain.UseCases.Contracts.Tags.Remove;
using Freem.Domain.UseCases.Contracts.Tags.Update;
using Freem.Domain.UseCases.Contracts.Users.Get;
using Freem.Domain.UseCases.Contracts.Users.Register;
using Freem.Domain.UseCases.Contracts.Users.Settings.Get;
using Freem.Domain.UseCases.Contracts.Users.Settings.Update;
using Freem.Domain.UseCases.Contracts.Export;
using Freem.Domain.UseCases.Contracts.Formats.List;
using Freem.Domain.UseCases.Events;
using Freem.Domain.UseCases.Export;
using Freem.Domain.UseCases.Formats;
using Freem.Domain.UseCases.Import;
using Freem.Domain.UseCases.Records;
using Freem.Domain.UseCases.RunningRecords;
using Freem.Domain.UseCases.Statistics;
using Freem.Domain.UseCases.Tags;
using Freem.Domain.UseCases.Users;
using Freem.Domain.UseCases.Users.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.DependencyInjection;

namespace Freem.Domain.UseCases.DependencyInjection.Microsoft.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEntitiesUseCases(this IServiceCollection services)
    {
        services.AddRequestMediator<HandlerContext>();
        services.TryAddTransient<EventProducer>();

        services
            .AddActivitiesUseCases()
            .AddTagsUseCases()
            .AddRecordsUseCases()
            .AddRunningRecordsUseCases()
            .AddEventsUseCases()
            .AddUserUseCases()
            .AddUserSettingsUseCases()
            .AddUsersRegistrationUseCases()
            .AddStatisticsUseCases()
            .AddDataTransferUseCases();

        return services;
    }

    private static IServiceCollection AddActivitiesUseCases(this IServiceCollection services)
    {
        services.AddHandler<CreateActivityRequest, Result<CreateActivityResponse, Error>, CreateActivityUseCase>();
        services.AddHandler<UpdateActivityRequest, Result<Error>, UpdateActivityUseCase>();
        services.AddHandler<RemoveActivityRequest, Result<Error>, RemoveActivityUseCase>();
        services.AddHandler<ArchiveActivityRequest, Result<Error>, ArchiveActivityUseCase>();
        services.AddHandler<UnarchiveActivityRequest, Result<Error>, UnarchiveActivityUseCase>();

        services.AddHandler<GetActivityRequest, Result<GetActivityResponse, Error>, GetActivityUseCase>();
        services.AddHandler<FindActivityRequest, Result<FindActivityResponse, Error>, FindActivityUseCase>();
        services.AddHandler<ListActivityRequest, Result<ListActivityResponse, Error>, ListActivityUseCase>();

        return services;
    }

    private static IServiceCollection AddTagsUseCases(this IServiceCollection services)
    {
        services.AddHandler<CreateTagRequest, Result<CreateTagResponse, Error>, CreateTagUseCase>();
        services.AddHandler<UpdateTagRequest, Result<Error>, UpdateTagUseCase>();
        services.AddHandler<RemoveTagRequest, Result<Error>, RemoveTagUseCase>();

        services.AddHandler<GetTagRequest, Result<GetTagResponse, Error>, GetTagUseCase>();
        services.AddHandler<FindTagByNameRequest, Result<FindTagByNameResponse, Error>, FindTagByNameUseCase>();
        services.AddHandler<ListTagRequest, Result<ListTagResponse, Error>, ListTagUseCase>();

        return services;
    }

    private static IServiceCollection AddRecordsUseCases(this IServiceCollection services)
    {
        services.AddHandler<CreateRecordRequest, Result<CreateRecordResponse, Error>, CreateRecordUseCase>();
        services.AddHandler<UpdateRecordRequest, Result<Error>, UpdateRecordUseCase>();
        services.AddHandler<RemoveRecordRequest, Result<Error>, RemoveRecordUseCase>();

        services.AddHandler<AddRecordPeriodRequest, Result<AddRecordPeriodResponse, Error>, AddRecordPeriodUseCase>();
        services.AddHandler<UpdateRecordPeriodRequest, Result<Error>, UpdateRecordPeriodUseCase>();
        services.AddHandler<RemoveRecordPeriodRequest, Result<Error>, RemoveRecordPeriodUseCase>();

        services.AddHandler<GetRecordRequest, Result<GetRecordResponse, Error>, GetRecordUseCase>();
        services.AddHandler<ListRecordRequest, Result<ListRecordResponse, Error>, ListRecordUseCase>();
        services.AddHandler<PeriodListRequest, Result<PeriodListResponse, Error>, PeriodListRecordUseCase>();

        return services;
    }

    private static IServiceCollection AddRunningRecordsUseCases(this IServiceCollection services)
    {
        services.AddHandler<StartRunningRecordRequest, Result<StartRunningRecordResponse, Error>, StartRunningRecordUseCase>();
        services.AddHandler<StopRunningRecordRequest, Result<Error>, StopRunningRecordUseCase>();
        services.AddHandler<UpdateRunningRecordRequest, Result<Error>, UpdateRunningRecordUseCase>();
        services.AddHandler<RemoveRunningRecordRequest, Result<Error>, RemoveRunningRecordUseCase>();

        services.AddHandler<GetRunningRecordRequest, Result<GetRunningRecordResponse, Error>, GetRunningRecordUseCase>();

        return services;
    }

    private static IServiceCollection AddEventsUseCases(this IServiceCollection services)
    {
        services.AddHandler<GetEventRequest, Result<GetEventResponse, Error>, GetEventUseCase>();
        services.AddHandler<ListEventRequest, Result<ListEventResponse, Error>, ListEventUseCase>();

        return services;
    }

    private static IServiceCollection AddUserUseCases(this IServiceCollection services)
    {
        services.AddHandler<GetUserRequest, Result<GetUserResponse, Error>, GetUserUseCase>();

        return services;
    }

    private static IServiceCollection AddUserSettingsUseCases(this IServiceCollection services)
    {
        services.AddHandler<UpdateUserSettingsRequest, Result<Error>, UpdateUserSettingsUseCase>();
        services.AddHandler<GetUserSettingsRequest, Result<GetUserSettingsResponse, Error>, GetUserSettingsUseCase>();

        return services;
    }

    private static IServiceCollection AddUsersRegistrationUseCases(this IServiceCollection services)
    {
        services.AddHandler<RegisterUserRequest, Result<RegisterUserResponse, Error>, RegisterUserUseCase>();

        return services;
    }

    private static IServiceCollection AddStatisticsUseCases(this IServiceCollection services)
    {
        services.AddHandler<StatisticsPerDaysRequest, Result<StatisticsPerDaysResponse, Error>, StatisticsPerDaysUseCase>();
        services.AddHandler<StatisticsPerPeriodRequest, Result<StatisticsPerPeriodResponse, Error>, StatisticsPerPeriodUseCase>();

        return services;
    }

    private static IServiceCollection AddDataTransferUseCases(this IServiceCollection services)
    {
        services.TryAddEnumerable(ServiceDescriptor.Singleton<IExportWriter, NativeJsonExportWriter>());
        services.TryAddEnumerable(ServiceDescriptor.Singleton<IImportParser, NativeJsonImportParser>());
        services.TryAddSingleton<ExportWriterRegistry>();
        services.TryAddSingleton<ImportParserRegistry>();
        services.TryAddTransient<ExportItemReader>();

        services.AddHandler<ExportDataRequest, Result<ExportDataResponse, Error>, ExportDataUseCase>();
        services.AddHandler<ListFormatsRequest, Result<ListFormatsResponse, Error>, ListFormatsUseCase>();

        return services;
    }

    private static IServiceCollection AddHandler<TRequest, TResponse, THandler>(this IServiceCollection services)
        where TRequest : IRequest
        where THandler : class, IRequestHandler<HandlerContext, TRequest, TResponse>
    {
        services.TryAddTransient<IRequestHandler<HandlerContext, TRequest, TResponse>, THandler>();
        return services;
    }
}