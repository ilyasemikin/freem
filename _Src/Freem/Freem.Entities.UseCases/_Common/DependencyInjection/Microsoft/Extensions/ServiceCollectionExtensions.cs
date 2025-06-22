using Freem.Entities.UseCases.Activities;
using Freem.Entities.UseCases.Events;
using Freem.Entities.UseCases.Records;
using Freem.Entities.UseCases.RunningRecords;
using Freem.Entities.UseCases.Statistics;
using Freem.Entities.UseCases.Tags;
using Freem.Entities.UseCases.Users;
using Freem.Entities.UseCases.Users.Password;
using Freem.Entities.UseCases.Users.Settings;
using Freem.Entities.UseCases.Users.Tokens;
using Freem.UseCases.Abstractions;
using Freem.UseCases.Implementations;
using Freem.UseCases.Implementations.Microsoft;
using Freem.UseCases.Types.DependencyInjection.Microsoft;
using Freem.UseCases.Types.DependencyInjection.Microsoft.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Freem.Entities.UseCases.DependencyInjection.Microsoft.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEntitiesUseCases(this IServiceCollection services)
    {
        services.TryAddTransient<IUseCaseResolver, MicrosoftAdapterUseCaseResolver>();
        services.TryAddTransient<IUseCaseExecutor<UseCaseExecutionContext>, UseCaseExecutor<UseCaseExecutionContext>>();
        services.AddUseCases<UseCaseExecutionContext>(builder => builder
            .AddActivitiesUseCases()
            .AddTagsUseCases()
            .AddRecordsUseCases()
            .AddRunningRecordsUseCases()
            .AddEventsUseCases()
            .AddUserUseCases()
            .AddUserSettingsUseCases()
            .AddUsersPasswordUseCases()
            .AddUsersTokensUseCases()
            .AddStatisticsUseCases());

        return services;
    }

    private static UseCasesBuilder<UseCaseExecutionContext> AddActivitiesUseCases(
        this UseCasesBuilder<UseCaseExecutionContext> builder)
    {
        builder.Add<CreateActivityUseCase>();
        builder.Add<UpdateActivityUseCase>();
        builder.Add<RemoveActivityUseCase>();
        builder.Add<ArchiveActivityUseCase>();
        builder.Add<UnarchiveActivityUseCase>();
        
        builder.Add<GetActivityUseCase>();
        builder.Add<FindActivityUseCase>();
        builder.Add<ListActivityUseCase>();
        
        return builder;
    }

    private static UseCasesBuilder<UseCaseExecutionContext> AddTagsUseCases(
        this UseCasesBuilder<UseCaseExecutionContext> builder)
    {
        builder.Add<CreateTagUseCase>();
        builder.Add<UpdateTagUseCase>();
        builder.Add<RemoveTagUseCase>();
        
        builder.Add<GetTagUseCase>();
        builder.Add<FindTagByNameUseCase>();
        builder.Add<ListTagUseCase>();
        
        return builder;
    }

    private static UseCasesBuilder<UseCaseExecutionContext> AddRecordsUseCases(
        this UseCasesBuilder<UseCaseExecutionContext> builder)
    {
        builder.Add<CreateRecordUseCase>();
        builder.Add<UpdateRecordUseCase>();
        builder.Add<RemoveRecordUseCase>();
        
        builder.Add<GetRecordUseCase>();
        builder.Add<ListRecordUseCase>();
        builder.Add<PeriodListRecordUseCase>();
        
        return builder;
    }

    private static UseCasesBuilder<UseCaseExecutionContext> AddRunningRecordsUseCases(
        this UseCasesBuilder<UseCaseExecutionContext> builder)
    {
        builder.Add<StartRunningRecordUseCase>();
        builder.Add<StopRunningRecordUseCase>();
        builder.Add<UpdateRunningRecordUseCase>();
        builder.Add<RemoveRunningRecordUseCase>();

        builder.Add<GetRunningRecordUseCase>();
        
        return builder;
    }

    private static UseCasesBuilder<UseCaseExecutionContext> AddEventsUseCases(
        this UseCasesBuilder<UseCaseExecutionContext> builder)
    {
        builder.Add<ListEventUseCase>();
        
        return builder;
    }

    private static UseCasesBuilder<UseCaseExecutionContext> AddUserUseCases(
        this UseCasesBuilder<UseCaseExecutionContext> builder)
    {
        builder.Add<GetUserUseCase>();

        return builder;
    }
    
    private static UseCasesBuilder<UseCaseExecutionContext> AddUserSettingsUseCases(
        this UseCasesBuilder<UseCaseExecutionContext> builder)
    {
        builder.Add<UpdateUserSettingsUseCase>();
        builder.Add<GetUserSettingsUseCase>();
        
        return builder;
    }
    
    private static UseCasesBuilder<UseCaseExecutionContext> AddUsersPasswordUseCases(
        this UseCasesBuilder<UseCaseExecutionContext> builder)
    {
        builder.Add<LoginUserPasswordUseCase>();
        builder.Add<RegisterUserPasswordUseCase>();
        builder.Add<UpdateLoginCredentialsUseCases>();

        return builder;
    }

    private static UseCasesBuilder<UseCaseExecutionContext> AddUsersTokensUseCases(
        this UseCasesBuilder<UseCaseExecutionContext> builder)
    {
        builder.Add<RefreshUserAccessTokenUseCase>();
        
        return builder;
    }

    private static UseCasesBuilder<UseCaseExecutionContext> AddStatisticsUseCases(
        this UseCasesBuilder<UseCaseExecutionContext> builder)
    {
        builder.Add<StatisticsPerDaysUseCase>();
        builder.Add<StatisticsPerPeriodUseCase>();

        return builder;
    }
}