using Freem.Entities.Identifiers;
using Freem.Entities.UseCases.Contracts.Users.Password.Login;
using Freem.Entities.UseCases.Contracts.Users.Password.Register;
using Freem.Entities.UseCases.Contracts.Users.Password.Update;
using Freem.Entities.UseCases.Exceptions;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class UsersPasswordUpdateUseCaseTests : UseCaseTestBase
{
    private const string Nickname = "user";
    private const string Login = "user";
    private const string OldPassword = "old_password";
    private const string NewPassword = "new_password";
    
    private const string InvalidOldPassword = "invalid_old_password";

    private readonly UseCaseExecutionContext _context;
    
    public UsersPasswordUpdateUseCaseTests(TestContext context) 
        : base(context)
    {
        using var filler = Context.CreateExecutor();
        
        var request = new RegisterUserPasswordRequest(Nickname, Login, OldPassword);
        var userId = filler.UsersPassword.Register(request);
        _context = new UseCaseExecutionContext(userId);
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new UpdateLoginCredentialsRequest(OldPassword, NewPassword);

        var response = await Context.ExecuteAsync<UpdateLoginCredentialsRequest, UpdateLoginCredentialsResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);
        
        using var executor = Context.CreateExecutor();
        var loginRequest = new LoginUserPasswordRequest(Login, NewPassword);
        executor.UsersPassword.Login(loginRequest);
    }

    [Fact]
    public async Task ShouldFailure_WhenUserDoesNotExist()
    {
        var userId = Context.CreateIdentifier<UserIdentifier>();
        var context = new UseCaseExecutionContext(userId);
        
        var request = new UpdateLoginCredentialsRequest(OldPassword, NewPassword);
        
        var response = await Context.ExecuteAsync<UpdateLoginCredentialsRequest, UpdateLoginCredentialsResponse>(context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);

        Assert.Equal(UpdateLoginCredentialsErrorCode.UserNotFound, response.Error.Code);
    }
    
    [Fact]
    public async Task ShouldFailure_WhenOldPasswordIsInvalid()
    {
        var request = new UpdateLoginCredentialsRequest(InvalidOldPassword, NewPassword);
        
        var response = await Context.ExecuteAsync<UpdateLoginCredentialsRequest, UpdateLoginCredentialsResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);
        
        Assert.Equal(UpdateLoginCredentialsErrorCode.InvalidCredentials, response.Error.Code);
    }
    
    [Fact]
    public async Task ShouldThrowException_WhenUnauthorized()
    {
        var request = new UpdateLoginCredentialsRequest(OldPassword, NewPassword);
        
        var exception = await Record.ExceptionAsync(async () => await Context
            .ExecuteAsync<UpdateLoginCredentialsRequest, UpdateLoginCredentialsResponse>(request));
        
        Assert.IsType<UnauthorizedException>(exception);
    }
}