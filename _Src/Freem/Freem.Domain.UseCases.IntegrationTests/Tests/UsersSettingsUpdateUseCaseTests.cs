using Freem.Domain;
using Freem.Domain.Users;
using Freem.Domain.Users.Models;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts;
using Freem.Domain.UseCases.Contracts.Users.Settings.Update;
using Freem.Domain.UseCases.IntegrationTests.Fixtures;
using Freem.Domain.UseCases.IntegrationTests.Tests.Abstractions;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.UseCases.IntegrationTests.Tests;

public sealed class UsersSettingsUpdateUseCaseTests : UseCaseTestBase
{
    private static readonly DayUtcOffset UpdatedUtcOffset = new(TimeSpan.FromHours(1));
    
    private HandlerContext _context = null!;
    
    public UsersSettingsUpdateUseCaseTests(TestContext context) 
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
    public async Task ShouldSuccess_WhenUpdateUtcOffset()
    {
        var request = new UpdateUserSettingsRequest
        {
            UtcOffset = new UpdateField<DayUtcOffset>(UpdatedUtcOffset)
        };
        
        var response = await Context.ExecuteAsync<UpdateUserSettingsRequest, Result<Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);
        
        using var executor = Context.CreateExecutor();
        var actual = await executor.UsersSettings.RequiredGetAsync(_context);
        
        Assert.Equal(UpdatedUtcOffset.Value, actual.DayUtcOffset.Value);
    }
    
    [Fact]
    public async Task ShouldFailure_WhenNothingToDo()
    {
        var request = new UpdateUserSettingsRequest();
        
        var response = await Context.ExecuteAsync<UpdateUserSettingsRequest, Result<Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);
        
        Assert.Equal(DomainErrorCatalog.Types.NothingToDo, response.Error.Type);
    }

    [Fact]
    public async Task ShouldFailure_WhenUserNotRegistered()
    {
        var notRegisteredUserId = UserId.Generate();
        var context = new HandlerContext(notRegisteredUserId);
        var request = new UpdateUserSettingsRequest
        {
            UtcOffset = new UpdateField<DayUtcOffset>(UpdatedUtcOffset)
        };
        
        var response = await Context.ExecuteAsync<UpdateUserSettingsRequest, Result<Error>>(context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);
        
        Assert.Equal(DomainErrorCatalog.Types.UserNotRegistered, response.Error.Type);
    }
    
}