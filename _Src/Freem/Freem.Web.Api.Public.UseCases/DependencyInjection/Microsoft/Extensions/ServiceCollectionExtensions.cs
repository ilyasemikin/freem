using Freem.Web.Api.Public.UseCases.Context;
using Freem.Web.Api.Public.UseCases.Contracts.Activities;
using Freem.Web.Api.Public.UseCases.Contracts.Auth;
using Freem.Web.Api.Public.UseCases.Contracts.Events;
using Freem.Web.Api.Public.UseCases.Contracts.Records;
using Freem.Web.Api.Public.UseCases.Contracts.RunningRecords;
using Freem.Web.Api.Public.UseCases.Contracts.Statistics;
using Freem.Web.Api.Public.UseCases.Contracts.Tags;
using Freem.Web.Api.Public.UseCases.Contracts.Users;
using Freem.Web.Api.Public.UseCases.Contracts.Users.Settings;
using Freem.Web.Api.Public.UseCases.Implementations.Handlers.Activities;
using Freem.Web.Api.Public.UseCases.Implementations.Handlers.Auth;
using Freem.Web.Api.Public.UseCases.Implementations.Handlers.Events;
using Freem.Web.Api.Public.UseCases.Implementations.Handlers.Records;
using Freem.Web.Api.Public.UseCases.Implementations.Handlers.RunningRecords;
using Freem.Web.Api.Public.UseCases.Implementations.Handlers.Statistics;
using Freem.Web.Api.Public.UseCases.Implementations.Handlers.Tags;
using Freem.Web.Api.Public.UseCases.Implementations.Handlers.Users;
using Freem.Web.Api.Public.UseCases.Implementations.Handlers.Users.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.DependencyInjection;

