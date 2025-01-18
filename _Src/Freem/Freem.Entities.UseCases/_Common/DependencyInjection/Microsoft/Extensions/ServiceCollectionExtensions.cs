using Freem.Entities.UseCases.Activities;
using Freem.Entities.UseCases.Contracts.Activities.Archive;
using Freem.Entities.UseCases.Contracts.Activities.Create;
using Freem.Entities.UseCases.Contracts.Activities.Get;
using Freem.Entities.UseCases.Contracts.Activities.List;
using Freem.Entities.UseCases.Contracts.Activities.Remove;
using Freem.Entities.UseCases.Contracts.Activities.Unarchive;
using Freem.Entities.UseCases.Contracts.Activities.Update;
using Freem.Entities.UseCases.Contracts.Events.List;
using Freem.Entities.UseCases.Contracts.Records.Create;
using Freem.Entities.UseCases.Contracts.Records.Get;
using Freem.Entities.UseCases.Contracts.Records.List;
using Freem.Entities.UseCases.Contracts.Records.Remove;
using Freem.Entities.UseCases.Contracts.Records.Update;
using Freem.Entities.UseCases.Contracts.RunningRecords.Get;
using Freem.Entities.UseCases.Contracts.RunningRecords.Remove;
using Freem.Entities.UseCases.Contracts.RunningRecords.Start;
using Freem.Entities.UseCases.Contracts.RunningRecords.Stop;
using Freem.Entities.UseCases.Contracts.RunningRecords.Update;
using Freem.Entities.UseCases.Contracts.Tags.Create;
using Freem.Entities.UseCases.Contracts.Tags.Get;
using Freem.Entities.UseCases.Contracts.Tags.List;
using Freem.Entities.UseCases.Contracts.Tags.Remove;
using Freem.Entities.UseCases.Contracts.Tags.Update;
using Freem.Entities.UseCases.Contracts.Users.Password.Login;
using Freem.Entities.UseCases.Contracts.Users.Password.Register;
using Freem.Entities.UseCases.Contracts.Users.Password.Update;
using Freem.Entities.UseCases.Contracts.Users.Tokens.Refresh;
using Freem.Entities.UseCases.Events;
using Freem.Entities.UseCases.Records;
using Freem.Entities.UseCases.RunningRecords;
using Freem.Entities.UseCases.Tags;
using Freem.Entities.UseCases.Users.Password;
using Freem.Entities.UseCases.Users.Tokens;
using Freem.UseCases.Abstractions;
using Freem.UseCases.Implementations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Freem.Entities.UseCases.DependencyInjection.Microsoft.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUseCaseExecutor(this IServiceCollection services)
    {
        services.AddTransient<IUseCaseExecutor<UseCaseExecutionContext>>(provider => new UseCaseExecutor<UseCaseExecutionContext>(provider));
        
        return services;
    }
    
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        return services
            .AddActivitiesUseCases()
            .AddTagsUseCases()
            .AddRecordsUseCases()
            .AddRunningRecordsUseCases()
            .AddEventsUseCases()
            .AddUsersPasswordUseCases()
            .AddUsersTokensUseCases();
    }

    private static IServiceCollection AddActivitiesUseCases(this IServiceCollection services)
    {
        services.TryAddTransient<IUseCase<UseCaseExecutionContext, CreateActivityRequest, CreateActivityResponse, CreateActivityErrorCode>, CreateActivityUseCase>();
        services.TryAddTransient<IUseCase<UseCaseExecutionContext, UpdateActivityRequest, UpdateActivityResponse, UpdateActivityErrorCode>, UpdateActivityUseCase>();
        services.TryAddTransient<IUseCase<UseCaseExecutionContext, RemoveActivityRequest, RemoveActivityResponse, RemoveActivityErrorCode>, RemoveActivityUseCase>();
        services.TryAddTransient<IUseCase<UseCaseExecutionContext, ArchiveActivityRequest, ArchiveActivityResponse, ArchiveActivityErrorCode>, ArchiveActivityUseCase>();
        services.TryAddTransient<IUseCase<UseCaseExecutionContext, UnarchiveActivityRequest, UnarchiveActivityResponse, UnarchiveActivityErrorCode>, UnarchiveActivityUseCase>();
        
        services.TryAddTransient<IUseCase<UseCaseExecutionContext, GetActivityRequest, GetActivityResponse, GetActivityErrorCode>, GetActivityUseCase>();
        services.TryAddTransient<IUseCase<UseCaseExecutionContext, ListActivityRequest, ListActivityResponse, ListActivityErrorCode>, ListActivityUseCase>();
        
        return services;
    }

    private static IServiceCollection AddTagsUseCases(this IServiceCollection services)
    {
        services.TryAddTransient<IUseCase<UseCaseExecutionContext, CreateTagRequest, CreateTagResponse, CreateTagErrorCode>, CreateTagUseCase>();
        services.TryAddTransient<IUseCase<UseCaseExecutionContext, UpdateTagRequest, UpdateTagResponse, UpdateTagErrorCode>, UpdateTagUseCase>();
        services.TryAddTransient<IUseCase<UseCaseExecutionContext, RemoveTagRequest, RemoveTagResponse, RemoveTagErrorCode>, RemoveTagUseCase>();
        
        services.TryAddTransient<IUseCase<UseCaseExecutionContext, GetTagRequest, GetTagResponse, GetTagErrorCode>, GetTagUseCase>();
        services.TryAddTransient<IUseCase<UseCaseExecutionContext, ListTagRequest, ListTagResponse, ListTagErrorCode>, ListTagUseCase>();
        
        return services;
    }

    private static IServiceCollection AddRecordsUseCases(this IServiceCollection services)
    {
        services.TryAddTransient<IUseCase<UseCaseExecutionContext, CreateRecordRequest, CreateRecordResponse, CreateRecordErrorCode>, CreateRecordUseCase>();
        services.TryAddTransient<IUseCase<UseCaseExecutionContext, UpdateRecordRequest, UpdateRecordResponse, UpdateRecordErrorCode>, UpdateRecordUseCase>();
        services.TryAddTransient<IUseCase<UseCaseExecutionContext, RemoveRecordRequest, RemoveRecordResponse, RemoveRecordErrorCode>, RemoveRecordUseCase>();
        
        services.TryAddTransient<IUseCase<UseCaseExecutionContext, GetRecordRequest, GetRecordResponse, GetRecordErrorCode>, GetRecordUseCase>();
        services.TryAddTransient<IUseCase<UseCaseExecutionContext, ListRecordRequest, ListRecordResponse, ListRecordErrorCode>, ListRecordUseCase>();
        
        return services;
    }

    private static IServiceCollection AddRunningRecordsUseCases(this IServiceCollection services)
    {
        services.TryAddTransient<IUseCase<UseCaseExecutionContext, StartRunningRecordRequest, StartRunningRecordResponse, StartRunningRecordErrorCode>, StartRunningRecordUseCase>();
        services.TryAddTransient<IUseCase<UseCaseExecutionContext, StopRunningRecordRequest, StopRunningRecordResponse, StopRunningRecordErrorCode>, StopRunningRecordUseCase>();
        services.TryAddTransient<IUseCase<UseCaseExecutionContext, UpdateRunningRecordRequest, UpdateRunningRecordResponse, UpdateRunningRecordErrorCode>, UpdateRunningRecordUseCase>();
        services.TryAddTransient<IUseCase<UseCaseExecutionContext, RemoveRunningRecordRequest, RemoveRunningRecordResponse, RemoveRunningRecordErrorCode>, RemoveRunningRecordUseCase>();

        services.TryAddTransient<IUseCase<UseCaseExecutionContext, GetRunningRecordRequest, GetRunningRecordResponse, GetRunningRecordErrorCode>, GetRunningRecordUseCase>();
        
        return services;
    }

    private static IServiceCollection AddEventsUseCases(this IServiceCollection services)
    {
        services.TryAddTransient<IUseCase<UseCaseExecutionContext, ListEventRequest, ListEventResponse, ListEventErrorCode>, ListEventUseCase>();
        
        return services;
    }

    private static IServiceCollection AddUsersPasswordUseCases(this IServiceCollection services)
    {
        services.TryAddTransient<IUseCase<UseCaseExecutionContext, LoginUserPasswordRequest, LoginUserPasswordResponse, LoginUserPasswordErrorCode>, LoginUserPasswordUseCase>();
        services.TryAddTransient<IUseCase<UseCaseExecutionContext, RegisterUserPasswordRequest, RegisterUserPasswordResponse, RegisterUserPasswordErrorCode>, RegisterUserPasswordUseCase>();
        services.TryAddTransient<IUseCase<UseCaseExecutionContext, UpdateLoginCredentialsRequest, UpdateLoginCredentialsResponse, UpdateLoginCredentialsErrorCode>, UpdateLoginCredentialsUseCases>();

        return services;
    }

    private static IServiceCollection AddUsersTokensUseCases(this IServiceCollection services)
    {
        services.TryAddTransient<IUseCase<UseCaseExecutionContext, RefreshUserAccessTokenRequest, RefreshUserAccessTokenResponse, RefreshUserAccessTokenErrorCode>, RefreshUserAccessTokenUseCase>();
        
        return services;
    }
}