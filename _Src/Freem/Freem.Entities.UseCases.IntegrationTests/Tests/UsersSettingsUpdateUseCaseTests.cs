using Freem.Entities.UseCases.Contracts.RunningRecords.Update;
using Freem.Entities.UseCases.Contracts.Users.Settings.Update;
using Freem.Entities.UseCases.Exceptions;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class UsersSettingsUpdateUseCaseTests : UseCaseTestBase
{
    private static readonly TimeSpan UpdatedUtcOffset = TimeSpan.FromHours(1);
    
    private readonly UseCaseExecutionContext _context;
    
    public UsersSettingsUpdateUseCaseTests(TestContext context) 
        : base(context)
    {
        using var filler = Context.CreateExecutor();

        var userId = filler.UsersPassword.Register();
        _context = new UseCaseExecutionContext(userId);
    }

    [Fact]
    public async Task ShouldSuccess_WhenUpdateUtcOffset()
    {
        var request = new UpdateUserSettingsRequest
        {
            UtcOffset = UpdatedUtcOffset
        };
        
        var response = await Context.ExecuteAsync<UpdateUserSettingsRequest, UpdateUserSettingsResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);
        
        using var executor = Context.CreateExecutor();
        var actual = executor.UsersSettings.RequiredGet(_context);
        
        Assert.Equal(UpdatedUtcOffset, actual.UtcOffset);
    }
    
    [Fact]
    public async Task ShouldFailure_WhenNothingToDo()
    {
        var request = new UpdateUserSettingsRequest();
        
        var response = await Context.ExecuteAsync<UpdateUserSettingsRequest, UpdateUserSettingsResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);
        
        Assert.Equal(UpdateUserSettingsErrorCode.NothingToDo, response.Error.Code);
    }
    
    [Fact]
    public async Task ShouldThrowException_WhenUnauthorized()
    {
        var request = new UpdateUserSettingsRequest();

        var exception = await Record.ExceptionAsync(async () => await Context
            .ExecuteAsync<UpdateUserSettingsRequest, UpdateUserSettingsResponse>(request));

        Assert.IsType<UnauthorizedException>(exception);
    }
}