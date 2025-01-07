using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Activities.Archive;
using Freem.Entities.UseCases.Activities.Archive.Models;
using Freem.Entities.UseCases.Activities.Create;
using Freem.Entities.UseCases.Activities.Create.Models;
using Freem.Entities.UseCases.Activities.Get;
using Freem.Entities.UseCases.Activities.Get.Models;
using Freem.Entities.UseCases.Activities.List;
using Freem.Entities.UseCases.Activities.List.Models;
using Freem.Entities.UseCases.Activities.Remove;
using Freem.Entities.UseCases.Activities.Remove.Models;
using Freem.Entities.UseCases.Activities.Unarchive;
using Freem.Entities.UseCases.Activities.Unarchive.Models;
using Freem.Entities.UseCases.Activities.Update;
using Freem.Entities.UseCases.Activities.Update.Models;
using Freem.Entities.UseCases.Events.List;
using Freem.Entities.UseCases.Events.List.Models;
using Freem.Entities.UseCases.Records.Create;
using Freem.Entities.UseCases.Records.Create.Models;
using Freem.Entities.UseCases.Records.Get;
using Freem.Entities.UseCases.Records.Get.Models;
using Freem.Entities.UseCases.Records.List;
using Freem.Entities.UseCases.Records.List.Models;
using Freem.Entities.UseCases.Records.Remove;
using Freem.Entities.UseCases.Records.Remove.Models;
using Freem.Entities.UseCases.Records.Update;
using Freem.Entities.UseCases.Records.Update.Models;
using Freem.Entities.UseCases.RunningRecords.Get;
using Freem.Entities.UseCases.RunningRecords.Get.Models;
using Freem.Entities.UseCases.RunningRecords.Remove;
using Freem.Entities.UseCases.RunningRecords.Remove.Models;
using Freem.Entities.UseCases.RunningRecords.Start;
using Freem.Entities.UseCases.RunningRecords.Start.Models;
using Freem.Entities.UseCases.RunningRecords.Stop;
using Freem.Entities.UseCases.RunningRecords.Stop.Models;
using Freem.Entities.UseCases.RunningRecords.Update;
using Freem.Entities.UseCases.RunningRecords.Update.Models;
using Freem.Entities.UseCases.Tags.Create;
using Freem.Entities.UseCases.Tags.Create.Models;
using Freem.Entities.UseCases.Tags.Get;
using Freem.Entities.UseCases.Tags.Get.Models;
using Freem.Entities.UseCases.Tags.List;
using Freem.Entities.UseCases.Tags.List.Models;
using Freem.Entities.UseCases.Tags.Remove;
using Freem.Entities.UseCases.Tags.Remove.Models;
using Freem.Entities.UseCases.Tags.Update;
using Freem.Entities.UseCases.Tags.Update.Models;
using Freem.Entities.UseCases.Users.Password.Login;
using Freem.Entities.UseCases.Users.Password.Login.Models;
using Freem.Entities.UseCases.Users.Password.Register;
using Freem.Entities.UseCases.Users.Password.Register.Models;
using Freem.Entities.UseCases.Users.Password.Update;
using Freem.Entities.UseCases.Users.Password.Update.Models;
using Freem.Entities.UseCases.Users.Tokens.Refresh;
using Freem.Entities.UseCases.Users.Tokens.Refresh.Models;
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
        services.TryAddTransient<IUseCase<CreateActivityRequest, CreateActivityResponse>, CreateActivityUseCase>();
        services.TryAddTransient<IUseCase<UpdateActivityRequest>, UpdateActivityUseCase>();
        services.TryAddTransient<IUseCase<RemoveActivityRequest>, RemoveActivityUseCase>();
        services.TryAddTransient<IUseCase<ArchiveActivityRequest>, ArchiveActivityUseCase>();
        services.TryAddTransient<IUseCase<UnarchiveActivityRequest>, UnarchiveActivityUseCase>();
        
        services.TryAddTransient<IUseCase<GetActivityRequest, GetActivityResponse>, GetActivityUseCase>();
        services.TryAddTransient<IUseCase<ListActivityRequest, ListActivityResponse>, ListActivityUseCase>();
        
        return services;
    }

    private static IServiceCollection AddTagsUseCases(this IServiceCollection services)
    {
        services.TryAddTransient<IUseCase<CreateTagRequest, CreateTagResponse>, CreateTagUseCase>();
        services.TryAddTransient<IUseCase<UpdateTagRequest>, UpdateTagUseCase>();
        services.TryAddTransient<IUseCase<RemoveTagRequest>, RemoveTagUseCase>();
        
        services.TryAddTransient<IUseCase<GetTagRequest, GetTagResponse>, GetTagUseCase>();
        services.TryAddTransient<IUseCase<ListTagRequest, ListTagResponse>, ListTagUseCase>();
        
        return services;
    }

    private static IServiceCollection AddRecordsUseCases(this IServiceCollection services)
    {
        services.TryAddTransient<IUseCase<CreateRecordRequest, CreateRecordResponse>, CreateRecordUseCase>();
        services.TryAddTransient<IUseCase<UpdateRecordRequest>, UpdateRecordUseCase>();
        services.TryAddTransient<IUseCase<RemoveRecordRequest>, RemoveRecordUseCase>();
        
        services.TryAddTransient<IUseCase<GetRecordRequest, GetRecordResponse>, GetRecordUseCase>();
        services.TryAddTransient<IUseCase<ListRecordRequest, ListRecordResponse>, ListRecordUseCase>();
        
        return services;
    }

    private static IServiceCollection AddRunningRecordsUseCases(this IServiceCollection services)
    {
        services.TryAddTransient<IUseCase<StartRunningRecordRequest, StartRunningRecordResponse>, StartRunningRecordUseCase>();
        services.TryAddTransient<IUseCase<StopRunningRecordRequest>, StopRunningRecordUseCase>();
        services.TryAddTransient<IUseCase<UpdateRunningRecordRequest>, UpdateRunningRecordUseCase>();
        services.TryAddTransient<IUseCase<RemoveRunningRecordRequest>, RemoveRunningRecordUseCase>();

        services.TryAddTransient<IUseCase<GetRunningRecordRequest, GetRunningRecordResponse>, GetRunningRecordUseCase>();
        
        return services;
    }

    private static IServiceCollection AddEventsUseCases(this IServiceCollection services)
    {
        services.TryAddTransient<IUseCase<ListEventRequest, ListEventResponse>, ListEventUseCase>();
        
        return services;
    }

    private static IServiceCollection AddUsersPasswordUseCases(this IServiceCollection services)
    {
        services.TryAddTransient<IUseCase<LoginUserPasswordRequest, LoginUserPasswordResponse>, LoginUserPasswordUseCase>();
        services.TryAddTransient<IUseCase<RegisterUserPasswordRequest, RegisterUserPasswordResponse>, RegisterUserPasswordUseCase>();
        services.TryAddTransient<IUseCase<UpdateLoginCredentialsRequest, UpdateLoginCredentialsResponse>, UpdateLoginCredentialsUseCases>();

        return services;
    }

    private static IServiceCollection AddUsersTokensUseCases(this IServiceCollection services)
    {
        services.TryAddTransient<IUseCase<RefreshUserAccessTokenRequest, RefreshUserAccessTokenResponse>, RefreshUserAccessTokenUseCase>();
        
        return services;
    }
}