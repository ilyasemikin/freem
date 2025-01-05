using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Entities.UseCases.Users.Password.Register.Models;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class UsersPasswordRegisterUseCaseTests : UseCaseTestBase
{
    public UsersPasswordRegisterUseCaseTests(ServicesContext services)
        : base(services)
    {
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        const string nickname = "user";
        const string login = "user";
        const string password = "password";
        
        var request = new RegisterUserPasswordRequest(nickname, login, password);
        
        var response = await Services.RequestExecutor.ExecuteAsync<RegisterUserPasswordRequest, RegisterUserPasswordResponse>(
            UseCaseExecutionContext.Empty, 
            request);
        
        Assert.NotNull(response);
        Assert.NotNull(response.UserId);
    }
}