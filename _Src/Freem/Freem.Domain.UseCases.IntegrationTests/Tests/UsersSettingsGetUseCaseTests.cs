using Freem.Domain.Users;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Users.Settings.Get;
using Freem.Domain.UseCases.IntegrationTests.Fixtures;
using Freem.Domain.UseCases.IntegrationTests.Tests.Abstractions;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.UseCases.IntegrationTests.Tests;

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

    [Fact]
    public async Task ShouldFailure_WhenUserNotRegistered()
    {
        var notRegisteredUserId = UserId.Generate();
        var context = new HandlerContext(notRegisteredUserId);
        var request = new GetUserSettingsRequest();
        
        var response = await Context.ExecuteAsync<GetUserSettingsRequest, Result<GetUserSettingsResponse, Error>>(context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);
        
        Assert.Equal(DomainErrorCatalog.Types.UserNotRegistered, response.Error.Type);
    }
}