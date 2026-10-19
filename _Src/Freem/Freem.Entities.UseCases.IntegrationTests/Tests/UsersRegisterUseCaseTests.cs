using Freem.Domain.Users;
using Freem.Domain.Users.Models;
using Freem.Entities.UseCases.Contracts.Users.Register;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class UsersRegisterUseCaseTests : UseCaseTestBase
{
    private static readonly Nickname Nickname = Nickname.Parse("user", null);
    private const string UserIdValue = "user-id";
    
    public UsersRegisterUseCaseTests(TestContext context)
        : base(context)
    {
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new RegisterUserRequest(Nickname);
        var userId = UserId.Parse(UserIdValue, null);
        var context = new UseCaseExecutionContext(userId);
        
        var response = await Context.ExecuteAsync<RegisterUserRequest, RegisterUserResponse>(context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.NotNull(response.UserId);
        Assert.Null(response.Error);
    }
}
