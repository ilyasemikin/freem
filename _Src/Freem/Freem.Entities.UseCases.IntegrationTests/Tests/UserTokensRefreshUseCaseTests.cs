﻿using Freem.Entities.UseCases.Contracts.Users.Password.Login;
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
    
    public UserTokensRefreshUseCaseTests(TestContext context) 
        : base(context)
    {
        using var filler = Context.CreateExecutor();
        
        var registerRequest = new RegisterUserPasswordRequest(Nickname, Login, Password);
        filler.UsersPassword.Register(registerRequest);

        var loginRequest = new LoginUserPasswordRequest(Login, Password);
        var tokens = filler.UsersPassword.Login(loginRequest);
        _token = tokens.RefreshToken;
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new RefreshUserAccessTokenRequest(_token);
        
        var response = await Context.ExecuteAsync<RefreshUserAccessTokenRequest, RefreshUserAccessTokenResponse>(
            UseCaseExecutionContext.Empty, 
            request);
        
        Assert.True(response.Success);
        Assert.NotNull(response.Tokens);
        Assert.Null(response.Error);
        
        Assert.NotEqual(_token, response.Tokens.RefreshToken);
    }

    [Fact]
    public async Task ShouldFailure_WhenUserDoesNotExist()
    {
        var request = new RefreshUserAccessTokenRequest(InvalidToken);
        
        var response = await Context.ExecuteAsync<RefreshUserAccessTokenRequest, RefreshUserAccessTokenResponse>(
            UseCaseExecutionContext.Empty, 
            request);
        
        Assert.False(response.Success);
        Assert.Null(response.Tokens);
        Assert.NotNull(response.Error);
        
        Assert.Equal(RefreshUserAccessTokenErrorCode.TokenInvalid, response.Error.Code);
    }
}