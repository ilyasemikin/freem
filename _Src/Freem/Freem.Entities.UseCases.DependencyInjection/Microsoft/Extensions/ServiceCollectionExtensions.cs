using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Activities;
using Freem.Entities.UseCases.DTO.Activities.Archive;
using Freem.Entities.UseCases.DTO.Activities.Create;
using Freem.Entities.UseCases.DTO.Activities.Get;
using Freem.Entities.UseCases.DTO.Activities.List;
using Freem.Entities.UseCases.DTO.Activities.Remove;
using Freem.Entities.UseCases.DTO.Activities.Unarchive;
using Freem.Entities.UseCases.DTO.Activities.Update;
using Freem.Entities.UseCases.DTO.Events.List;
using Freem.Entities.UseCases.DTO.Records.Create;
using Freem.Entities.UseCases.DTO.Records.Get;
using Freem.Entities.UseCases.DTO.Records.List;
using Freem.Entities.UseCases.DTO.Records.Remove;
using Freem.Entities.UseCases.DTO.Records.Update;
using Freem.Entities.UseCases.DTO.RunningRecords.Get;
using Freem.Entities.UseCases.DTO.RunningRecords.Remove;
using Freem.Entities.UseCases.DTO.RunningRecords.Start;
using Freem.Entities.UseCases.DTO.RunningRecords.Stop;
using Freem.Entities.UseCases.DTO.RunningRecords.Update;
using Freem.Entities.UseCases.DTO.Tags.Create;
using Freem.Entities.UseCases.DTO.Tags.Get;
using Freem.Entities.UseCases.DTO.Tags.List;
using Freem.Entities.UseCases.DTO.Tags.Remove;
using Freem.Entities.UseCases.DTO.Tags.Update;
using Freem.Entities.UseCases.DTO.Users.Password.Login;
using Freem.Entities.UseCases.DTO.Users.Password.Register;
using Freem.Entities.UseCases.DTO.Users.Password.Update;
using Freem.Entities.UseCases.DTO.Users.Tokens.Refresh;
using Freem.Entities.UseCases.Events;
using Freem.Entities.UseCases.Records;
using Freem.Entities.UseCases.RunningRecords;
using Freem.Entities.UseCases.Tags;
using Freem.Entities.UseCases.Users.Password;
using Freem.Entities.UseCases.Users.Tokens;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Freem.Entities.UseCases.DependencyInjection.Microsoft.Extensions;

public static class ServiceCollectionExtensions
{
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
        services.TryAddTransient<IUseCase<CreateActivityRequest, CreateActivityResponse, CreateActivityErrorCode>, CreateActivityUseCase>();
        services.TryAddTransient<IUseCase<UpdateActivityRequest, UpdateActivityResponse, UpdateActivityErrorCode>, UpdateActivityUseCase>();
        services.TryAddTransient<IUseCase<RemoveActivityRequest, RemoveActivityResponse, RemoveActivityErrorCode>, RemoveActivityUseCase>();
        services.TryAddTransient<IUseCase<ArchiveActivityRequest, ArchiveActivityResponse, ArchiveActivityErrorCode>, ArchiveActivityUseCase>();
        services.TryAddTransient<IUseCase<UnarchiveActivityRequest, UnarchiveActivityResponse, UnarchiveActivityErrorCode>, UnarchiveActivityUseCase>();
        
        services.TryAddTransient<IUseCase<GetActivityRequest, GetActivityResponse, GetActivityErrorCode>, GetActivityUseCase>();
        services.TryAddTransient<IUseCase<ListActivityRequest, ListActivityResponse, ListActivityErrorCode>, ListActivityUseCase>();
        
