﻿using Freem.Entities.UseCases.Contracts.Users.Password.Register;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class UsersPasswordRegisterUseCaseTests : UseCaseTestBase
{
    private const string Nickname = "user";
    private const string Login = "user";
    private const string Password = "password";
    
    public UsersPasswordRegisterUseCaseTests(TestContext context)
        : base(context)
    {
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new RegisterUserPasswordRequest(Nickname, Login, Password);
        
        var response = await Context.ExecuteAsync<RegisterUserPasswordRequest, RegisterUserPasswordResponse>(
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
        using var executor = Context.CreateExecutor();
        executor.UsersPassword.Register(Login);
        
        var request = new RegisterUserPasswordRequest(Nickname, Login, Password);

        var response = await Context.ExecuteAsync<RegisterUserPasswordRequest, RegisterUserPasswordResponse>(UseCaseExecutionContext.Empty, request);

        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.Null(response.UserId);
        Assert.NotNull(response.Error);
        
        Assert.Equal(RegisterUserPasswordErrorCode.LoginAlreadyUsed, response.Error.Code);
    }
}