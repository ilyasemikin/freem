using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.Activities.Create.Models;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Entities.UseCases.Users.Password.Register.Models;
using ActivityStatus = Freem.Entities.Activities.Models.ActivityStatus;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class ActivitiesCreateUseCase : UseCaseTestBase
{
    private const string ActivityName = "activity";

    private readonly UseCaseExecutionContext _context;
    
    public ActivitiesCreateUseCase(ServicesContext services) 
        : base(services)
    {
        var userId = services.Samples.Users.Register();
        _context = new UseCaseExecutionContext(userId);
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new CreateActivityRequest(ActivityName);
        
        var response = await Services.RequestExecutor.ExecuteAsync<CreateActivityRequest, CreateActivityResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.NotNull(response.Activity);
        
        Assert.Equal(_context.UserId, response.Activity.UserId);
        Assert.Equal(ActivityName, response.Activity.Name);
        Assert.Equal(ActivityStatus.Active, response.Activity.Status);
        Assert.Equal(0, response.Activity.Tags.Count);
    }
}