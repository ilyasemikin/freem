using System.Net;
using Freem.Api.Public.Contracts.V1.Users;
using Freem.Domain.Users.Models;
using Freem.Api.Public.IntegrationTests.Context;
using Freem.Api.Public.IntegrationTests.Context.Preparers;
using Freem.Api.Public.IntegrationTests.Extensions;
using Freem.Api.Public.IntegrationTests.Tests.Base;

namespace Freem.Api.Public.IntegrationTests.Tests.V1.Users;

public sealed class RegisterUserTests : TestBase
{
    public RegisterUserTests(TestContext context) 
        : base(context)
    {
    }

    [Fact]
    public async Task Request_ShouldSuccess()
    {
        var client = Context.Client;
        await Context.Preparer.Users.AuthenticateAsync();

        var nickname = Nickname.Parse(UserPreparer.NicknameValue, null);
        var request = new UserRegisterRequest(nickname);
        
        var result = await client.Users.RegisterAsync(request);

        Assert.NotNull(result);
        result.ShouldSuccess();
    }

    [Fact]
    public async Task Request_WhenAlreadyRegistered_ShouldConflict()
    {
        var client = Context.Client;
        await Context.Preparer.Users.RegisterAsync();

        var nickname = Nickname.Parse(UserPreparer.NicknameValue, null);
        var request = new UserRegisterRequest(nickname);

        var result = await client.Users.RegisterAsync(request);

        Assert.NotNull(result);
        result.ShouldHaveStatusCode(HttpStatusCode.Conflict);
    }
}
