using Freem.Entities.UseCases.Context;
using Freem.Entities.UseCases.Contracts.Users.Settings.Get;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class UsersSettingsGetUseCaseTests : UseCaseTestBase
{
    private HandlerContext _context = null!;
    
    public UsersSettingsGetUseCaseTests(TestContext context) 
        : base(context)
    {
    }

    public override async Task InitializeAsync()
    {
        using var filler = Context.CreateExecutor();

        var userId = await filler.Users.RegisterAsync();
        _context = new HandlerContext(userId);
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new GetUserSettingsRequest();
        
        var response = await Context.ExecuteAsync<GetUserSettingsRequest, Result<GetUserSettingsResponse, Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);
    }
    
}