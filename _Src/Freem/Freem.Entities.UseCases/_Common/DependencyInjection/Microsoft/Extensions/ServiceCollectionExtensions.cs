using Freem.Entities.UseCases.Activities;
using Freem.Entities.UseCases.Context;
using Freem.Entities.UseCases.Contracts.Activities.Archive;
using Freem.Entities.UseCases.Contracts.Activities.Create;
using Freem.Entities.UseCases.Contracts.Activities.Find;
using Freem.Entities.UseCases.Contracts.Activities.Get;
using Freem.Entities.UseCases.Contracts.Activities.List;
using Freem.Entities.UseCases.Contracts.Activities.Remove;
using Freem.Entities.UseCases.Contracts.Activities.Unarchive;
using Freem.Entities.UseCases.Contracts.Activities.Update;
using Freem.Entities.UseCases.Contracts.Events.List;
using Freem.Entities.UseCases.Contracts.Records.Create;
using Freem.Entities.UseCases.Contracts.Records.Get;
using Freem.Entities.UseCases.Contracts.Records.List;
using Freem.Entities.UseCases.Contracts.Records.PeriodList;
using Freem.Entities.UseCases.Contracts.Records.Remove;
using Freem.Entities.UseCases.Contracts.Records.Update;
using Freem.Entities.UseCases.Contracts.RunningRecords.Get;
using Freem.Entities.UseCases.Contracts.RunningRecords.Remove;
using Freem.Entities.UseCases.Contracts.RunningRecords.Start;
using Freem.Entities.UseCases.Contracts.RunningRecords.Stop;
using Freem.Entities.UseCases.Contracts.RunningRecords.Update;
using Freem.Entities.UseCases.Contracts.Statistics.PerDays;
using Freem.Entities.UseCases.Contracts.Statistics.PerPeriod;
using Freem.Entities.UseCases.Contracts.Tags.Create;
using Freem.Entities.UseCases.Contracts.Tags.Get;
using Freem.Entities.UseCases.Contracts.Tags.GetByName;
using Freem.Entities.UseCases.Contracts.Tags.List;
using Freem.Entities.UseCases.Contracts.Tags.Remove;
using Freem.Entities.UseCases.Contracts.Tags.Update;
using Freem.Entities.UseCases.Contracts.Users.Get;
using Freem.Entities.UseCases.Contracts.Users.Register;
using Freem.Entities.UseCases.Contracts.Users.Settings.Get;
using Freem.Entities.UseCases.Contracts.Users.Settings.Update;
using Freem.Entities.UseCases.Events;
using Freem.Entities.UseCases.Records;
using Freem.Entities.UseCases.RunningRecords;
using Freem.Entities.UseCases.Statistics;
using Freem.Entities.UseCases.Tags;
using Freem.Entities.UseCases.Users;
using Freem.Entities.UseCases.Users.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.DependencyInjection;

namespace Freem.Entities.UseCases.DependencyInjection.Microsoft.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEntitiesUseCases(this IServiceCollection services)
    {
        services.AddRequestMediator<HandlerContext>();

        services
            .AddActivitiesUseCases()
            .AddTagsUseCases()
            .AddRecordsUseCases()
            .AddRunningRecordsUseCases()
            .AddEventsUseCases()
            .AddUserUseCases()
            .AddUserSettingsUseCases()
            .AddUsersRegistrationUseCases()
            .AddStatisticsUseCases();

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

    private static IServiceCollection AddHandler<TRequest, TResponse, THandler>(this IServiceCollection services)
        where TRequest : IRequest
        where THandler : class, IRequestHandler<HandlerContext, TRequest, TResponse>
    {
        services.TryAddTransient<IRequestHandler<HandlerContext, TRequest, TResponse>, THandler>();
        return services;
    }
}