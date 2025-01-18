﻿using Freem.Entities.UseCases.Activities;
using Freem.Entities.UseCases.Events;
using Freem.Entities.UseCases.Records;
using Freem.Entities.UseCases.RunningRecords;
using Freem.Entities.UseCases.Tags;
using Freem.Entities.UseCases.Users.Password;
using Freem.Entities.UseCases.Users.Tokens;
using Freem.UseCases.Abstractions;
using Freem.UseCases.Implementations;
using Freem.UseCases.Types.DependencyInjection.Microsoft;
using Freem.UseCases.Types.DependencyInjection.Microsoft.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Freem.Entities.UseCases.DependencyInjection.Microsoft.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEntitiesUseCases(this IServiceCollection services)
    { 
        services.AddTransient<IUseCaseExecutor<UseCaseExecutionContext>, UseCaseExecutor<UseCaseExecutionContext>>();
        services.AddUseCases<UseCaseExecutionContext>(builder => builder
            .AddActivitiesUseCases()
            .AddTagsUseCases()
            .AddRecordsUseCases()
            .AddRunningRecordsUseCases()
            .AddEventsUseCases()
            .AddUsersPasswordUseCases()
            .AddUsersTokensUseCases());

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
}