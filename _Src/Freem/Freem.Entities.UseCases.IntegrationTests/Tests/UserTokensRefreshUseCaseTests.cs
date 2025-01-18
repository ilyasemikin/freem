using Freem.Entities.UseCases.Contracts.Users.Password.Login;
using Freem.Entities.UseCases.Contracts.Users.Password.Register;
using Freem.Entities.UseCases.Contracts.Users.Tokens.Refresh;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class UserTokensRefreshUseCaseTests : UseCaseTestBase
{
    private const string Nickname = "user";
    private const string Login = "user";
    private const string Password = "password";

    private const string InvalidToken = "invalid_token";
    
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
        Assert.Null(response.Error);
        
        Assert.NotEqual(_token, response.RefreshToken);
    }

    [Fact]
    public async Task ShouldFailure_WhenUserDoesNotExist()
    {
        var request = new RefreshUserAccessTokenRequest(InvalidToken);
        
        var response = await Services.RequestExecutor.ExecuteAsync<RefreshUserAccessTokenRequest, RefreshUserAccessTokenResponse>(
            UseCaseExecutionContext.Empty, 
            request);
        
        Assert.False(response.Success);
        Assert.Null(response.AccessToken);
        Assert.Null(response.RefreshToken);
        Assert.NotNull(response.Error);
        
        Assert.Equal(RefreshUserAccessTokenErrorCode.TokenInvalid, response.Error.Code);
    }
}