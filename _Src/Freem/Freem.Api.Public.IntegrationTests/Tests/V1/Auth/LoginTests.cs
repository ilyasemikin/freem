using System.Net;
using System.Net.Mime;
using System.Text;
using Freem.Domain;
using Freem.Api.Public.IntegrationTests.Context;
using Freem.Api.Public.IntegrationTests.Extensions;
using Freem.Api.Public.IntegrationTests.Tests.Base;

namespace Freem.Api.Public.IntegrationTests.Tests.V1.Auth;

public sealed class LoginTests : TestBase
{
    public LoginTests(TestContext context) 
        : base(context)
    {
    }

    [Fact]
    public async Task Request_ShouldReturnInvalidRequestError_WhenBodyMissesFields()
    {
        using var client = Context.CreateHttpClient();
        using var content = new StringContent("{}", Encoding.UTF8, MediaTypeNames.Application.Json);

        using var response = await client.PostAsync("api/v1/auth/login", content);

        await response.ShouldHaveErrorAsync(HttpStatusCode.BadRequest, DomainErrorCatalog.Types.InvalidRequest);
    }
}
