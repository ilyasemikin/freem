using Freem.Entities.UseCases.Contracts.Users.Register;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class UsersRegisterUseCaseTests : UseCaseTestBase
{
    private const string Nickname = "user";
    private const string UserId = "user-id";
    
    public UsersRegisterUseCaseTests(TestContext context)
        : base(context)
    {
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new RegisterUserRequest(Nickname);
        
        var response = await Context.ExecuteAsync<RegisterUserRequest, RegisterUserResponse>(
            new UseCaseExecutionContext(UserId), 
            request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.NotNull(response.UserId);
        Assert.Null(response.Error);
    }
}
