using System.Net;
using Freem.Web.Api.Public.Contracts.Users.LoginPassword;
using Freem.Web.Api.Public.FunctionalTests.Context;
using Freem.Web.Api.Public.FunctionalTests.Context.Preparers;
using Freem.Web.Api.Public.FunctionalTests.Tests.Base;
using Xunit.Abstractions;

namespace Freem.Web.Api.Public.FunctionalTests.Tests.V1.Users.PasswordCredentials;

public sealed class LoginPasswordCredentialsTests : TestBase
{
    public LoginPasswordCredentialsTests(ITestOutputHelper output, TestContext context) 
        : base(context, output)
    {
        Context.Preparer.Users.Register();
    }

    [Fact]
    public async Task Request_ShouldSuccess()
    {
        var client = Context.Client;

        var request = new LoginPasswordCredentialsRequest(UserPreparer.LoginValue, UserPreparer.PasswordValue);
        
        var result = await client.Users.LoginAsync(request);
        
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task Request_ShouldFail_WhenLoginInvalid()
    {
        var client = Context.Client;
        
        var request = new LoginPasswordCredentialsRequest("invalid", UserPreparer.PasswordValue);
        
        var result = await client.Users.LoginAsync(request);
        
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.Forbidden, result.StatusCode);
        Assert.Null(result.Value);
    }
    
    [Fact]
    public async Task Request_ShouldFail_WhenPasswordInvalid()
    {
        var client = Context.Client;
        
        var request = new LoginPasswordCredentialsRequest(UserPreparer.LoginValue, "invalid");
        
        var result = await client.Users.LoginAsync(request);

        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.Forbidden, result.StatusCode);
        Assert.Null(result.Value);
    }
}