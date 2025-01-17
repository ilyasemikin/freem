using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.DTO.Users.Password.Login;
using Freem.Entities.UseCases.DTO.Users.Password.Register;
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
        Assert.Null(response.Error);
    }

    [Fact]
    public async Task ShouldFailure_WhenLoginNotExisted()
    {
        var request = new LoginUserPasswordRequest(NotExistedLogin, Password);
        
        var response = await Services.RequestExecutor.ExecuteAsync<LoginUserPasswordRequest, LoginUserPasswordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.Null(response.AccessToken);
        Assert.Null(response.RefreshToken);
        Assert.NotNull(response.Error);
        
        Assert.Equal(LoginUserPasswordErrorCode.UserNotFound, response.Error.Code);
    }
    
    [Fact]
    public async Task ShouldFailure_WhenPasswordInvalid()
    {
        var request = new LoginUserPasswordRequest(Login, "InvalidPassword");
        
        var response = await Services.RequestExecutor.ExecuteAsync<LoginUserPasswordRequest, LoginUserPasswordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.Null(response.AccessToken);
        Assert.Null(response.RefreshToken);
        Assert.NotNull(response.Error);
        
        Assert.Equal(LoginUserPasswordErrorCode.InvalidCredentials, response.Error.Code);
    }
}