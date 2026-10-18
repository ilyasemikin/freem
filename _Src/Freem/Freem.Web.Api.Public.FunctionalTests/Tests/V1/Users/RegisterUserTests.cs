using System.Net;
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

        var request = new RegisterUserRequest(UserPreparer.NicknameValue);
        
        var result = await client.Users.RegisterAsync(request);

        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
    }
}
