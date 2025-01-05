using Freem.Entities.Activities.Identifiers;
using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.Activities.Create.Models;
using Freem.Entities.UseCases.Activities.Remove.Models;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Entities.UseCases.Users.Password.Register.Models;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class ActivitiesRemoveUseCase : UseCaseTestBase
{
    private readonly UseCaseExecutionContext _context;
    private readonly ActivityIdentifier _activityId;
    
    public ActivitiesRemoveUseCase(ServicesContext services) 
        : base(services)
    {
        var userId = services.Samples.Users.Register();
        var activity = services.Samples.Activities.Create(userId);

        _context = new UseCaseExecutionContext(userId);
        _activityId = activity.Id;
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new RemoveActivityRequest(_activityId);

        await Services.RequestExecutor.ExecuteAsync(_context, request);
    }
}