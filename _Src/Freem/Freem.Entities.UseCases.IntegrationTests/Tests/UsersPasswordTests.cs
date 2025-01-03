using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Entities.UseCases.Users.Password.Register.Models;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class UsersPasswordTests : UseCaseTestBase
{
    public UsersPasswordTests(ServicesContext context)
        : base(context)
    {
    }

    [Fact]
    public async Task Register_ShouldSuccess()
    {
        var request = new RegisterUserPasswordRequest
        {
            Login = "user",
            Nickname = "user",
            Password = "password"
        };

        var context = new UseCaseExecutionContext();
        await Context.RequestExecutor.ExecuteAsync(context, request);
    }
}