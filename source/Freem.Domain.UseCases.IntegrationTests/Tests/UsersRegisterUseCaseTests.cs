using Freem.Domain.Users;
using Freem.Domain.Users.Models;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Users.Register;
using Freem.Domain.UseCases.IntegrationTests.Fixtures;
using Freem.Domain.UseCases.IntegrationTests.Tests.Abstractions;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.UseCases.IntegrationTests.Tests;

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
        var context = new HandlerContext(userId);
        
        var response = await Context.ExecuteAsync<RegisterUserRequest, Result<RegisterUserResponse, Error>>(context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.NotNull(response.Value.UserId);
        Assert.Null(response.Error);
    }
}
