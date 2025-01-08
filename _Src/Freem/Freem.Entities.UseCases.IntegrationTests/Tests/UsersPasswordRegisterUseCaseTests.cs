using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Entities.UseCases.Users.Password.Register.Models;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class UsersPasswordRegisterUseCaseTests : UseCaseTestBase
{
    private const string Nickname = "user";
    private const string Login = "user";
    private const string Password = "password";
    
    public UsersPasswordRegisterUseCaseTests(ServicesContext services)
        : base(services)
    {
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new RegisterUserPasswordRequest(Nickname, Login, Password);
        
        var response = await Services.RequestExecutor.ExecuteAsync<RegisterUserPasswordRequest, RegisterUserPasswordResponse>(
            UseCaseExecutionContext.Empty, 
            request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.NotNull(response.UserId);
        Assert.Null(response.Error);
    }

    [Fact]
    public async Task ShouldFailure_WhenLoginAlreadyUsed()
    {
        Services.Samples.Users.Register();
        
        var request = new RegisterUserPasswordRequest(Nickname, Login, Password);

        var response = await Services.RequestExecutor.ExecuteAsync<RegisterUserPasswordRequest, RegisterUserPasswordResponse>(UseCaseExecutionContext.Empty, request);

        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.Null(response.UserId);
        Assert.NotNull(response.Error);
        
        Assert.Equal(RegisterUserPasswordErrorCode.LoginAlreadyUsed, response.Error.Code);
    }
}