        return services;
    }

    private static IServiceCollection AddTagsUseCases(this IServiceCollection services)
    {
        services.TryAddTransient<IUseCase<CreateTagRequest, CreateTagResponse, CreateTagErrorCode>, CreateTagUseCase>();
        services.TryAddTransient<IUseCase<UpdateTagRequest, UpdateTagResponse, UpdateTagErrorCode>, UpdateTagUseCase>();
        services.TryAddTransient<IUseCase<RemoveTagRequest, RemoveTagResponse, RemoveTagErrorCode>, RemoveTagUseCase>();
        
        services.TryAddTransient<IUseCase<GetTagRequest, GetTagResponse, GetTagErrorCode>, GetTagUseCase>();
        services.TryAddTransient<IUseCase<ListTagRequest, ListTagResponse, ListTagErrorCode>, ListTagUseCase>();
        
        return services;
    }

    private static IServiceCollection AddRecordsUseCases(this IServiceCollection services)
    {
        services.TryAddTransient<IUseCase<CreateRecordRequest, CreateRecordResponse, CreateRecordErrorCode>, CreateRecordUseCase>();
        services.TryAddTransient<IUseCase<UpdateRecordRequest, UpdateRecordResponse, UpdateRecordErrorCode>, UpdateRecordUseCase>();
        services.TryAddTransient<IUseCase<RemoveRecordRequest, RemoveRecordResponse, RemoveRecordErrorCode>, RemoveRecordUseCase>();
        
        services.TryAddTransient<IUseCase<GetRecordRequest, GetRecordResponse, GetRecordErrorCode>, GetRecordUseCase>();
        services.TryAddTransient<IUseCase<ListRecordRequest, ListRecordResponse, ListRecordErrorCode>, ListRecordUseCase>();
        
        return services;
    }

    private static IServiceCollection AddRunningRecordsUseCases(this IServiceCollection services)
    {
        services.TryAddTransient<IUseCase<StartRunningRecordRequest, StartRunningRecordResponse, StartRunningRecordErrorCode>, StartRunningRecordUseCase>();
        services.TryAddTransient<IUseCase<StopRunningRecordRequest, StopRunningRecordResponse, StopRunningRecordErrorCode>, StopRunningRecordUseCase>();
        services.TryAddTransient<IUseCase<UpdateRunningRecordRequest, UpdateRunningRecordResponse, UpdateRunningRecordErrorCode>, UpdateRunningRecordUseCase>();
        services.TryAddTransient<IUseCase<RemoveRunningRecordRequest, RemoveRunningRecordResponse, RemoveRunningRecordErrorCode>, RemoveRunningRecordUseCase>();

        services.TryAddTransient<IUseCase<GetRunningRecordRequest, GetRunningRecordResponse, GetRunningRecordErrorCode>, GetRunningRecordUseCase>();
        
        return services;
    }

    private static IServiceCollection AddEventsUseCases(this IServiceCollection services)
    {
        services.TryAddTransient<IUseCase<ListEventRequest, ListEventResponse, ListEventErrorCode>, ListEventUseCase>();
        
        return services;
    }

    private static IServiceCollection AddUsersPasswordUseCases(this IServiceCollection services)
    {
        services.TryAddTransient<IUseCase<LoginUserPasswordRequest, LoginUserPasswordResponse, LoginUserPasswordErrorCode>, LoginUserPasswordUseCase>();
        services.TryAddTransient<IUseCase<RegisterUserPasswordRequest, RegisterUserPasswordResponse, RegisterUserPasswordErrorCode>, RegisterUserPasswordUseCase>();
        services.TryAddTransient<IUseCase<UpdateLoginCredentialsRequest, UpdateLoginCredentialsResponse, UpdateLoginCredentialsErrorCode>, UpdateLoginCredentialsUseCases>();

        return services;
    }

    private static IServiceCollection AddUsersTokensUseCases(this IServiceCollection services)
    {
        services.TryAddTransient<IUseCase<RefreshUserAccessTokenRequest, RefreshUserAccessTokenResponse, RefreshUserAccessTokenErrorCode>, RefreshUserAccessTokenUseCase>();
        
        return services;
    }
}