using Freem.Entities.Activities;
using Freem.Entities.Activities.Comparers;
using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.Activities.Create.Models;
using Freem.Entities.UseCases.Activities.Get.Models;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Entities.UseCases.Users.Password.Register.Models;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class ActivitiesGetUseCase : UseCaseTestBase
{
    private readonly UseCaseExecutionContext _context;
    private readonly Activity _activity;
    
    public ActivitiesGetUseCase(ServicesContext services) 
        : base(services)
    {
        var userId = services.Samples.Users.Register();
        var activity = services.Samples.Activities.Create(userId);

        _context = new UseCaseExecutionContext(userId);
        _activity = activity;
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new GetActivityRequest(_activity.Id);

        var response = await Services.RequestExecutor.ExecuteAsync<GetActivityRequest, GetActivityResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Founded);
        Assert.NotNull(response.Activity);
        
        Assert.Equal(_activity, response.Activity, new ActivityEqualityComparer());
    }
}