using Freem.Domain.Entities.Activities;
using Freem.Domain.Entities.Activities.Comparers;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Activities.Get;
using Freem.Domain.UseCases.IntegrationTests.Fixtures;
using Freem.Domain.UseCases.IntegrationTests.Tests.Abstractions;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.UseCases.IntegrationTests.Tests;

public sealed class ActivitiesGetUseCase : UseCaseTestBase
{
    private HandlerContext _context = null!;
    private Activity _activity = null!;
    
    public ActivitiesGetUseCase(TestContext context) 
        : base(context)
    {
    }

    public override async Task InitializeAsync()
    {
        using var filler = Context.CreateExecutor();
        
        var userId = await filler.Users.RegisterAsync();
        _context = new HandlerContext(userId);
        
        var activity = await filler.Activities.CreateAsync(_context);
        _activity = activity;
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new GetActivityRequest(_activity.Id);

        var response = await Context.ExecuteAsync<GetActivityRequest, Result<GetActivityResponse, Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.NotNull(response.Value.Activity);
        
        Assert.Equal(_activity, response.Value.Activity, new ActivityEqualityComparer());
    }
    
}