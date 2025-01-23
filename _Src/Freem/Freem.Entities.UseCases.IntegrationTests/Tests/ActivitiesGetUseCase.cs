using Freem.Entities.Activities;
using Freem.Entities.Activities.Comparers;
using Freem.Entities.UseCases.Contracts.Activities.Get;
using Freem.Entities.UseCases.Exceptions;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class ActivitiesGetUseCase : UseCaseTestBase
{
    private readonly UseCaseExecutionContext _context;
    private readonly Activity _activity;
    
    public ActivitiesGetUseCase(TestContext context) 
        : base(context)
    {
        using var filler = Context.CreateExecutor();
        
        var userId = filler.UsersPassword.Register();
        _context = new UseCaseExecutionContext(userId);
        
        var activity = filler.Activities.Create(_context);
        _activity = activity;
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new GetActivityRequest(_activity.Id);

        var response = await Context.ExecuteAsync<GetActivityRequest, GetActivityResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.NotNull(response.Activity);
        
        Assert.Equal(_activity, response.Activity, new ActivityEqualityComparer());
    }
    
    [Fact]
    public async Task ShouldThrowException_WhenUnauthorized()
    {
        var request = new GetActivityRequest(_activity.Id);
        
        var exception = await Record.ExceptionAsync(async () => await Context
            .ExecuteAsync<GetActivityRequest, GetActivityResponse>(request));
        
        Assert.IsType<UnauthorizedException>(exception);
    }
}