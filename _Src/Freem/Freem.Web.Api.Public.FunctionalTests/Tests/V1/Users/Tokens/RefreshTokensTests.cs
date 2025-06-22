using System.Net;
using Freem.Web.Api.Public.Contracts.DTO.Users.Tokens;
using Freem.Web.Api.Public.FunctionalTests.Context;
using Freem.Web.Api.Public.FunctionalTests.Tests.Base;

namespace Freem.Web.Api.Public.FunctionalTests.Tests.V1.Users.Tokens;

public sealed class RefreshTokensTests : TestBase
{
    private readonly string _token;
    
    public RefreshTokensTests(TestContext context) 
        : base(context)
    {
        Context.Preparer.Users.Register();
        Context.Preparer.Users.Login();

        if (!Context.TokenLoader.Authorized)
            throw new InvalidOperationException();
        
        _token = Context.TokenLoader.Tokens.AccessToken;
    }

    [Fact]
    public async Task Request_ShouldSuccess()
    {
        var request = new RefreshTokensRequest(_token);

        var response = await Context.Client.Users.RefreshTokensAsync(request);
        
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(response.Value);
    }

    [Fact]
    public async Task Request_ShouldFail_WhenTokenIsInvalid()
    {
        var token = Guid.NewGuid().ToString();
        var request = new RefreshTokensRequest(token);
        
        var response = await Context.Client.Users.RefreshTokensAsync(request);
        
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        Assert.Null(response.Value);
    }
}