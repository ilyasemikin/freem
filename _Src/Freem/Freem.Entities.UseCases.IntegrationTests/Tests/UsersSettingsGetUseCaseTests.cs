using Freem.Entities.UseCases.Contracts.Users.Settings.Get;
using Freem.Entities.UseCases.Exceptions;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class UsersSettingsGetUseCaseTests : UseCaseTestBase
{
    private readonly UseCaseExecutionContext _context;
    
    public UsersSettingsGetUseCaseTests(TestContext context) 
        : base(context)
    {
        using var filler = Context.CreateExecutor();

        var userId = filler.UsersPassword.Register();
        _context = new UseCaseExecutionContext(userId);
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new GetUserSettingsRequest();
        
        var response = await Context.ExecuteAsync<GetUserSettingsRequest, GetUserSettingsResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);
    }
    
    [Fact]
    public async Task ShouldThrowException_WhenUnauthorized()
    {
        var request = new GetUserSettingsRequest();

        var exception = await Record.ExceptionAsync(async () => await Context
            .ExecuteAsync<GetUserSettingsRequest, GetUserSettingsResponse>(request));

        Assert.IsType<UnauthorizedException>(exception);
    }
}