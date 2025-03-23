using System.Net;
using Freem.Web.Api.Public.Contracts.Users.LoginPassword;
using Freem.Web.Api.Public.FunctionalTests.Context;
using Freem.Web.Api.Public.FunctionalTests.Context.Preparers;
using Freem.Web.Api.Public.FunctionalTests.Tests.Base;

namespace Freem.Web.Api.Public.FunctionalTests.Tests.V1.Users.PasswordCredentials;

public sealed class UpdatePasswordCredentialsTests : TestBase
{
    public UpdatePasswordCredentialsTests(TestContext context) 
        : base(context)
    {
        Context.Preparer.Users.Register();
        
        var lr = new LoginPasswordCredentialsRequest(UserPreparer.LoginValue, UserPreparer.PasswordValue);
        var response = Context.SyncClient.Users.Login(lr);
        
        response.EnsureSuccess();
        Context.TokenLoader.Update(response.Value.Tokens);
    }

    [Fact]
    public async Task Request_ShouldSuccess()
    {
        var request = new UpdatePasswordCredentialsRequest(UserPreparer.PasswordValue, "new");

        var response = await Context.Client.Users.UpdatePasswordCredentialsAsync(request);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Request_ShouldFail_WhenOldPasswordIsInvalid()
    {
        var request = new UpdatePasswordCredentialsRequest("invalid", "new");
        
        var response = await Context.Client.Users.UpdatePasswordCredentialsAsync(request);
        
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
}