using Freem.Entities.UseCases.Context;
using Freem.Entities.UseCases.IntegrationTests.Fixtures.Executors;
using Microsoft.Extensions.DependencyInjection;
using Utilities.UseCases.Abstractions;

namespace Freem.Entities.UseCases.IntegrationTests.Fixtures;

public sealed class UseCaseExecutors : IDisposable
{
    private readonly IServiceScope _scope;

    public ActivitiesExecutor Activities { get; }
    public TagsExecutor Tags { get; }
    public RecordsExecutor Records { get; }
    public RunningRecordsExecutor RunningRecords { get; }
    public UsersExecutor Users { get; }
    public UsersSettingsExecutor UsersSettings { get; }

    private UseCaseExecutors(IServiceScope scope, IRequestMediator<HandlerContext> mediator)
    {
        _scope = scope;

        Activities = new ActivitiesExecutor(mediator);
        Tags = new TagsExecutor(mediator);
        Records = new RecordsExecutor(mediator);
        RunningRecords = new RunningRecordsExecutor(mediator);
        Users = new UsersExecutor(mediator);
        UsersSettings = new UsersSettingsExecutor(mediator);
    }

    public void Dispose()
    {
        _scope.Dispose();
    }

    public static UseCaseExecutors Create(IServiceProvider provider)
    {
        var scope = provider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IRequestMediator<HandlerContext>>();

        return new UseCaseExecutors(scope, mediator);
    }
}