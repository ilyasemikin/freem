﻿using Freem.Entities.UseCases.Contracts.Users.Password.Login;
using Freem.Entities.UseCases.Contracts.Users.Password.Register;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class UsersPasswordLoginUseCaseTests : UseCaseTestBase
{
    private const string Nickname = "user";
    private const string Login = "user";
    private const string Password = "password";

    private const string NotExistedLogin = "not_existed_user";

    private readonly UseCaseExecutionContext _context;
    
    public UsersPasswordLoginUseCaseTests(TestContext context) 
        : base(context)
    {
        using var filler = Context.CreateExecutor();
        
        var request = new RegisterUserPasswordRequest(Nickname, Login, Password);
        var userId = filler.UsersPassword.Register(request);
        _context = new UseCaseExecutionContext(userId);
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new LoginUserPasswordRequest(Login, Password);

        var response = await Context.ExecuteAsync<LoginUserPasswordRequest, LoginUserPasswordResponse>(_context, request);

        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.NotNull(response.Tokens);
        Assert.Null(response.Error);
    }

    [Fact]
    public async Task ShouldFailure_WhenLoginNotExisted()
    {
        var request = new LoginUserPasswordRequest(NotExistedLogin, Password);
        
        var response = await Context.ExecuteAsync<LoginUserPasswordRequest, LoginUserPasswordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.Null(response.Tokens);
        Assert.NotNull(response.Error);
        
        Assert.Equal(LoginUserPasswordErrorCode.UserNotFound, response.Error.Code);
    }
    
    [Fact]
    public async Task ShouldFailure_WhenPasswordInvalid()
    {
        var request = new LoginUserPasswordRequest(Login, "InvalidPassword");
        
        var response = await Context.ExecuteAsync<LoginUserPasswordRequest, LoginUserPasswordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.Null(response.Tokens);
        Assert.NotNull(response.Error);
        
        Assert.Equal(LoginUserPasswordErrorCode.InvalidCredentials, response.Error.Code);
    }
}