using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Entities.UseCases.Users.Password.Login.Models;
using Freem.Entities.UseCases.Users.Password.Register.Models;
using Freem.Entities.UseCases.Users.Tokens.Refresh.Models;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class UserTokensRefreshUseCaseTests : UseCaseTestBase
{
    private const string Nickname = "user";
    private const string Login = "user";
    private const string Password = "password";

    private readonly string _token;
    
    public UserTokensRefreshUseCaseTests(ServicesContext services) 
        : base(services)
    {
        var registerRequest = new RegisterUserPasswordRequest(Nickname, Login, Password);
        Services.RequestExecutor.Execute<RegisterUserPasswordRequest, RegisterUserPasswordResponse>(UseCaseExecutionContext.Empty, registerRequest);

        var loginRequest = new LoginUserPasswordRequest(Login, Password);
        var response = Services.RequestExecutor.Execute<LoginUserPasswordRequest, LoginUserPasswordResponse>(UseCaseExecutionContext.Empty, loginRequest);

        if (!response.Success)
            throw new InvalidOperationException($"Login unsuccessful");
        
        _token = response.RefreshToken;
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new RefreshUserAccessTokenRequest(_token);
        
        var response = await Services.RequestExecutor.ExecuteAsync<RefreshUserAccessTokenRequest, RefreshUserAccessTokenResponse>(
            UseCaseExecutionContext.Empty, 
            request);
        
        Assert.True(response.Success);
        Assert.NotNull(response.AccessToken);
        Assert.NotNull(response.RefreshToken);
        Assert.NotEqual(_token, response.RefreshToken);
    }
}