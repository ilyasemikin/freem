using System.Net;
using Freem.Domain.Users.Models;
using Freem.Web.Api.Public.Contracts.DTO.Users;
using Freem.Web.Api.Public.FunctionalTests.Context;
using Freem.Web.Api.Public.FunctionalTests.Context.Preparers;
using Freem.Web.Api.Public.FunctionalTests.Tests.Base;

namespace Freem.Web.Api.Public.FunctionalTests.Tests.V1.Users;

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
        Context.Preparer.Users.Authenticate();

        var nickname = Nickname.Parse(UserPreparer.NicknameValue, null);
        var request = new RegisterUserRequest(nickname);
        
        var result = await client.Users.RegisterAsync(request);

        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
    }

    [Fact]
    public async Task Request_WhenAlreadyRegistered_ShouldConflict()
    {
        var client = Context.Client;
        Context.Preparer.Users.Register();

        var nickname = Nickname.Parse(UserPreparer.NicknameValue, null);
        var request = new RegisterUserRequest(nickname);

        var result = await client.Users.RegisterAsync(request);

        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.Conflict, result.StatusCode);
    }
}
