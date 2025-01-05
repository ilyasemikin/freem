using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Entities.UseCases.Users.Password.Login.Models;
using Freem.Entities.UseCases.Users.Password.Register.Models;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class UsersPasswordLoginUseCaseTests : UseCaseTestBase
{
    private const string Nickname = "user";
    private const string Login = "user";
    private const string Password = "password";

    private readonly UseCaseExecutionContext _context;
    
    public UsersPasswordLoginUseCaseTests(ServicesContext services) 
        : base(services)
    {
        var request = new RegisterUserPasswordRequest(Nickname, Login, Password);
        var response = Services.RequestExecutor.Execute<RegisterUserPasswordRequest, RegisterUserPasswordResponse>(UseCaseExecutionContext.Empty, request);

        _context = new UseCaseExecutionContext(response.UserId);
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new LoginUserPasswordRequest(Login, Password);

        var response = await Services.RequestExecutor.ExecuteAsync<LoginUserPasswordRequest, LoginUserPasswordResponse>(_context, request);

        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.NotEmpty(response.AccessToken);
        Assert.NotEmpty(response.RefreshToken);
    }

    [Fact]
    public async Task ShouldFail_WhenPasswordInvalid()
    {
        var request = new LoginUserPasswordRequest(Login, "InvalidPassword");
        
        var response = await Services.RequestExecutor.ExecuteAsync<LoginUserPasswordRequest, LoginUserPasswordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
    }
}