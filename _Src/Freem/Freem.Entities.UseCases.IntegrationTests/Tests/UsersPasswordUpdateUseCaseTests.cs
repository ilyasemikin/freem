using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.Abstractions.Exceptions;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Entities.UseCases.Users.Password.Register.Models;
using Freem.Entities.UseCases.Users.Password.Update.Models;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class UsersPasswordUpdateUseCaseTests : UseCaseTestBase
{
    private const string Nickname = "user";
    private const string Login = "user";
    private const string OldPassword = "old_password";
    private const string NewPassword = "new_password";
    
    private const string InvalidOldPassword = "invalid_old_password";

    private readonly UseCaseExecutionContext _context;
    
    public UsersPasswordUpdateUseCaseTests(ServicesContext services) 
        : base(services)
    {
        var request = new RegisterUserPasswordRequest(Nickname, Login, OldPassword);
        var response = Services.RequestExecutor.Execute<RegisterUserPasswordRequest, RegisterUserPasswordResponse>(UseCaseExecutionContext.Empty, request);

        _context = new UseCaseExecutionContext(response.UserId);
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new UpdateLoginCredentialsRequest(OldPassword, NewPassword);

        var response = await Services.RequestExecutor.ExecuteAsync<UpdateLoginCredentialsRequest, UpdateLoginCredentialsResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Null(response.Error);
    }

    [Fact]
    public async Task ShouldFailure_WhenUserDoesNotExist()
    {
        var userId = Services.Generators.CreateUserIdentifier();
        var context = new UseCaseExecutionContext(userId);
        
        var request = new UpdateLoginCredentialsRequest(OldPassword, NewPassword);
        
        var response = await Services.RequestExecutor.ExecuteAsync<UpdateLoginCredentialsRequest, UpdateLoginCredentialsResponse>(context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);

        Assert.Equal(UpdateLoginCredentialsErrorCode.UserNotFound, response.Error.Code);
    }
    
    [Fact]
    public async Task ShouldFailure_WhenOldPasswordIsInvalid()
    {
        var request = new UpdateLoginCredentialsRequest(InvalidOldPassword, NewPassword);
        
        var response = await Services.RequestExecutor.ExecuteAsync<UpdateLoginCredentialsRequest, UpdateLoginCredentialsResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.NotNull(response.Error);
        
        Assert.Equal(UpdateLoginCredentialsErrorCode.InvalidCredentials, response.Error.Code);
    }
    
    [Fact]
    public async Task ShouldThrowException_WhenUnauthorized()
    {
        var request = new UpdateLoginCredentialsRequest(OldPassword, NewPassword);
        
        var exception = await Record.ExceptionAsync(async () => await Services.RequestExecutor
            .ExecuteAsync<UpdateLoginCredentialsRequest, UpdateLoginCredentialsResponse>(UseCaseExecutionContext.Empty, request));
        
        Assert.IsType<UnauthorizedException>(exception);
    }
}