namespace Freem.Web.Api.Public.UseCases.DependencyInjection.Microsoft.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiUseCases(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        services.AddRequestMediator<ApiAuthorizedHandlerContext>();
        services.AddRequestMediator<ApiAnonymousHandlerContext>();

        services
            .AddActivitiesUseCases()
            .AddAuthUseCases()
            .AddEventsUseCases()
            .AddRecordsUseCases()
            .AddRunningRecordsUseCases()
            .AddStatisticsUseCases()
            .AddTagsUseCases()
            .AddUsersUseCases()
            .AddUserSettingsUseCases();

        return services;
    }

    private static IServiceCollection AddActivitiesUseCases(this IServiceCollection services)
    {
        services.AddHandler<ApiArchiveActivityRequest, ApiArchiveActivityRequestHandler>();
        services.AddHandler<ApiCreateActivityRequest, ApiCreateActivityRequestHandler>();
        services.AddHandler<ApiFindActivityByNameRequest, ApiFindActivityByNameRequestHandler>();
        services.AddHandler<ApiGetActivityRequest, ApiGetActivityRequestHandler>();
        services.AddHandler<ApiListActivityRequest, ApiListActivityRequestHandler>();
        services.AddHandler<ApiRemoveActivityRequest, ApiRemoveActivityRequestHandler>();
        services.AddHandler<ApiUnarchiveActivityRequest, ApiUnarchiveActivityRequestHandler>();
        services.AddHandler<ApiUpdateActivityRequest, ApiUpdateActivityRequestHandler>();

        return services;
    }

    private static IServiceCollection AddAuthUseCases(this IServiceCollection services)
    {
        services.AddAnonymousHandler<ApiLoginRequest, ApiLoginRequestHandler>();
        services.AddAnonymousHandler<ApiLogoutRequest, ApiLogoutRequestHandler>();
        services.AddAnonymousHandler<ApiRefreshRequest, ApiRefreshRequestHandler>();
        services.AddAnonymousHandler<ApiRegisterRequest, ApiRegisterRequestHandler>();

        return services;
    }

    private static IServiceCollection AddEventsUseCases(this IServiceCollection services)
    {
        services.AddHandler<ApiListEventRequest, ApiListEventRequestHandler>();

        return services;
    }

    private static IServiceCollection AddRecordsUseCases(this IServiceCollection services)
    {
        services.AddHandler<ApiCreateRecordRequest, ApiCreateRecordRequestHandler>();
        services.AddHandler<ApiGetRecordRequest, ApiGetRecordRequestHandler>();
        services.AddHandler<ApiListRecordRequest, ApiListRecordRequestHandler>();
        services.AddHandler<ApiListRecordByPeriodRequest, ApiListRecordByPeriodRequestHandler>();
        services.AddHandler<ApiRemoveRecordRequest, ApiRemoveRecordRequestHandler>();
        services.AddHandler<ApiUpdateRecordRequest, ApiUpdateRecordRequestHandler>();

        return services;
    }

    private static IServiceCollection AddRunningRecordsUseCases(this IServiceCollection services)
    {
        services.AddHandler<ApiGetRunningRecordRequest, ApiGetRunningRecordRequestHandler>();
        services.AddHandler<ApiRemoveRunningRecordRequest, ApiRemoveRunningRecordRequestHandler>();
        services.AddHandler<ApiStartRunningRecordRequest, ApiStartRunningRecordRequestHandler>();
        services.AddHandler<ApiStopRunningRecordRequest, ApiStopRunningRecordRequestHandler>();
        services.AddHandler<ApiUpdateRunningRecordRequest, ApiUpdateRunningRecordRequestHandler>();

        return services;
    }

    private static IServiceCollection AddStatisticsUseCases(this IServiceCollection services)
    {
        services.AddHandler<ApiStatisticsPerDaysRequest, ApiStatisticsPerDaysRequestHandler>();
        services.AddHandler<ApiStatisticsPerPeriodRequest, ApiStatisticsPerPeriodRequestHandler>();

        return services;
    }

    private static IServiceCollection AddTagsUseCases(this IServiceCollection services)
    {
        services.AddHandler<ApiCreateTagRequest, ApiCreateTagRequestHandler>();
        services.AddHandler<ApiFindTagByNameRequest, ApiFindTagByNameRequestHandler>();
        services.AddHandler<ApiGetTagRequest, ApiGetTagRequestHandler>();
        services.AddHandler<ApiListTagRequest, ApiListTagRequestHandler>();
        services.AddHandler<ApiRemoveTagRequest, ApiRemoveTagRequestHandler>();
        services.AddHandler<ApiUpdateTagRequest, ApiUpdateTagRequestHandler>();

        return services;
    }

    private static IServiceCollection AddUsersUseCases(this IServiceCollection services)
    {
        services.AddHandler<ApiGetUserRequest, ApiGetUserRequestHandler>();
        services.AddHandler<ApiRegisterUserRequest, ApiRegisterUserRequestHandler>();

        return services;
    }

    private static IServiceCollection AddUserSettingsUseCases(this IServiceCollection services)
    {
        services.AddHandler<ApiGetUserSettingsRequest, ApiGetUserSettingsRequestHandler>();
        services.AddHandler<ApiUpdateUserSettingsRequest, ApiUpdateUserSettingsRequestHandler>();

        return services;
    }

    private static IServiceCollection AddHandler<TRequest, THandler>(this IServiceCollection services)
        where TRequest : IRequest
        where THandler : class, IRequestHandler<ApiAuthorizedHandlerContext, TRequest, IActionResult>
    {
        services.TryAddTransient<IRequestHandler<ApiAuthorizedHandlerContext, TRequest, IActionResult>, THandler>();
        return services;
    }

    private static IServiceCollection AddAnonymousHandler<TRequest, THandler>(this IServiceCollection services)
        where TRequest : IRequest
        where THandler : class, IRequestHandler<ApiAnonymousHandlerContext, TRequest, IActionResult>
    {
        services.TryAddTransient<IRequestHandler<ApiAnonymousHandlerContext, TRequest, IActionResult>, THandler>();
        return services;
    }
}
